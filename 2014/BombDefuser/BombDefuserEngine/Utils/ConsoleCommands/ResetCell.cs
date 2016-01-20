using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoGameConsole;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BombDefuserEngine.Utils.ConsoleCommands
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
                    color = RobotColors.Red;
                    break;
                case "green":
                    color = RobotColors.Green;
                    break;
                case "blue":
                    color = RobotColors.Blue;
                    break;
                case "cyan":
                    color = RobotColors.Cyan;
                    break;
                case "yellow":
                    color = RobotColors.Yellow;
                    break;
                case "magenta":
                    color = RobotColors.Magenta;
                    break;
                case "orange":
                    color = RobotColors.Orange;
                    break;
                default:
                    return "Unknown color";
            }

            Field.ResetCell(color);

            return "Cell reset";
        }

        public string Name
        {
            get { return "ResetCell"; }
        }
    }
}