using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BombDefuserEngine.PlayerFeedback
{
    public class Tutorial : DrawableGameComponent
    {
        private Game Parent;
        private SpriteBatch spriteBatch;
        public bool IsShown = false;
        private int currentScreen = 0;
        private GamePadState previousstate;

        private CountdownTimer targetTimer;

        private Texture2D Screen1;
        private Texture2D Screen2;

        public Tutorial(Game game) : base(game)
        {
            Parent = game;
        }

        public void Show(CountdownTimer timer)
        {
            this.IsShown = true;
            targetTimer = timer;
        }

        public void Reset()
        {
            IsShown = false;
            currentScreen = 0;
            targetTimer.IsEnabled = true;
        }

        public override void Initialize()
        {
            base.Initialize();

            spriteBatch = new SpriteBatch(Parent.GraphicsDevice);

            previousstate = GamePad.GetState(PlayerIndex.One);
        }

        protected override void LoadContent()
        {
            Screen1 = Parent.Content.Load<Texture2D>("tutScreen1.png");
            Screen2 = Parent.Content.Load<Texture2D>("tutScreen2.png");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            GamePadState state = GamePad.GetState(PlayerIndex.One);
            if (IsShown)
            {
                if (state.Buttons.A == ButtonState.Pressed &&
                    previousstate.Buttons.A == ButtonState.Released)
                {
                    currentScreen++;
                }
            }
            if (currentScreen >= 2)
            {
                this.Reset();
            }
            previousstate = state;
        }

        public override void Draw(GameTime gameTime)
        {
            if (IsShown)
            {
                spriteBatch.Begin();

                if (currentScreen == 0)
                {
                    spriteBatch.Draw(Screen1, new Rectangle(
                        0,
                        0,
                        Parent.GraphicsDevice.Viewport.Width,
                        Parent.GraphicsDevice.Viewport.Height),
                        Color.White);
                }
                else if (currentScreen == 1)
                {
                    spriteBatch.Draw(Screen2, new Rectangle(
                        0,
                        0,
                        Parent.GraphicsDevice.Viewport.Width,
                        Parent.GraphicsDevice.Viewport.Height),
                        Color.White);
                }

                spriteBatch.End();
            }
        }
    }
}
