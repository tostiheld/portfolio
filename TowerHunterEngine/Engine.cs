/*
 *
 * File created by: Thomas Schraven
 * Creation date: 26-09-2014
 * 
 * Last modified by: Thomas Schraven
 * Last modification date: 26-09-2014
 * 
 */

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace TowerHunterEngine
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Engine : Game
    {
        private readonly Point GAMERES = new Point(1024, 600);
        private readonly Point FIELDSIZE = new Point(10, 5);
        private const int TOWERS = 4;
        private const bool FULLSCREEN = true;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        KeyboardState oldState;

        private Playfield.Field playField;

        float frameRate;
        SpriteFont font;

        public Engine()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = FULLSCREEN;
            graphics.PreferredBackBufferHeight = GAMERES.Y;
            graphics.PreferredBackBufferWidth = GAMERES.X;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            SetupPlayfield(GAMERES, FIELDSIZE, TOWERS, ref playField);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //font = Content.Load<SpriteFont>("default")
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
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

            frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
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

            //spriteBatch.DrawString(font, frameRate.ToString(), new Point(10, 10), Color.Red);

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
