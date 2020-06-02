using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Server.Models;

namespace Server.Services
{
    public class ChatService : ChatServices.ChatServicesBase
    {
        private readonly ILogger<ChatService> m_logger;
        private static ChatRoomService m_chatRoomService = new ChatRoomService();

        public ChatService(ILogger<ChatService> logger)
        {
            m_logger = logger;
        }

        public override Task<JoinClientReply> JoinClientChat(JoinClientRequest request, ServerCallContext context)
        {
            m_chatRoomService.AddClientToChatRoomAsync(request.ClientDetails);
            return Task.FromResult(new JoinClientReply { });
        }

        public override async Task SendMessageInChat(Grpc.Core.IAsyncStreamReader<ChatMessage> requestStream,
            Grpc.Core.IServerStreamWriter<ChatMessage> responseStream, Grpc.Core.ServerCallContext context)
        {
            var auxRequest = requestStream;
            if (!await requestStream.MoveNext())
            {
                return;
            }
            
            foreach(var client in m_chatRoomService.chatRoom.Clients)
            {
                Console.Write("[INFO] " + client.Name + " is connected!\n");
            }

            m_chatRoomService.ConnectClientToChatRoom(Guid.Parse(requestStream.Current.ClientId), responseStream);

            while (await requestStream.MoveNext())
            {
                var chatMessage = requestStream.Current;

                foreach (var client in m_chatRoomService.chatRoom.Clients)
                {
                    if (string.Equals(requestStream.Current.Message, "qw!", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.Write("[INFO] " + client.Name + " disconnected!\n");
                        m_chatRoomService.DisconnectClient(Guid.Parse(auxRequest.Current.ClientId));
                        break;
                    }

                    await client.Stream.WriteAsync(chatMessage);
                }
            }
        }
    }
}
