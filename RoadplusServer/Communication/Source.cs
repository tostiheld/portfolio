using System;
using System.Net;

namespace Roadplus.Server.Communication
{
    public struct Source
    {
        public SourceType Type { get; set; }
        public IPAddress IP { get; set; }

        public Source(SourceType type, IPAddress ip) : this()
        {
            Type = type;
            IP = ip;
        }
    }
}

