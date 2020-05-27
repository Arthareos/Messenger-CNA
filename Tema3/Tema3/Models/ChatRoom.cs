using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
    public class ChatRoom
    {
        private List<Client> clientsInRoom;
        public List<Client> ClientsInRoom
        {
            get
            {
                return clientsInRoom;
            }
        }
        public ChatRoom()
        {
            clientsInRoom = new List<Client>();
        }
    }
}
