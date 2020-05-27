using Grpc.Core;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public class ChatRoomService
    {
		private readonly ChatRoom chatRoom;

		public ChatRoomService(ChatRoom chatRoom)
		{
			this.chatRoom = chatRoom;
		}

		public async Task BroadcastMessageAsync(ChatMessage message)
		{
			foreach (var client in this.chatRoom.ClientsInRoom)
			{
				await client.Stream.WriteAsync(message);
				Console.WriteLine($"Sent message from {message.ClientName} to {client.Name}");
			}
		}

		public Task<int> AddClientToChatRoomAsync(ClientDetails customer)
		{
			chatRoom.ClientsInRoom.Add(new Client
			{
				Color = customer.ColorInConsole,
				Name = customer.Name,
				ClientId = int.Parse(customer.Id)
			});
			return Task.FromResult(chatRoom.Id);
		}

		public void ConnectCustomerToChatRoom(int roomId, int customerId, IAsyncStreamWriter<ChatMessage> responseStream)
		{
			chatRoom.ClientsInRoom.FirstOrDefault(c => c.ClientId == customerId).Stream = responseStream;
		}

		public void DisconnectCustomer(int roomId, int customerId)
		{
			chatRoom.ClientsInRoom.Remove(chatRoom.ClientsInRoom.FirstOrDefault(c => c.ClientId == customerId));
		}
	}
}
