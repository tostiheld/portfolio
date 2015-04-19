using System;

namespace Roadplus.Server.Traffic
{
    public class NewConnectionEventArgs : EventArgs
    {
        public Vertex End { get; private set; }
        public Edge Connection { get; private set; }

        public NewConnectionEventArgs(Vertex end, Edge connection)
            : base()
        {
            End = end;
            Connection = connection;
        }
    }
}

