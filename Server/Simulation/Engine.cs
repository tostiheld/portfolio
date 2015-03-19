using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Server.Simulation
{
    public class Engine : Game
    {
        SpriteBatch spriteBatch;

        public Engine()
            : base()
        {
            // initialise graphics
            GraphicsDeviceManager graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            base.Initialize();
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Content.RootDirectory = "";
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Green);

            base.Draw(gameTime);

            spriteBatch.Begin();

            spriteBatch.End();
        }
    }
}
