using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace BombDefuserEngine.Utils
{
    public class AvailableColor
    {
        public Color Value { get; set; }
        public bool Available { get; set; }

        public AvailableColor(Color color, bool available)
        {
            this.Value = color;
            this.Available = available;
        }
    }
}
