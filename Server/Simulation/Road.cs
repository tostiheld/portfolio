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

        private Vector2 origin;

        public Road(Point start, Point end, int width, int speedlimit = 30)
        {
            Start = start;
            End = end;

            origin = new Vector2(
                Convert.ToSingle(Start.X),
                Convert.ToSingle(Start.Y));

            int x = Start.X - (width / 2);
            int y = Start.Y;

            int height = Convert.ToInt32(
                Math.Sqrt(
                Math.Pow(End.X - Start.X, 2.0) +
                Math.Pow(End.Y - Start.Y, 2.0)));

            Surface = new Rectangle(
                x,
                y,
                height,
                width);

            int dX = End.X - Start.X;
            int dY = End.Y - Start.Y;

            if (dX == 0)
            {
                if (dY > 0)
                {
                    Angle = DegreesToRadians(90.0);
                }
                else
                {
                    Angle = DegreesToRadians(270.0);
                }
            }
            else
            {
                Angle = Convert.ToSingle(
                    Math.Atan2(
                    Convert.ToDouble(dY),
                    Convert.ToDouble(dX)));
            }

            SpeedLimit = speedlimit;
        }

        private float DegreesToRadians(double degrees)
        {
            return Convert.ToSingle(Math.PI * degrees / 180.0);
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
                    Vector2.Zero,         // could be origin?
                    SpriteEffects.None,
                    0f);
            }
        }
    }
}

