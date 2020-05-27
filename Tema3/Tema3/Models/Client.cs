using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
    public class Client
    {
        private int clientId;
        private string name;
        private string color;

        public Client()
        {
            clientId = 0;
            name = string.Empty;
            color = string.Empty;
        }

        public int ClientId
        {
            get
            {
                return this.clientId;
            }
            set
            {
                this.clientId = value;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public string Color
        {
            get
            {
                return this.color;
            }
            set
            {
                this.color = value;
            }
        }

    }
}
