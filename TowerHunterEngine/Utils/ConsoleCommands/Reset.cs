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
            get { return "Resets the game. Optional arguments: initial time, max hit points and initial amount of bombs"; }
        }

        public string Execute(string[] arguments)
        {
            int time = 0;
            int hp = 0;
            int bombs = 0;
            if (arguments.Length > 0)
            {
                if (Int32.TryParse(arguments[0], out time) &&
                    Int32.TryParse(arguments[1], out hp) &&
                    Int32.TryParse(arguments[2], out bombs))
                {
                    Parent.Reset(bombs, time, hp);
                }
            }
            else
            {
                Parent.Reset();
            }

            return "Game reset.";
        }

        public string Name
        {
            get { return "Reset"; }
        }
    }
}
