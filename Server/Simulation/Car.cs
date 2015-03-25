using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Server.Simulation
{
    public class Car : IDisposable
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

        private Texture2D carTexture;

        public Car(Texture2D texture, Size size)
        {
            carTexture = texture;
            Bounds = new Rectangle(
                0,
                0,
                size.Width,
                size.Height);
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (carTexture != null)
            {
                spriteBatch.Draw(
                    carTexture,
                    Bounds,
                    Color.White);
            }
        }

        public void Dispose()
        {
            carTexture.Dispose();
        }
    }
}

