using System;
using Newtonsoft.Json;

namespace Roadplus.Server.API
{
    [JsonObject]
    public class ResponseFailure : IResponse
    {
        public ResponseFailure()
        { }

        public ResponseFailure(int id, string message)
        {
            ID = id;
            ErrorMessage = message;
        }

        [JsonProperty("response")]
        public string ResponseString { get { return "error"; } }

        [JsonProperty("error-message")]
        public string ErrorMessage { get; set; }

        [JsonProperty("id")]
        public int ID { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

