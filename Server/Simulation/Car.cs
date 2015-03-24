using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Server.Simulation
{
    public class Car : IDisposable
    {
        public float Speed { get; set; }
        public Rectangle Bounds { get; private set; }
        public int Angle { get; set; }

        private Texture2D texture;

        public Car(ContentManager content, string assetname)
        {
            texture = content.Load<Texture2D>(assetname);
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (texture != null)
            {
                spriteBatch.Draw(
                    texture,
                    Bounds,
                    Color.White);
            }
        }

        public void Dispose()
        {
            texture.Dispose();
        }
    }
}

