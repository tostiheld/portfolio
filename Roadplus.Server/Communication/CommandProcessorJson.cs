﻿using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Roadplus.Server.API;

namespace Roadplus.Server.Communication
{
    public class CommandProcessorJson : CommandProcessor
    {
        public const string CommandKey = "command";
        public const string IDKey = "id";

        #region implemented abstract members of CommandProcessor

        public override IResponse Process(string command)
        {
            JObject o = JsonConvert.DeserializeObject<JObject>(command);

            foreach (ICommand c in RegisteredCommands)
            {
                if (c.Name == o[CommandKey].ToString())
                {
                    return c.Execute(command);
                }
            }

            int id = Convert.ToInt32(o[IDKey]);
            return new ResponseFailure(id, "Command not found");
        }

        #endregion
    }
}

