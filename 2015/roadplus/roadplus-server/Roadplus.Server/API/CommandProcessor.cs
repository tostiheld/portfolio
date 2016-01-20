using System;
using System.Collections.Generic;

namespace Roadplus.Server.API
{
    public abstract class CommandProcessor
    {
        public List<ICommand> RegisteredCommands { get; private set; }

        public CommandProcessor()
        {
            RegisteredCommands = new List<ICommand>();
        }

        public abstract IResponse Process(string command);
    }
}

