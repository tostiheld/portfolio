using System;

using Roadplus.Server.API;

using Newtonsoft.Json;

namespace Roadplus.Server.Commands.Json
{
    [JsonObject]
    public class RemoveResponse : IResponse
    {
        [JsonProperty("reponse")]
        public string ResponseString { get { return "OK"; } }

        [JsonProperty("command")]
        public string Command { get; set; }

        [JsonProperty("removedObject")]
        public int RemovedObjectId { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

