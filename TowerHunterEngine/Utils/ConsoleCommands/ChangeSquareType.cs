using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoGameConsole;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerHunterEngine.Utils.ConsoleCommands
{
    class ChangeSquareType : IConsoleCommand
    {
        private Engine Game;
        private Point Resolution;
        private Point Size;
        private int TowerAmount;

        public ChangeSquareType(Engine game, Point resolution, Point size, int towerAmount)
        {
            this.Game = game;
            this.Resolution = resolution;
            this.Size = size;
            this.TowerAmount = towerAmount;
        }
        public string Description
        {
            get { return "Change the type of a square at (x,y)"; }
        }

        public string Execute(string[] arguments)
        {
            Playfield.Field tempfield = new Playfield.Field(Resolution, Size, TowerAmount);
            if (Game.playField != null)
                Game.playField.Dispose();

            Playfield.Generator.Generate(tempfield);

            int ax = Convert.ToInt32(arguments[0]);
            int ay = Convert.ToInt32(arguments[1]);

            Playfield.SquareType type = Playfield.SquareType.Safe;

            switch (arguments[2])
            {
                case "Safe":
                    type = Playfield.SquareType.Safe;
                    break;
                case "Forbidden":
                    type = Playfield.SquareType.Forbidden;
                    break;
                case "Powerup":
                    type = Playfield.SquareType.Powerup;
                    break;
                case "Coin":
                    type = Playfield.SquareType.Coin;
                    break;
                case "Goal":
                    type = Playfield.SquareType.Goal;
                    break;
                case "Test":
                    type = Playfield.SquareType.Test;
                    break;
                default:
                    return "Unknown type";

            }

            tempfield.Grid[ax, ay].ChangeType(type);

            for (int x = 0; x < tempfield.Grid.GetLength(0); x++)
            {
                for (int y = 0; y < tempfield.Grid.GetLength(1); y++)
                {
                    tempfield.Grid[x, y].Texture =
                        Utils.RuntimeTextures.BasicBordered(
                            Game.GraphicsDevice,
                            tempfield.Grid[x, y].Fill,
                            tempfield.Grid[x, y].Borders);
                }
            }

            Game.playField = tempfield;

            return "Changed a Square";
        }

        public string Name
        {
            get { return "SquareType"; }
        }
    }
}
