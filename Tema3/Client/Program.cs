using Grpc.Net.Client;
using System;
using System.Threading.Tasks;
using Tema3;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            bool isValid = false;
            string name;
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Greeter.GreeterClient(channel);

            name = Console.ReadLine();


            var clientBirthDate = new HelloRequest { Name = name };
            var reply = await client.SayHelloAsync(clientBirthDate);
            Console.WriteLine(reply.Message);
            Console.ReadLine();
        }
    }
}
