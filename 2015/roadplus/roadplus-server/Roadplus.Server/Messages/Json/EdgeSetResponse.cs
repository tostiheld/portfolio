using System;

using Newtonsoft.Json;

using Roadplus.Server.API;
using Roadplus.Server.Data;

namespace Roadplus.Server.Messages.Json
{
    [JsonObject]
    public class EdgeSetResponse : IResponse
    {
        [JsonProperty("response")]
        public string ResponseString { get { return "OK"; } }

        [JsonProperty("command")]
        public string Command { get { return "createEdgeSet"; } set { } }

        [JsonProperty("zoneId")]
        public int ZoneId { get; set; }

        [JsonProperty("createdEdge")]
        public Edge CreatedEdge { get; set; }

        [JsonProperty("startVertex")]
        public Vertex StartVertex { get; set; }

        [JsonProperty("endVertex")]
        public Vertex EndVertex { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

