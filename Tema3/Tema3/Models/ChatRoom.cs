using System.Collections.Generic;

namespace Server.Models
{
    public class ChatRoom
    {
        private List<Client> m_clients;

        public ChatRoom()
        {
            m_clients = new List<Client>();
        }

        public List<Client> Clients
        {
            get
            {
                return m_clients;
            }
        }
    }
}
