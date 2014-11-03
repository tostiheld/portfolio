using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using System.Timers;

using BombDefuserEngine.Utils.BMFont;

namespace BombDefuserEngine.PlayerFeedback
{
    public class GameOverView : DrawableGameComponent
    {
        private const int ExplosionsAmount = 40;

        private Game Parent;
        private SpriteBatch spriteBatch;
        private Timer ExplosionTimer;
        private List<KeyValuePair<Vector2, Utils.AnimatedTexture>> Explosions;
        private Point MaxPositions;

        private int currentExplosion = 0;
        private int textOpacity = 0;

        private FontRenderer fontRenderer;
        private FontFile fontFile;
        private Texture2D fontTexture;

        public bool Playing { get; private set; }

        public GameOverView(Game game) : base(game)
        {
            this.Parent = game;
            Explosions = new List<KeyValuePair<Vector2, Utils.AnimatedTexture>>();

            ExplosionTimer = new Timer(100);
            ExplosionTimer.Elapsed += new ElapsedEventHandler(ExplosionTimer_Elapsed);
        }

        public void Reset()
        {
            ExplosionTimer.Stop();
            this.Playing = false;
            currentExplosion = 0;
            textOpacity = 0;

            foreach (KeyValuePair<Vector2, Utils.AnimatedTexture> pair in Explosions)
            {
                pair.Value.Reset();
                pair.Value.Pause();
            }
        }

        public void Begin()
        {
            Playing = true;
            ExplosionTimer.Start();
        }

        private void ExplosionTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (currentExplosion < ExplosionsAmount)
            {
                Explosions[currentExplosion].Value.Play();
                currentExplosion++;
                textOpacity += 10;
            }
            else if (textOpacity < 255)
            {
                textOpacity += 10;
            }
            else
            {
                ExplosionTimer.Stop();
            }
        }

        public override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            MaxPositions = new Point(Parent.GraphicsDevice.Viewport.Width - 200,
                                     Parent.GraphicsDevice.Viewport.Height - 200);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            string path = System.IO.Path.Combine(
                Parent.Content.RootDirectory, 
                "Fonts/octin-stencil-gameover.fnt");

            fontTexture = Parent.Content.Load<Texture2D>("Fonts/octin-stencil-gameover_0.png");
            fontFile = FontLoader.Load(path);
            fontRenderer = new FontRenderer(fontFile, fontTexture);

            Random rand = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < ExplosionsAmount; i++)
            {
                Vector2 pos = new Vector2(rand.Next(MaxPositions.X),
                                          rand.Next(MaxPositions.Y));

                Utils.AnimatedTexture anim = 
                    new Utils.AnimatedTexture(
                        Vector2.Zero, 
                        0f, 
                        1f,
                        0f, 
                        new Point(192, 192));

                anim.Load(Parent.Content, "explosion.png", 8, 10);
                anim.Pause();
                KeyValuePair<Vector2, Utils.AnimatedTexture> pair = 
                    new KeyValuePair<Vector2, Utils.AnimatedTexture>(pos, anim);

                Explosions.Add(pair);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (KeyValuePair<Vector2, Utils.AnimatedTexture> pair in Explosions)
            {
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                pair.Value.UpdateFrame(elapsed);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            foreach (KeyValuePair<Vector2, Utils.AnimatedTexture> pair in Explosions)
            {
                pair.Value.DrawFrame(spriteBatch, pair.Key);
            }

            if (this.Playing)
            {
                int text1 = fontRenderer.MeasureString("GAME");
                int text2 = fontRenderer.MeasureString("OVER");
                int posX1 = (Parent.GraphicsDevice.Viewport.Width / 2) - (text1 / 2);
                int posX2 = (Parent.GraphicsDevice.Viewport.Width / 2) - (text2 / 2);

                fontRenderer.DrawText(spriteBatch, posX1, 100, "GAME", textOpacity);
                fontRenderer.DrawText(spriteBatch, posX2, 300, "OVER", textOpacity);
            }

            spriteBatch.End();
        }
    }
}
