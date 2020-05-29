using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
    public class Client
    {
        private Guid m_ID;
        private string m_name;
        private string m_color;
        public IAsyncStreamWriter<ChatMessage> m_stream;
       
        public Client()
        {
            m_name = string.Empty;
            m_color = string.Empty;
        }

        public Guid ID
        {
            get
            {
                return this.m_ID;
            }
            set
            {
                this.m_ID = value;
            }
        }

        public string Name
        {
            get
            {
                return this.m_name;
            }
            set
            {
                this.m_name = value;
            }
        }

        public string Color
        {
            get
            {
                return this.m_color;
            }
            set
            {
                this.m_color = value;
            }
        }

        public IAsyncStreamWriter<ChatMessage> Stream
        {
            get
            {
                return this.m_stream;
            }
            set
            {
                this.m_stream = value;
            }
        }
    }
}
