using Grpc.Core;
using System;

namespace Server.Models
{
    public class Client
    {
        private Guid m_ID;
        private String m_name;
        private String m_color;
        public IAsyncStreamWriter<ChatMessage> m_stream;
       
        public Client()
        {
            m_name = String.Empty;
            m_color = String.Empty;
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

        public String Name
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

        public String Color
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
