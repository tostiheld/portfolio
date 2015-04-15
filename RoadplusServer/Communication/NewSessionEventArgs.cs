using System;

namespace Roadplus.Server.Communication
{
    public class NewSessionEventArgs : EventArgs
    {
        public WSSession Session { get; private set; }

        public NewSessionEventArgs(WSSession session)
        {
            Session = session;
        }
    }
}

