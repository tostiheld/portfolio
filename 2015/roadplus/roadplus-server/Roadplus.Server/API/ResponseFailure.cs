using System;
using Newtonsoft.Json;

namespace Roadplus.Server.API
{
    [JsonObject]
    public class ResponseFailure : IResponse
    {
        public ResponseFailure()
        { }

        public ResponseFailure(string command, string message)
        {
            if (command == null ||
                message == null)
            {
                throw new ArgumentNullException();
            }

            Command = command;
            ErrorMessage = message;
        }

        [JsonProperty("response")]
        public string ResponseString { get { return "error"; } }

        [JsonProperty("error-message")]
        public string ErrorMessage { get; set; }

        [JsonProperty("command")]
        public string Command { get; set; }
    }
}

