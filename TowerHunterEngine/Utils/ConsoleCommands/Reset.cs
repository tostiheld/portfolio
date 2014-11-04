using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameConsole;

namespace BombDefuserEngine.Utils.ConsoleCommands
{
    public class Reset : IConsoleCommand
    {
        private Engine Parent;

        public Reset(Engine parent)
        {
            Parent = parent;
        }

        public string Description
        {
            get { return "Resets the game."; }
        }

        public string Execute(string[] arguments)
        {
            Parent.Reset();

            return "Game reset.";
        }

        public string Name
        {
            get { return "Reset"; }
        }
    }
}
