using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoGameConsole;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerHunterEngine.Utils.ConsoleCommands
{
    class ChangeCellType : IConsoleCommand
    {
        private Playfield.Field Field;

        public ChangeCellType(Playfield.Field field)
        {
            this.Field = field;
        }
        public string Description
        {
            get { return "Change the type of a cell at (x,y)"; }
        }

        public string Execute(string[] arguments)
        {
            int ax = Convert.ToInt32(arguments[0]);
            int ay = Convert.ToInt32(arguments[1]);

            Playfield.CellType type = Playfield.CellType.Safe;

            switch (arguments[2])
            {
                case "Safe":
                    type = Playfield.CellType.Safe;
                    break;
                case "Forbidden":
                    type = Playfield.CellType.Bomb;
                    break;
                case "Powerup":
                    type = Playfield.CellType.Powerup;
                    break;
                case "Coin":
                    type = Playfield.CellType.Coin;
                    break;
                case "Goal":
                    type = Playfield.CellType.Goal;
                    break;
                case "Test":
                    type = Playfield.CellType.Test;
                    break;
                default:
                    return "Unknown type";

            }

            Field.Cells[ax, ay].ChangeType(type, Field.AnimatedTextures["bomb"]);
            Field.MustUpdate = true;

            return "Changed a Square";
        }

        public string Name
        {
            get { return "ChangeCellType"; }
        }
    }
}