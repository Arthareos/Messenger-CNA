using System;
using System.Collections.Generic;
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
        private ChatRoomService chatRoomService;
        public ChatService(ILogger<ChatService> logger)
        {
            _logger = logger;
        }

        public override async Task<JoinClientReply> JoinClientChat(JoinClientRequest request, ServerCallContext context)
        {
            return new JoinClientReply { RoomId = await chatRoomService.AddClientToChatRoomAsync(request.ClientDetails) };
        }
        public override async Task SendMessageInChat(Grpc.Core.IAsyncStreamReader<ChatMessage> requestStream,
            Grpc.Core.IServerStreamWriter<ChatMessage> responseStream, Grpc.Core.ServerCallContext context)
        {
            while (await requestStream.MoveNext())
            {
                var chatMessage = requestStream.Current;
                List<ChatMessage> prevMessages = new List<ChatMessage>();
                prevMessages.Add(chatMessage);
                foreach (var message in prevMessages)
                {
                    await responseStream.WriteAsync(chatMessage);
                }
            }
        }
    }
}
