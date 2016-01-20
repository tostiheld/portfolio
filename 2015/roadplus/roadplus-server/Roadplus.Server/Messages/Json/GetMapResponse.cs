using System;

using Newtonsoft.Json;

using Roadplus.Server.API;
using Roadplus.Server.Data;

namespace Roadplus.Server.Messages.Json
{
    [JsonObject]
    public class GetMapResponse : IResponse
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
                return "requestMap";
            }
            set
            { }
        }

        [JsonProperty("vertices")]
        public Vertex[] Vertices { get; set; }

        [JsonProperty("edges")]
        public Edge[] Edges { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

