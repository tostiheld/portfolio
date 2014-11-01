using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerHunterEngine.PlayerFeedback
{
    public class InfoView : DrawableGameComponent
    {
        private const int WIDTH = 250;
        private const int HEIGHT = 30;
        private const int CHAR_POINTS = 36;
        private const int CHAR_HEIGHT = 48;

        private Game Parent;
        private Player.Data Data;

        private Rectangle Bounds;
        private Rectangle HPBarFill;
        private string Score;
        private Point ScorePosition;

        public Point Position
        {
            get;
            set
            {
                Position = value;
                Bounds = new Rectangle(value.X, value.Y, WIDTH, HEIGHT);

                // make hpbar depend on own bounds
                int barWidth = (int)(Bounds.Width * 0.7);
                int barHeight = (int)(Bounds.Height * 0.3);
                int barX = (int)(Position.X + (Bounds.Width * 0.04));
                int barY = (Bounds.Height / 2) - (barHeight / 2);
                HPBar = new Rectangle(barX, barY, barWidth, barHeight);

                // make score position depend on same bounds
                ScorePosition.X = (int)(Position.X + (Bounds.Width * 0.8));
                ScorePosition.Y = 
            }
        }

        private Rectangle HPBar
        {
            get;
            set
            {
                HPBar = value;
                HPBarFill.Location = HPBar.Location;
                HPBarFill.Height = HPBar.Height;
                HPBarFill.Width = (Data.HitPoints / Data.MaxHitPoints) * HPBar.Width;
            }
        }

        public InfoView(Game game, Player.Data data, Point position) : base(game)
        {
            this.Parent = game;
            this.Data = data;
            this.Position = position;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.Score = Data.Score.ToString();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
