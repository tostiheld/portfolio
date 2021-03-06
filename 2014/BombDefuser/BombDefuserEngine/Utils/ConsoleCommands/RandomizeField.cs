﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameConsole;

namespace BombDefuserEngine.Utils.ConsoleCommands
{
    public class RandomizeField : IConsoleCommand
    {
        private Playfield.Field Field;
        private int Bombs;

        public RandomizeField(Playfield.Field field)
        {
            this.Field = field;
            this.Bombs = field.BombAmount;
        }

        public string Description
        {
            get { return "Generate new random playfield"; }
        }

        public string Execute(string[] arguments)
        {
            foreach (Utils.AvailableColor ac in Field.AvailableColors.Values)
            {
                ac.Available = true;
            }

            Field.GenerateRandom();
            for (int i = 0; i < Bombs; i++)
            {
                Field.AddSpecialCell(Playfield.CellType.Bomb);
            }

            return "Randomized the field";
        }

        public string Name
        {
            get { return "RandomizeField"; }
        }
    }
}