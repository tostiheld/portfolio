using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Server.Simulation
{
    public class Engine : Game
    {
        public SpriteBatch spriteBatch { get; private set; }

        private TrafficManager Traffic;
        private MouseState previousState;

        public Engine(Zone zone)
            : base()
        {
            // initialise graphics
            new GraphicsDeviceManager(this);

            IsMouseVisible = true;

            // set up traffic system
            Road startRoad = new Road(
                new Point(100, 40),
                new Point(100, 100),
                Settings.RoadWidth);
            Traffic = new TrafficManager(this, startRoad);
            Components.Add(Traffic);

            Content.RootDirectory = "Resources";
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            // why does this have to happen here?
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            MouseState mouse = Mouse.GetState();

            //System.Diagnostics.Debug.WriteLine(
            //    mouse.Position.ToString());

            if (previousState.LeftButton == ButtonState.Pressed &&
                mouse.LeftButton == ButtonState.Released &&
                GraphicsDevice.Viewport.Bounds.Contains(mouse.Position))
            {
                Traffic.AddRoad(mouse.Position);
            }

            previousState = mouse;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            base.Draw(gameTime);
        }
    }
}
