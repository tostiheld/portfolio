using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoGameConsole;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BombDefuserEngine.Utils.ConsoleCommands
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
            AnimatedTexture anim = null;

            string sType = arguments[2].ToLower();
            Color color = Color.White;

            if (sType == "safe")
            {
                type = Playfield.CellType.Safe;
                color = new Color(242, 180, 82);
            }
            else
            {
                switch (sType)
                {
                    case "bomb":
                        anim = Field.AnimatedTextures["bomb"];
                        type = Playfield.CellType.Bomb;
                        break;
                    case "powerup":
                        type = Playfield.CellType.Powerup;
                        break;
                    case "coin":
                        anim = Field.AnimatedTextures["coin"];
                        type = Playfield.CellType.Coin;
                        break;
                    case "goal":
                        type = Playfield.CellType.Goal;
                        break;
                    case "test":
                        type = Playfield.CellType.Test;
                        break;
                    default:
                    return "Unknown type";
                }
                color = Field.GetFirstAvailableColor();
            }

            Field.Cells[ax, ay].ChangeType(type, color, anim);
            Field.MustUpdate = true;

            return "Changed a Cell";
        }

        public string Name
        {
            get { return "ChangeCellType"; }
        }
    }
}