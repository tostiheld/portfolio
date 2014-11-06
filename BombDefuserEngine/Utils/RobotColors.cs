using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace BombDefuserEngine.Utils
{
    public struct RobotColors
    {
        public static Color Red
        {
            get
            {
                return new Color(255, 0, 0);
            }
        }

        public static Color Green
        {
            get
            {
                return new Color(0, 255, 0);
            }
        }

        public static Color Blue
        {
            get
            {
                return new Color(0, 0, 255);
            }
        }

        public static Color Cyan
        {
            get
            {
                return new Color(0, 255, 255);
            }
        }

        public static Color Yellow
        {
            get
            {
                return new Color(255, 255, 0);
            }
        }

        public static Color Magenta
        {
            get
            {
                return new Color(255, 0, 180);
            }
        }

        public static Color Orange
        {
            get
            {
                return new Color(255, 127, 0);
            }
        }


    }
}
