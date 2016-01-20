using System;

using Newtonsoft.Json;

using Roadplus.Server.API;

namespace Roadplus.Server.Messages.Json
{
    [JsonObject]
    public class CreateResponse : IResponse
    {
        [JsonProperty("command")]
        public string Command { get; set;}

        [JsonProperty("response")]
        public string ResponseString { get { return "OK"; } }

        [JsonProperty("createdObject")]
        public object CreatedObject { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

