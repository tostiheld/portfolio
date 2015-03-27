using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Server.Simulation
{
    public class Car
    {
        public int Angle { get; set; }
        public float Speed { get; set; }
        public float Slowrate { get; set; }
        public Rectangle Bounds { get; private set; }
        public Rectangle FOV 
        { 
            get
            {
                return new Rectangle(
                    Bounds.X,
                    Bounds.Y - (Settings.CarFOVMargin / 2),
                    Bounds.Width,
                    Bounds.Height + Settings.CarFOVMargin);
            }
        }

        public Car(Size size)
        {
            Bounds = new Rectangle(
                0,
                0,
                size.Width,
                size.Height);
        }

        public void Update(GameTime time, int maxspeed)
        {

        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(
                texture,
                Bounds,
                Color.White);
        }
    }
}

