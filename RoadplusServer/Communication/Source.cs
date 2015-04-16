using System;
using System.Net;

namespace Roadplus.Server.Communication
{
    public struct Source
    {
        public SourceTypes Type { get; set; }
        public IPAddress IP { get; set; }
        public string Port { get; set; }

        public Source(SourceTypes type, IPAddress ip) : this()
        {
            Type = type;
            IP = ip;
        }

        public Source(SourceTypes type, string port) : this()
        {
            Type = type;
            Port = port;
        }
    }
}

