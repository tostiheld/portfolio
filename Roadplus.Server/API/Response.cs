using System;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Roadplus.Server.API
{
    [JsonObject]
    public class Response
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ResponseType Type { get; set; }

        [JsonProperty("activity-type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ActivityType FromActivity { get; private set; }

        [JsonProperty("message")]
        public string Message
        { 
            get
            {
                return message;
            }
            set
            {
                PayloadList = null;
                message = value;
            }
        }

        [JsonProperty("payload")]
        public object[] Payload 
        { 
            get
            {
                if (PayloadList != null)
                {
                    return PayloadList.ToArray();
                }

                return null;
            }
        }


        [JsonIgnore]
        public List<object> PayloadList { get; set; }
        [JsonIgnore]
        public string DestinationAddress { get; private set; }
        [JsonIgnore]
        public bool Broadcast { get; private set; }
        [JsonIgnore]
        public LinkType? BroadcastTo { get; private set; }

        private string message;

        public Response()
            : this("")
        { }

        public Response(string dest)
            : this(ActivityType.Unknown, dest)
        { }

        public Response(ActivityType fromactivity, string dest)
        {
            Type = ResponseType.Acknoledge;
            FromActivity = fromactivity;
            Message = "";
            DestinationAddress = dest;
        }

        public void SetBroadcast(LinkType to)
        {
            Broadcast = true;
            BroadcastTo = to;
            DestinationAddress = null;
        }

        public void UnsetBroadCast(string dest)
        {
            Broadcast = false;
            BroadcastTo = null;
            DestinationAddress = dest;
        }

        public string Format(IFormatHandler handler)
        {
            return handler.Format(this);
        }
    }
}

