using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameConsole;

namespace TowerHunterEngine.Utils.ConsoleCommands
{
    public class SetPosition : IConsoleCommand
    {
        public SetPosition()
        {

        }

        public string Description
        {
            get { return "Restarts the game at a new position"; }
        }

        public string Execute(string[] arguments)
        {
            
            return "";
        }

        public string Name
        {
            get { return "RunFullTest"; }
        }
    }
}
