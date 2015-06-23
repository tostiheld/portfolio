using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LinqToDB;
using MySql.Data.MySqlClient;

using Roadplus.Server.API;

namespace Roadplus.Server.Communication
{
    public class CommandProcessorJson : CommandProcessor
    {
        public const string CommandKey = "command";

        public override IResponse Process(string command)
        {
            JObject o = null;

            try
            {
                o = JsonConvert.DeserializeObject<JObject>(command);

                foreach (ICommand c in RegisteredCommands)
                {
                    if (c.Name.ToLower() == o[CommandKey].ToString().ToLower())
                    {
                        return c.Execute(command);
                    }
                }

                return new ResponseFailure(
                    o[CommandKey].ToString(), "Command not found");
            }
            catch (JsonReaderException)
            {
                return new ResponseFailure(
                    "unknown", "Error reading JSON");
            }
            catch (LinqToDBException)
            {
                return new ResponseFailure(
                    o[CommandKey].ToString(), "Internal server error");
            }
            catch (MySqlException)
            {
                return new ResponseFailure(
                    o[CommandKey].ToString(), "Error connecting to database");
            }
        }
    }
}

