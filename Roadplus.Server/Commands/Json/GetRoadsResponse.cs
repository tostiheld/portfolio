using System;

using Newtonsoft.Json;

using Roadplus.Server.API;

namespace Roadplus.Server.Commands.Json
{
    [JsonObject]
    public class GetRoadsResponse : IResponse
    {
        [JsonProperty("response")]
        public string ResponseString { get { return "OK"; } }

        [JsonProperty("command")]
        public string Command
        {
            get
            {
                return "getRoads";
            }
            set { }
        }

        [JsonProperty("ports")]
        public string[] Ports { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

