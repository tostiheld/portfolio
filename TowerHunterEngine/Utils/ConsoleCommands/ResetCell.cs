using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoGameConsole;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerHunterEngine.Utils.ConsoleCommands
{
    class ResetCell : IConsoleCommand
    {
        private Playfield.Field Field;

        public ResetCell(Playfield.Field field)
        {
            this.Field = field;
        }
        public string Description
        {
            get { return "Reset cell by color"; }
        }

        public string Execute(string[] arguments)
        {
            string arg = arguments[0].ToLower();

            Color color = Color.White;

            switch (arg)
            {
                case "red":
                    color = new Color(255, 0, 0);
                    break;
                case "green":
                    color = new Color(0, 255, 0);
                    break;
                case "blue":
                    color = new Color(0, 0, 255);
                    break;
                case "cyan":
                    color = new Color(0, 255, 255);
                    break;
                case "yellow":
                    color = new Color(255, 255, 0);
                    break;
                case "magenta":
                    color = new Color(255, 0, 255);
                    break;
                case "orange":
                    color = new Color(255, 127, 0);
                    break;
                default:
                    break;
            }

            Field.ResetCell(color);
            Field.MustUpdate = true;

            return "Bomb removed";
        }

        public string Name
        {
            get { return "ResetCell"; }
        }
    }
}