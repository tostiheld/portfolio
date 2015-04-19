using System;
using Roadplus.Server.Communication.Protocol;

namespace Roadplus.Server
{
    public class MessageFoundEventArgs : EventArgs
    {
        public Message FoundMessage { get; private set; }

        public MessageFoundEventArgs(Message found) : base()
        {
            FoundMessage = found;
        }
    }
}

