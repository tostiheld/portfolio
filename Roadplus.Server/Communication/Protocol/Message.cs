using System;

namespace Roadplus.Server.Communication.Protocol
{
    public class Message
    {
        public Link From { get; private set; }
        public string Format { get; private set; }
        public string Content { get; private set; }

        public Message(Link from, string format, string content)
        {
            From = from;
            Format = format;
            Content = content;
        }
    }
}

