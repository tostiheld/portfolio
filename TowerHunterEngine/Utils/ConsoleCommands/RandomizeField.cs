using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameConsole;

namespace TowerHunterEngine.Utils.ConsoleCommands
{
    public class RandomizeField : IConsoleCommand
    {
        private Engine Game;
        private Point Resolution;
        private Point Size;
        private int TowerAmount;

        public RandomizeField(Engine game, Point resolution, Point size, int towerAmount)
        {
            this.Game = game;
            this.Resolution = resolution;
            this.Size = size;
            this.TowerAmount = towerAmount;
        }

        public string Description
        {
            get { return "Generate new random playfield"; }
        }

        public string Execute(string[] arguments)
        {
            Playfield.Field tempfield = new Playfield.Field(Resolution, Size, TowerAmount);
            if (Game.playField != null)
                Game.playField.Dispose();

            Playfield.Generator.Generate(tempfield);

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

            return "Randomized the field";
        }

        public string Name
        {
            get { return "RandomizeField"; }
        }
    }
}
