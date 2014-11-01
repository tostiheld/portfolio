using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoGameConsole;

namespace TowerHunterEngine.Utils.ConsoleCommands
{
    public class GetLastMessage : IConsoleCommand
    {
        private Robot.Connection Connection;

        public GetLastMessage(Robot.Connection connection)
        {
            Connection = connection;
        }

        public string Description
        {
            get { return "Gets last message sent by the EV3."; }
        }

        public string Execute(string[] arguments)
        {
            if (Connection == null)
            {
                return "Connection with the EV3 is broken.";
            }
            else
            {
                if (Connection.LastMessage != null)
                {
                    return Connection.LastMessage.ToString();
                }
                else
                {
                    return "There is no last message.";
                }
            }
            
        }

        public string Name
        {
            get { return "GetLastmessage"; }
        }
    }
}
