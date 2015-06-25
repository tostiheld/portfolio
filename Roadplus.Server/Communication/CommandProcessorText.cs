using System;
using System.Linq;

using Roadplus.Server.API;
using System.Collections.Generic;

namespace Roadplus.Server.Communication
{
    public class CommandProcessorText : CommandProcessor
    {
        public CommandProcessorText()
        {
        }

        public override IResponse Process(string command)
        {
            string trimmed = command.Trim('>', ';');
            string first = trimmed.Split(':')[0].ToLower();
            List<string> split = trimmed.Split(':').ToList();
            split.RemoveAt(0);

            string args = "";
            foreach (string s in split)
            {
                args += s + ":";
            }

            if (first == "message")
            {
                return null;
            }

            try
            {
                foreach (ICommand c in RegisteredCommands)
                {
                    if (c.Name.ToLower() == first)
                    {
                        return c.Execute(args);
                    }
                }

                return new ResponseFailure(first, "Command not found");
            }
            catch (Exception ex)
            {
                return new ResponseFailure("unknown", ex.Message);
            }
        }
    }
}

