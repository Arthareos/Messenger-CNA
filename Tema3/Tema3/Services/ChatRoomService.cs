using Grpc.Core;
using Server.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public class ChatRoomService
    {
		public readonly ChatRoom chatRoom;

		public ChatRoomService()
		{
			chatRoom = new ChatRoom();
		}

		public async Task BroadcastMessageAsync(ChatMessage message)
		{
			foreach (var client in this.chatRoom.ClientsInRoom)
			{
				await client.Stream.WriteAsync(message);
			}
		}

		public void AddClientToChatRoomAsync(ClientDetails client)
		{
			chatRoom.ClientsInRoom.Add(new Client
			{
				Color = client.ColorInConsole,
				Name = client.Name,
				ClientId = Guid.Parse(client.Id)
			});
		}

		public void ConnectClientToChatRoom( Guid customerId, IAsyncStreamWriter<ChatMessage> responseStream)
		{
			chatRoom.ClientsInRoom.FirstOrDefault(c => c.ClientId == customerId).Stream = responseStream;
		}

		public void DisconnectClient( Guid clientId)
		{
			chatRoom.ClientsInRoom.Remove(chatRoom.ClientsInRoom.FirstOrDefault(c => c.ClientId == clientId));
		}
	}
}
