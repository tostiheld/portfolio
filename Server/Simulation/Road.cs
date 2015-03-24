using System;
using Microsoft.Xna.Framework;

namespace Server.Simulation
{
    public class Road
    {
        public Point Start { get; private set; }
        public Point End { get; private set; }
        public Rectangle Surface { get; private set; }
        public int SpeedLimit { get; set; }
        public double Angle { get; private set; }

        public Road(Point start, Point end, int width, int speedlimit = 30)
        {
            Start = start;
            End = end;

            int x = end.X - (width / 2);
            int y = end.Y;
            int height = end.Y - start.Y;
            Surface = new Rectangle(
                x,
                y,
                width,
                height);

            double dX = Convert.ToDouble(end.X - start.X);
            double dY = Convert.ToDouble(end.Y - start.Y);

            //if (Math.Abs(dX) <= Double.Epsilon)
            if (IsApproximatelyEqualTo(dX, 0.0, Double.Epsilon))
            {
                if (dY > 0)
                {
                    Angle = 90.0;
                }
                else
                {
                    Angle = 270.0;
                }
            }
            else
            {
                Angle = Math.Atan2(dY, dX) * 180 / Math.PI;
            }

            SpeedLimit = speedlimit;
        }

        private bool IsApproximatelyEqualTo(double initialValue, double value, double maximumDifferenceAllowed)
        {
            // Handle comparisons of floating point values that may not be exactly the same
            return (Math.Abs(initialValue - value) < maximumDifferenceAllowed);
        }
    }
}

