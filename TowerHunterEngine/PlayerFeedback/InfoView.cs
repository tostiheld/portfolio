using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using TowerHunterEngine.Utils.BMFont;

namespace TowerHunterEngine.PlayerFeedback
{
    public class InfoView : DrawableGameComponent
    {
        public const int WIDTH = 300;
        public const int HEIGHT = 60;
        private const int CHAR_HEIGHT = 48;

        private Game Parent;
        private SpriteBatch spriteBatch;
        private ContentManager Content;

        private Rectangle Bounds;
        private Texture2D Background;

        private HPBar HitPointBar;
        private string Score;
        private Point ScorePosition;
        private Texture2D ScoreFontTexture;

        private FontRenderer fontRenderer;
        private FontFile fontFile;

        private Point _Position;

        public Point Position
        {
            get { return _Position; }
            set
            {
                _Position = value;
                Bounds = new Rectangle(value.X, value.Y, WIDTH, HEIGHT);

                // make score position depend on same bounds
                ScorePosition.X = (int)(Position.X + (Bounds.Width * 0.8));
                ScorePosition.Y = Position.Y + (Bounds.Height / 2) - (CHAR_HEIGHT / 2);
            }
        }

        public Player.Data Data { get; set; }

        public InfoView(Game game, Player.Data data, Point position) : base(game)
        {
            this.Parent = game;
            this.Data = data;
            this.Position = position;
            this.Content = game.Content;

            Rectangle HPBarBounds = new Rectangle(
                (int)(Position.X + (Bounds.Width * 0.1f)),
                (int)(Position.Y  + ((Bounds.Height / 2) - ((Bounds.Height * 0.9f) / 2))),
                (int)(Bounds.Width * 0.7f),
                (int)(Bounds.Height * 0.9f));
            HitPointBar = new HPBar(Parent, HPBarBounds);
        }

        public override void Initialize()
        {
            base.Initialize();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            Background = 
                Utils.RuntimeTextures.ShadowedBackground(
                GraphicsDevice,
                Color.CornflowerBlue, 
                new Point(Bounds.Width, Bounds.Height));
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            string path = System.IO.Path.Combine(Parent.Content.RootDirectory, "Fonts/octin-stencil-60.fnt");
            ScoreFontTexture = Parent.Content.Load<Texture2D>("Fonts/octin-stencil-60_0.png");
            fontFile = FontLoader.Load(path);
            fontRenderer = new FontRenderer(fontFile, ScoreFontTexture);

            HitPointBar.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.Score = Data.Score.ToString();

            float percentage = ((float)Data.HitPoints / (float)Data.MaxHitPoints);
            HitPointBar.Percentage = percentage;
            HitPointBar.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(Background, Bounds, Color.White);
            fontRenderer.DrawText(spriteBatch, ScorePosition.X, ScorePosition.Y, Score);

            spriteBatch.End();

            HitPointBar.Draw(gameTime);

            base.Draw(gameTime);
            
        }
    }
}
