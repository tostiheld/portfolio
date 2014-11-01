using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameConsole;

namespace TowerHunterEngine.Utils.ConsoleCommands
{
    public class Reset : IConsoleCommand
    {
        private Game Parent;

        public Reset(Game game)
        {
            Parent = game;
        }

        public string Description
        {
            get { return "Resets the game"; }
        }

        public string Execute(string[] arguments)
        {
            
            return "Game reset.";
        }

        public string Name
        {
            get { return "Reset"; }
        }
    }
}
