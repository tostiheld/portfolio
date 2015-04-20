using System;
using System.Runtime.Serialization;

namespace Roadplus.Server.Communication.Protocol
{
    [DataContract]
    public class Response
    {
        [DataMember(Name="type")]
        public ResponseType Type { get; private set; }
        [DataMember(Name="from")]
        public string From 
        { 
            get
            {
                return fromType.Name;
            }
            private set
            {
                // required for datacontract
            }
        }
        [DataMember(Name="content")]
        public string[] Content { get; private set; }
        public Link To { get; private set; }
        public bool Broadcast { get; private set; }
        public LinkType? BroadcastTo { get; private set; }

        private Type fromType;

        public Response(ResponseType type, Link to, Type from, string[] content)
        {
            Type = type;
            To = to;
            fromType = from;
            Content = content;
            Broadcast = false;
            BroadcastTo = null;
        }

        public Response(ResponseType type, Type from, string[] content)
        {
            Type = type;
            To = null;
            fromType = from;
            Content = content;
            Broadcast = true;
            BroadcastTo = null;
        }

        public Response(ResponseType type, Type from, string[] content, LinkType broadcasto)
        {
            Type = type;
            To = null;
            fromType = from;
            Content = content;
            Broadcast = true;
            BroadcastTo = broadcasto;
        }

        public string Format(FormatHandler handler)
        {
            return handler.Format(this);
        }
    }
}

