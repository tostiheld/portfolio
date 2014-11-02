using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace BombDefuserEngine.PlayerFeedback
{
    public class HPBar
    {
        private Game Parent;
        private SpriteBatch spriteBatch;

        private Rectangle Bounds;
        private Texture2D Background;
        private Rectangle FillBounds;
        private Texture2D Fill;

        private int MaxFillWidth;

        private float percentage;
        public float Percentage
        {
            get { return percentage; }
            set
            {
                if (value < 0)
                {
                    throw new NotSupportedException("Percentage value must be larger than 0.");
                }
                else if (value > 1)
                {
                    throw new NotSupportedException("Percentage value must be smaller than 1.");
                }
                else
                {
                    percentage = value;
                }
            }
        }

        public HPBar(Game game, Rectangle bounds)
        {
            Parent = game;
            percentage = 0;
            Bounds = bounds;

            MaxFillWidth = (int)(Bounds.Width * 0.94f);

            Vector2 FillPosition = new Vector2(
                Bounds.Location.X + (Bounds.Width * 0.04f),
                Bounds.Location.Y);

            FillBounds = new Rectangle(
                (int)FillPosition.X,
                (int)FillPosition.Y,
                MaxFillWidth,
                Bounds.Height);
        }

        public void LoadContent()
        {

            spriteBatch = new SpriteBatch(Parent.GraphicsDevice);

            Background = Parent.Content.Load<Texture2D>("HPBar.png");
            Fill = Parent.Content.Load<Texture2D>("HPBarFill.png");
        }

        public void Update(GameTime gameTime)
        {
            FillBounds.Width = (int)(MaxFillWidth * percentage);
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullNone); // these are settings to scale up textures without blurring them

            spriteBatch.Draw(Background, Bounds, Color.White);
            spriteBatch.Draw(Fill, FillBounds, Color.White);

            spriteBatch.End();
        }
    }
}
