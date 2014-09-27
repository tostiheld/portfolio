﻿#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Reflection;
#endregion

namespace TowerHunterEngine
{
    public class Engine : Game
    {
        private readonly Point GAMERES = new Point(1024, 600);
        /*
         * Field sizes affect framerates
         * 40, 20 = 10-16 fps
         * 30, 15 = 30 fps max
         * 25, 12 = 40 fps max
         * 20, 10 = 60 fps max
         * (tested on THOMAS-LAPTOP)
         * 
         */
        private readonly Point FIELDSIZE = new Point(40, 20);
        private const int TOWERS = 4;
        private const bool FULLSCREEN = false;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        KeyboardState oldState;

        private Playfield.Field playField;

        float frameRate;
        private List<string> DebugLine;
        SpriteFont font;

        public Engine()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = FULLSCREEN;
            graphics.PreferredBackBufferHeight = GAMERES.Y;
            graphics.PreferredBackBufferWidth = GAMERES.X;
            graphics.ApplyChanges();

#if DEBUG
            Components.Add(new Utils.FrameCounter(this));
#endif

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            SetupPlayfield(GAMERES, FIELDSIZE, TOWERS, ref playField);

#if DEBUG
            DebugLine = new List<string>(2);
            DebugLine.Add("DEBUGGING");
            DebugLine.Add(Assembly.GetExecutingAssembly().GetName().Version.ToString());
#endif

            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("font");
        }
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState newState = Keyboard.GetState();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (newState.IsKeyDown(Keys.R))
            {
                if (!oldState.IsKeyDown(Keys.R))
                    SetupPlayfield(GAMERES, FIELDSIZE, TOWERS, ref playField);
            }

            oldState = newState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.NonPremultiplied,
                SamplerState.PointClamp,
                DepthStencilState.Default,
                RasterizerState.CullNone);

            for (int x = 0; x < playField.Grid.GetLength(0); x++)
            {
                for (int y = 0; y < playField.Grid.GetLength(1); y++)
                {
                    spriteBatch.Draw(
                        playField.Grid[x, y].Texture,
                        playField.Grid[x, y].Bounds,
                        Color.White);
                }
            }

#if DEBUG
            spriteBatch.DrawString(font, DebugLine[0] + " v" + DebugLine[1], new Vector2(10, 10), Color.Red);
#endif
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void SetupPlayfield(Point resolution, Point size, int towerAmount, ref Playfield.Field field)
        {
            // dispose of the old field because we would otherwise
            // get a memory leak
            if (field != null)
                field.Dispose();

            field = new Playfield.Field(resolution, size, towerAmount);

            Playfield.Generator.Generate(field);

            for (int x = 0; x < field.Grid.GetLength(0); x++)
            {
                for (int y = 0; y < field.Grid.GetLength(1); y++)
                {
                    field.Grid[x, y].Texture =
                        Utils.RuntimeTextures.BasicBordered(
                            GraphicsDevice,
                            Color.Green,
                            field.Grid[x, y].Borders);
                }
            }
        }
    }
}
