using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            /*while (await requestStream.MoveNext())
            {
                var chatMessage = requestStream.Current;
                List<ChatMessage> prevMessages = new List<ChatMessage>();
                prevMessages.Add(chatMessage);
                foreach (var message in prevMessages)
                {
                    await responseStream.WriteAsync(chatMessage);
                }
            }*/

            if (!await requestStream.MoveNext())
            {
                return;
            }
            /*
            chatRoomService.ConnectClientToChatRoom(requestStream.Current.RoomId, Guid.Parse(requestStream.Current.ClientId), responseStream);
            _logger.LogInformation($"{user} connected"); */
            chatRoomService.ConnectClientToChatRoom( Guid.Parse(requestStream.Current.ClientId), responseStream);
             while (await requestStream.MoveNext())
            {
                var chatMessage = requestStream.Current;
                
                foreach (var client in chatRoomService.chatRoom.ClientsInRoom)
                {
                    await client.Stream.WriteAsync(chatMessage);
                }
            }
            
        }
    }
}
