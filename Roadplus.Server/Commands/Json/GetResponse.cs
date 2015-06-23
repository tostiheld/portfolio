using System;

using Newtonsoft.Json;

using Roadplus.Server.API;

namespace Roadplus.Server.Commands.Json
{
    [JsonObject]
    public class GetResponse : IResponse
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("response")]
        public string ResponseString { get { return "OK"; } }

        [JsonProperty("requestedObjects")]
        public object[] RequestedObjects { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

