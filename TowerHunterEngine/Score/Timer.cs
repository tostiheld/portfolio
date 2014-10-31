using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TowerHunterEngine.Score
{
    public class Timer : DrawableGameComponent
    {
        private Vector2 Position;
        private Vector2 PositionUp;
        private Vector2 PositionDown;
        private Vector2 PositionLeft;
        private Vector2 PositionRight;
        private SpriteBatch spriteBatch;
        ContentManager content;
        SpriteFont spriteFont;

        public bool IsEnabled { get; set; }
        public bool Elapsed { get; private set; }
        public TimeSpan TimeLeft { get; set; }

        public Timer(Game game, int initialSeconds, Vector2 position) : base(game)
        {
            this.IsEnabled = false;
            this.TimeLeft = TimeSpan.FromSeconds(initialSeconds);
            
            this.Position = position;
            this.PositionUp = new Vector2(Position.X, Position.Y + 2);
            this.PositionDown = new Vector2(Position.X, Position.Y - 2);
            this.PositionLeft = new Vector2(Position.X - 2, Position.Y);
            this.PositionRight = new Vector2(Position.X + 2, Position.Y);

            content = new ContentManager(game.Services);
            content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = content.Load<SpriteFont>("timerFont");
        }

        protected override void UnloadContent()
        {
            content.Unload();
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
            spriteBatch.DrawString(spriteFont, time, this.PositionUp, Color.Orange);
            spriteBatch.DrawString(spriteFont, time, this.PositionDown, Color.Orange);
            spriteBatch.DrawString(spriteFont, time, this.PositionLeft, Color.Orange);
            spriteBatch.DrawString(spriteFont, time, this.PositionRight, Color.Orange);
            spriteBatch.DrawString(spriteFont, time, this.Position, Color.DarkRed);
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
