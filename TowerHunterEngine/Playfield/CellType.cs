using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace TowerHunterEngine.Playfield
{
    public class CellType
    {
        public Color Fill { get; private set; }
        public Point Position { get; private set; }
        public CellVariants Variant { get; set; }

        public CellType(string color, Point position, CellVariants variant)
        {
            Color c = Color.White;

            switch (color)
            {
                case "red":
                    c = Color.Red;
                    break;
                case "green":
                    c = Color.Green;
                    break;
                case "blue":
                    c = Color.Blue;
                    break;
                case "cyan":
                    c = Color.Cyan;
                    break;
                case "yellow":
                    c = Color.Yellow;
                    break;
                case "magenta":
                    c = Color.Magenta;
                    break;
            }

            this.Fill = c;
            this.Position = position;
            this.Variant = variant;
        }
    }
}
