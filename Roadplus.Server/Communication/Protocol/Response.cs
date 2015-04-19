using System;

namespace Roadplus.Server.Communication.Protocol
{
    public class Response : IFormattable
    {
        public ResponseType Type { get; private set; }
        public Link To { get; private set; }
        public Type From { get; private set; }
        public string[] Content { get; private set; }
        public bool Broadcast { get; private set; }
        public LinkType? BroadcastTo { get; private set; }

        public Response(ResponseType type, Link to, Type from, string[] content)
        {
            Type = type;
            To = to;
            From = from;
            Content = content;
            Broadcast = false;
            BroadcastTo = null;
        }

        public Response(ResponseType type, Type from, string[] content)
        {
            Type = type;
            To = null;
            From = from;
            Content = content;
            Broadcast = true;
            BroadcastTo = null;
        }

        public Response(ResponseType type, Type from, string[] content, LinkType broadcasto)
        {
            Type = type;
            To = null;
            From = from;
            Content = content;
            Broadcast = true;
            BroadcastTo = broadcasto;
        }

        #region IFormattable implementation

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return String.Format(formatProvider, format, this);
        }

        #endregion
    }
}

