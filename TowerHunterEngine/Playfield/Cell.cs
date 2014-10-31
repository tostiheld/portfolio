using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TowerHunterEngine.Playfield
{
    public class Cell : IDisposable
    {
        private bool[] _borders;

        public Rectangle Bounds { get; private set; }
        public int Value { get; set; }
        public CellType Type { get; private set; }
        public bool[] Borders { get; set; }
        public Texture2D Texture { get; set; }
        public Utils.AnimatedTexture Animation { get; private set; }
        public Vector2 AnimationPosition { get; private set; }
        public Color Fill { get; private set; }

        public Cell(Rectangle bounds)
        {
            this.Bounds = bounds;
            this.ChangeType(CellType.Safe);
            this.Borders = new bool[4] { false, false, false, false };
        }

        public Cell(Rectangle bounds, CellType type, Color fill, Utils.AnimatedTexture anim)
        {
            this.Bounds = bounds;
            this.ChangeType(type, fill, anim);
            this.Borders = new bool[4] { false, false, false, false };

            Point animSize = new Point(0, 0);
            if (anim != null)
                animSize = anim.Size;

            AnimationPosition = new Vector2((float)(Bounds.X + (Bounds.Width / 2) - (animSize.X / 2)),
                                            (float)(Bounds.Y + (Bounds.Height / 2) - (animSize.Y / 2)));

        }

        public void UpdateAnimation(GameTime gameTime)
        {
            if (this.Animation != null)
            {
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                this.Animation.UpdateFrame(elapsed);
            }
        }

        public void DrawAnimation(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin();
            if (this.Animation != null)
            {
                this.Animation.DrawFrame(spriteBatch, AnimationPosition);
            }
            //spriteBatch.End();
        }

        public void ChangeType(CellType type)
        {
            this.Type = type;
            this.Fill = new Color(242, 180, 82);
            this.Animation = null;
        }

        public void ChangeType(CellType type, Color fill, Utils.AnimatedTexture anim)
        {
            this.Type = type;
            this.Fill = fill;
            this.Animation = anim;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // TODO: make a better dispose method. the current
        //       method only disposes the biggest part
        //       (the texture ==> the bitmap)
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Texture != null) Texture.Dispose();
            }
        }
    }
}
