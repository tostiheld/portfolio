﻿using System;
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
            Command = command;
            ErrorMessage = message;
        }

        [JsonProperty("response")]
        public string ResponseString { get { return "error"; } }

        [JsonProperty("error-message")]
        public string ErrorMessage { get; set; }

        [JsonProperty("command")]
        public string Command { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

