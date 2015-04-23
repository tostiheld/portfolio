using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Roadplus.Server.API
{
    [JsonObject]
    public class Response
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ResponseType Type { get; private set; }

        [JsonProperty("activity-type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ActivityType FromActivity { get; private set; }

        [JsonProperty("message")]
        public string Message { get; private set; }

        [JsonProperty("payload")]
        public object[] Payload { get; private set; }

        [JsonIgnore]
        public string DestinationAddress { get; private set; }
        [JsonIgnore]
        public bool Broadcast { get; private set; }
        [JsonIgnore]
        public LinkType? BroadcastTo { get; private set; }

        public Response(ResponseType type, ActivityType fromactivity, string message, string dest)
        {
            Type = type;
            FromActivity = fromactivity;
            Message = message;
            Payload = null;

            DestinationAddress = dest;
            Broadcast = false;
            BroadcastTo = null;
        }

        public Response(ResponseType type, ActivityType fromactivity, string message, LinkType to)
        {
            Type = type;
            FromActivity = fromactivity;
            Message = message;
            Payload = null;

            DestinationAddress = null;
            Broadcast = true;
            BroadcastTo = to;
        }

        public Response(ResponseType type, ActivityType fromactivity, object[] payload, string dest)
        {
            Type = type;
            FromActivity = fromactivity;
            Message = "";
            Payload = payload;

            DestinationAddress = dest;
            Broadcast = false;
            BroadcastTo = null;
        }

        public Response(ResponseType type, ActivityType fromactivity, object[] payload, LinkType to)
        {
            Type = type;
            FromActivity = fromactivity;
            Message = "";
            Payload = payload;

            DestinationAddress = null;
            Broadcast = true;
            BroadcastTo = to;
        }

        public string Format(IFormatHandler handler)
        {
            return handler.Format(this);
        }
    }
}

