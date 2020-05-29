using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Server;
using Server.Services;

namespace Tema3
{
    public class ChatService : ChatServices.ChatServicesBase
    {
        private readonly ILogger<ChatService> _logger;
        private static ChatRoomService chatRoomService=new ChatRoomService();
        public ChatService(ILogger<ChatService> logger)
        {
            _logger = logger;
        }
        
        public override Task<JoinClientReply> JoinClientChat(JoinClientRequest request, ServerCallContext context)
        {
            chatRoomService.AddClientToChatRoomAsync(request.ClientDetails);
            return Task.FromResult(new JoinClientReply { });
        }
        public override async Task SendMessageInChat(Grpc.Core.IAsyncStreamReader<ChatMessage> requestStream,
            Grpc.Core.IServerStreamWriter<ChatMessage> responseStream, Grpc.Core.ServerCallContext context)
        { 
            if (!await requestStream.MoveNext())
            {
                return;
            }
            
            //logger.LogInformation($"{user} connected");

            chatRoomService.ConnectClientToChatRoom(Guid.Parse(requestStream.Current.ClientId), responseStream);

            while (await requestStream.MoveNext())
            {
                var chatMessage = requestStream.Current;
                
                foreach (var client in chatRoomService.chatRoom.ClientsInRoom)
                {
                    await client.Stream.WriteAsync(chatMessage);
                }
            }

            chatRoomService.DisconnectClient(Guid.Parse(requestStream.Current.ClientId));
        }
    }
}
