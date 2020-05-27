using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
    public class ChatRoom
    {
        private int id;
        private List<Client> clientsInRoom;
        public int Id
        {
            get
            {
                return id;
            }

        }
        public List<Client> ClientsInRoom
        {
            get
            {
                return clientsInRoom;
            }
        }
        public ChatRoom(int id)
        {
            this.id = id;
            clientsInRoom = new List<Client>();
        }
    }
}
