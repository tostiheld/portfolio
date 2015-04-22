using System;

using Roadplus.Server.EntityManagement;

namespace Roadplus.Server.Communication.Protocol
{
    public class Response
    {
        // json: type
        public ResponseType Type { get; private set; }
        // json: activity-type
        public ActivityType FromActivity { get; private set; }
        // json: message
        public string Message { get; private set; }
        // json: payload
        public object[] Payload { get; private set; }

        public Link To { get; private set; }
        public bool Broadcast { get; private set; }
        public LinkType? BroadcastTo { get; private set; }

        public Response(ResponseType type, ActivityType fromactivity, string message, Link to)
        {
            Type = type;
            FromActivity = fromactivity;
            Message = message;
            Payload = null;

            To = to;
            Broadcast = false;
            BroadcastTo = null;
        }

        public Response(ResponseType type, ActivityType fromactivity, string message, LinkType to)
        {
            Type = type;
            FromActivity = fromactivity;
            Message = message;
            Payload = null;

            To = null;
            Broadcast = true;
            BroadcastTo = to;
        }

        public Response(ResponseType type, ActivityType fromactivity, object[] payload, Link to)
        {
            Type = type;
            FromActivity = fromactivity;
            Message = "";
            Payload = payload;

            To = to;
            Broadcast = false;
            BroadcastTo = null;
        }

        public Response(ResponseType type, ActivityType fromactivity, object[] payload, LinkType to)
        {
            Type = type;
            FromActivity = fromactivity;
            Message = "";
            Payload = payload;

            To = null;
            Broadcast = true;
            BroadcastTo = to;
        }

        public string Format(FormatHandler handler)
        {
            return handler.Format(this);
        }
    }
}

