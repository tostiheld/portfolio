using System;

namespace Roadplus.Server.Communication
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public Message Received { get; private set; }

        public MessageReceivedEventArgs(Message message)
        {
            Received = message;
        }
    }
}

