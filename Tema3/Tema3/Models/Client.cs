﻿using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
    public class Client
    {
        private Guid clientId;
        private string name;
        private string color;
        public IAsyncStreamWriter<ChatMessage> stream;
       
        public Client()
        {
            name = string.Empty;
            color = string.Empty;
        }
        public IAsyncStreamWriter<ChatMessage> Stream 
        {
            get 
            {
                return this.stream;
            }
            set
            {
                this.stream = value;
            }
        }
        public Guid ClientId
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
