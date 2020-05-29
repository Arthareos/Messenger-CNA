using Grpc.Net.Client;
using System;
using System.Threading.Tasks;
using Server;
using Grpc.Core;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //var channel = GrpcChannel.ForAddress("https://arthareos.go.ro:5002");
            var channel = GrpcChannel.ForAddress("https://localhost:5002");
            var client = new ChatServices.ChatServicesClient(channel);

            Console.Write("Introduceti Numele: ");
            string nume = Console.ReadLine();

            var clientDetails = new ClientDetails
            {
                ColorInConsole = GetRandomChatColor(),
                Id = Guid.NewGuid().ToString(),
                Name = args.Length > 0 ? args[0] : nume
            };
            
            var joinClientRequest = new JoinClientRequest
            {
                ClientDetails = clientDetails
            };

            var joinClientReply = await client.JoinClientChatAsync(joinClientRequest);
            using var streaming = client.SendMessageInChat(new Metadata { new Metadata.Entry("CustomerName", clientDetails.Name) });

            var response = Task.Run(async () =>
            {
                while (await streaming.ResponseStream.MoveNext())
                {
                    Console.ForegroundColor = Enum.Parse<ConsoleColor>(streaming.ResponseStream.Current.Color);
                    Console.WriteLine($"{streaming.ResponseStream.Current.ClientName}: {streaming.ResponseStream.Current.Message}");
                    Console.ForegroundColor = Enum.Parse<ConsoleColor>(clientDetails.ColorInConsole);
                }
            });

            await streaming.RequestStream.WriteAsync(new ChatMessage
            {
                ClientId = clientDetails.Id,
                Color = clientDetails.ColorInConsole,
                Message = "",
                ClientName = clientDetails.Name
            });

            var line = Console.ReadLine();
            //DeletePrevConsoleLine();

            //Pana cand nu vad cuvantul magic "qw!" eu trimit mesaje
            while (!string.Equals(line, "qw!", StringComparison.OrdinalIgnoreCase))
            {
                var messageDetails = new ChatMessage
                {
                    ClientName = clientDetails.Name,
                    ClientId = clientDetails.Id,
                    Color = clientDetails.ColorInConsole,
                    Message = line
                };

                await streaming.RequestStream.WriteAsync(messageDetails);
                line = Console.ReadLine();
                //De sters cu set cursor position si overwrite cu WriteLine
            }

            await streaming.RequestStream.CompleteAsync();
        }

        private static string GetRandomChatColor()
        {
            var colors = Enum.GetValues(typeof(ConsoleColor));
            var rnd = new Random();
            return colors.GetValue(rnd.Next(1, colors.Length - 1)).ToString();
        }
    }
}
