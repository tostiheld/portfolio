using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Server.Simulation
{
    public class Road
    {
        public Point Start { get; private set; }
        public Point End { get; private set; }
        public Rectangle Surface { get; private set; }
        public int SpeedLimit { get; set; }
        public float Angle { get; private set; }

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

            /*
             * We don't use this code because this gives a 
             * really odd bug in Mono.
             * This should be the correct code though, 
             * because this works in .NET on Windows.
             * 
            double dX = Convert.ToDouble(end.X - start.X);
            double dY = Convert.ToDouble(end.Y - start.Y);

            if (Math.Abs(dX) <= Double.Epsilon)
            {
                if (dY > 0.0)
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
            }*/

            // Instead, we use this code:
            int dX = End.X - Start.X;
            int dY = End.Y - Start.Y;

            if (dX == 0)
            {
                if (dY > 0)
                {
                    Angle = 90f;
                }
                else
                {
                    Angle = 270f;
                }
            }
            else
            {
                Angle = Convert.ToSingle(
                    Math.Atan2(
                    Convert.ToDouble(dY),
                    Convert.ToDouble(dX))
                    * 180 / Math.PI);
            }

            SpeedLimit = speedlimit;
        }

        public void Draw(SpriteBatch batch, Texture2D texture)
        {
            if (texture != null)
            {
                batch.Draw(
                    texture,
                    Surface,
                    null,
                    Color.White,
                    Angle,
                    Vector2.Zero,
                    SpriteEffects.None,
                    0f);
            }
        }
    }
}

