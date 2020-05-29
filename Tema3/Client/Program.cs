using System;
using System.Threading.Tasks;
using Server;
using Grpc.Core;
using Grpc.Net.Client;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Face ca aplicatia sa mearga fara https ~Simone
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            //var channel = GrpcChannel.ForAddress("http://arthareos.go.ro:8888");
            var channel = GrpcChannel.ForAddress("http://localhost:8888");
            var client = new ChatServices.ChatServicesClient(channel);

            Console.Write("Introduceti Numele: ");
            string nume = Console.ReadLine();
            Console.Clear();

            //Create client
            ClientDetails clientDetails = new ClientDetails
            {
                ColorInConsole = GetRandomChatColor(),
                Id = Guid.NewGuid().ToString(),
                Name = args.Length > 0 ? args[0] : nume
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
                        String aux = streaming.ResponseStream.Current.Message;
                        aux = aux.Replace(" ", string.Empty);

                        if (!streaming.ResponseStream.Current.Message.Equals("") && !aux.Equals(""))
                        {
                            Console.ForegroundColor = Enum.Parse<ConsoleColor>(streaming.ResponseStream.Current.Color);

                            if (streaming.ResponseStream.Current.ClientName == clientDetails.Name)
                            {
                                Console.WriteLine($"You: {streaming.ResponseStream.Current.Message}");
                            }
                            else
                            {
                                Console.WriteLine($"{streaming.ResponseStream.Current.ClientName}: {streaming.ResponseStream.Current.Message}");
                            }

                            Console.ForegroundColor = Enum.Parse<ConsoleColor>(clientDetails.ColorInConsole);
                        }
                    }
                });

                await streaming.RequestStream.WriteAsync(new ChatMessage
                {
                    ClientId = clientDetails.Id,
                    Color = clientDetails.ColorInConsole,
                    Message = "",
                    ClientName = clientDetails.Name
                });

                Console.ForegroundColor = Enum.Parse<ConsoleColor>(clientDetails.ColorInConsole);
                String line = Console.ReadLine();
                Console.CursorTop -= 1;

                // preia mesajul scris de client
                while (true)
                {
                    ChatMessage messageDetails = new ChatMessage
                    {
                        ClientName = clientDetails.Name,
                        ClientId = clientDetails.Id,
                        Color = clientDetails.ColorInConsole,
                        Message = line
                    };

                    await streaming.RequestStream.WriteAsync(messageDetails);

                    if (string.Equals(line, "qw!", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }

                    line = Console.ReadLine();
                    Console.CursorTop -= 1;
                }

                await streaming.RequestStream.CompleteAsync();
            }
        }

        private static string GetRandomChatColor()
        {
            var colors = Enum.GetValues(typeof(ConsoleColor));
            var rnd = new Random();
            return colors.GetValue(rnd.Next(1, colors.Length - 1)).ToString();
        }
    }
}
