using System;

namespace Roadplus.Server.API
{
    public class MessageFoundEventArgs : EventArgs
    {
        public RawMessage FoundMessage { get; private set; }

        public MessageFoundEventArgs(RawMessage found) : base()
        {
            FoundMessage = found;
        }
    }
}

