﻿using System;
using System.Windows;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Server;

namespace ClientUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            StartClient();
        }

        static async Task StartClient()
        {
            // Face ca aplicatia sa mearga fara https ~Simone
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            //var channel = GrpcChannel.ForAddress("http://arthareos.go.ro:8888");
            GrpcChannel channel = GrpcChannel.ForAddress("http://localhost:8888");
            var client = new ChatServices.ChatServicesClient(channel);

            String name = ReadClientName();

            //Create client
            ClientDetails clientDetails = new ClientDetails
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                ColorInConsole = GetRandomChatColor()
            };

            JoinClientRequest joinClientRequest = new JoinClientRequest { ClientDetails = clientDetails };
            JoinClientReply joinClientReply = await client.JoinClientChatAsync(joinClientRequest);

            //Scrie mesajele in consola
            using (var streaming = client.SendMessageInChat(new Metadata { new Metadata.Entry("CustomerName", clientDetails.Name) }))
            {
                Task response = Task.Run(async () =>
                {
                    while (await streaming.ResponseStream.MoveNext())
                    {
                        Console.ForegroundColor = Enum.Parse<ConsoleColor>(streaming.ResponseStream.Current.Color);

                        if (streaming.ResponseStream.Current.ClientName == clientDetails.Name)
                        {
                            Console.Write($"You: ");
                        }
                        else
                        {
                            Console.Write($"{streaming.ResponseStream.Current.ClientName}: ");
                        }

                        Console.WriteLine($"{streaming.ResponseStream.Current.Message};");
                        Console.ForegroundColor = Enum.Parse<ConsoleColor>(clientDetails.ColorInConsole);
                    }
                });

                await streaming.RequestStream.WriteAsync(new ChatMessage
                {
                    ClientId = clientDetails.Id,
                    ClientName = clientDetails.Name,
                    Color = clientDetails.ColorInConsole,
                    Message = ""
                });

                Console.ForegroundColor = Enum.Parse<ConsoleColor>(clientDetails.ColorInConsole);
                String line = Console.ReadLine();
                Console.CursorTop -= 1;

                //Preia mesajul scris de client
                while (true)
                {
                    if (!IsStringEmpty(line))
                    {
                        ChatMessage messageDetails = new ChatMessage
                        {
                            ClientId = clientDetails.Id,
                            ClientName = clientDetails.Name,
                            Color = clientDetails.ColorInConsole,
                            Message = line
                        };

                        await streaming.RequestStream.WriteAsync(messageDetails);
                    }

                    if (string.Equals(line, "qw!", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }

                    line = Console.ReadLine();
                    line = line.Trim();
                    Console.CursorTop -= 1;
                }

                await streaming.RequestStream.CompleteAsync();
            }
        }

        public static String GetRandomChatColor()
        {
            Array colors = Enum.GetValues(typeof(ConsoleColor));
            Random random = new Random();
            return colors.GetValue(random.Next(1, colors.Length - 1)).ToString();
        }

        public static bool IsStringEmpty(String message)
        {
            if (!message.Equals("") && !message.Replace(" ", string.Empty).Equals(""))
            {
                return false;
            }

            return true;
        }

        public String ReadClientName()
        {
            txtChat.Text += "\n";
            txtChat.Text += "Enter your name in the text box and send;";

            
            String name = Console.ReadLine();

            while (IsStringEmpty(name))
            {
                Console.Clear();
                Console.Write("Invalid name! Please enter a new one: ");
                name = Console.ReadLine();
            }

            name = name.Trim();
            Console.Clear();
            Console.WriteLine($"Your name: {name};");

            return name;
        }
    }
}
