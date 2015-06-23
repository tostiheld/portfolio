using System;

using Newtonsoft.Json;

using Roadplus.Server.API;

namespace Roadplus.Server.Commands.Json
{
    [JsonObject]
    public class ConnectRoadToZoneResponse : IResponse
    {
        [JsonProperty("response")]
        public string ResponseString
        {
            get
            {
                return "OK";
            }
        }

        [JsonProperty("command")]
        public string Command
        {
            get
            {
                return "connectRoad";
            }
            set
            { }
        }

        [JsonProperty("zoneId")]
        public int ZoneId { get; set;}

        [JsonProperty("roadPort")]
        public string RoadPort { get; set;}

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

