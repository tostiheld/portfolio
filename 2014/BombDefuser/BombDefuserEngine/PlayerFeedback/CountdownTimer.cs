﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using BombDefuserEngine.Utils.BMFont;

namespace BombDefuserEngine.PlayerFeedback
{
    public class CountdownTimer : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private Game Parent;

        private FontRenderer fontRenderer;
        private FontFile fontFile;
        private Texture2D fontTexture;

        public Point Position { get; set; }

        public bool IsEnabled { get; set; }
        public bool Elapsed { get; private set; }
        public TimeSpan TimeLeft { get; set; }

        public CountdownTimer(Game game, int initialSeconds, Point position) : base(game)
        {
            this.Parent = game;
            this.IsEnabled = false;
            this.TimeLeft = TimeSpan.FromSeconds(initialSeconds);
            
            this.Position = position;
        }

        public void Reset(int initialSeconds)
        {
            this.Elapsed = false;
            this.IsEnabled = false;
            this.TimeLeft = TimeSpan.FromSeconds(initialSeconds);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            string path = System.IO.Path.Combine(Parent.Content.RootDirectory, "Fonts/octin-stencil-100.fnt");
            fontTexture = Parent.Content.Load<Texture2D>("Fonts/octin-stencil-100_0.png");
            fontFile = FontLoader.Load(path);
            fontRenderer = new FontRenderer(fontFile, fontTexture);
        }

        public override void Update(GameTime gameTime)
        {
            if (this.IsEnabled)
            {
                if (this.TimeLeft >= TimeSpan.Zero)
                {
                    this.TimeLeft -= gameTime.ElapsedGameTime;
                }
                else
                {
                    this.Elapsed = true;
                    this.IsEnabled = false;
                }
            }
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            string time = TimeLeft.ToString(@"mm\:ss");

            spriteBatch.Begin();
            fontRenderer.DrawText(spriteBatch, Position.X, Position.Y, time);
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
