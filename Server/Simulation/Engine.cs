using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Server.Simulation
{
    public class Engine : Game
    {
        GraphicsDevice graphics;
        SpriteBatch spriteBatch;

        public Engine()
            : base()
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
            GraphicsDeviceManager graphicsd = new GraphicsDeviceManager(this);
            graphicsd.CreateDevice();
            graphics = graphicsd.GraphicsDevice;
            spriteBatch = new SpriteBatch(graphics);

            Content.RootDirectory = "";
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.Clear(Color.Green);

            base.Draw(gameTime);

            spriteBatch.Begin();

            spriteBatch.End();
        }
    }
}
