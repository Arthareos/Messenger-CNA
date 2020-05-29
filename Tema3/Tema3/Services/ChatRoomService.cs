using System;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Server.Models;

namespace Server.Services
{
    public class ChatRoomService
    {
		public ChatRoom chatRoom;

		public ChatRoomService()
		{
			chatRoom = new ChatRoom();
		}

		public async Task BroadcastMessageAsync(ChatMessage message)
		{
			foreach (var client in this.chatRoom.Clients)
			{
				await client.Stream.WriteAsync(message);
			}
		}

		public void AddClientToChatRoomAsync(ClientDetails client)
		{
			chatRoom.Clients.Add(new Client
			{
				Color = client.ColorInConsole,
				Name = client.Name,
				ID = Guid.Parse(client.Id)
			});
		}

		public void ConnectClientToChatRoom(Guid customerId, IAsyncStreamWriter<ChatMessage> responseStream)
		{
			chatRoom.Clients.FirstOrDefault(c => c.ID == customerId).Stream = responseStream;
		}

		public void DisconnectClient(Guid clientId)
		{
			chatRoom.Clients.Remove(chatRoom.Clients.FirstOrDefault(c => c.ID == clientId));
		}
	}
}
