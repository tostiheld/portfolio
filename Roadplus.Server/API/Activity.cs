using System;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Roadplus.Server.API
{
    [JsonObject]
    public class Activity
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ActivityType Type { get; private set; }

        [JsonProperty("payload")]
        public object[] Payload { get; set; }

        [JsonIgnore]
        public string SourceAddress { get; private set; }
        [JsonIgnore]
        public LinkType SourceType { get; private set; }
        [JsonIgnore]
        public List<Type> TargetTypes { get; set; }

        public Activity(ActivityType type, string sourceaddress, LinkType sourcetype)
        {
            Type = type;
            SourceAddress = sourceaddress;
            SourceType = sourcetype;

            Payload = null;
            TargetTypes = new List<Type>()
        }
    }
}

