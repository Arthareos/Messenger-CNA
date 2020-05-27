using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Server;
namespace Tema3
{
    public class ChatService : ChatServices.ChatServicesBase
    {
        private readonly ILogger<ChatService> _logger;
        public ChatService(ILogger<ChatService> logger)
        {
            _logger = logger;
        }
        /*List<ChatMessage> addChatMessages(ChatMessage chatMessage)
        {

        }*/
        public override async Task SendMessageInChat(Grpc.Core.IAsyncStreamReader<ChatMessage> requestStream,
            Grpc.Core.IServerStreamWriter<ChatMessage> responseStream, Grpc.Core.ServerCallContext context)
        {
            while (await requestStream.MoveNext())
            {
                //var note = requestStream.Current;
                var chatMessage = requestStream.Current;
                List<ChatMessage> prevMessages = new List<ChatMessage>();
                prevMessages.Add(chatMessage);
                /*List<ChatMessage> prevNotes = AddNoteForLocation(note.Location, note);*/
                foreach (var message in prevMessages)
                {
                    await responseStream.WriteAsync(chatMessage);
                }
            }
        }
    }
}
