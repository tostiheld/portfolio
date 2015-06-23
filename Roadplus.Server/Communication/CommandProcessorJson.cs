using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Roadplus.Server.API;

namespace Roadplus.Server.Communication
{
    public class CommandProcessorJson : CommandProcessor
    {
        public const string CommandKey = "command";

        public override IResponse Process(string command)
        {
            JObject o = JsonConvert.DeserializeObject<JObject>(command);

            foreach (ICommand c in RegisteredCommands)
            {
                if (c.Name.ToLower() == o[CommandKey].ToString().ToLower())
                {
                    return c.Execute(command);
                }
            }

            return new ResponseFailure(o[CommandKey].ToString(), "Command not found");
        }
    }
}

