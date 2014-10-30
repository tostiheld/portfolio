using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerHunterEngine.Playfield
{
    public class Cell : IDisposable
    {
        public Rectangle Bounds { get; private set; }
        public int Value { get; set; }
        public CellType Type { get; private set; }
        public bool[] Borders { get; set; }
        public Texture2D Texture { get; set; }
        public Utils.AnimatedTexture Animation { get; private set; }
        public Color Fill { get; private set; }


        public Cell(Rectangle bounds, CellType type)
        {
            this.Bounds = bounds;
            this.ChangeType(type);
            this.Borders = new bool[4] { false, false, false, false };
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
                this.Animation.DrawFrame(spriteBatch,
                    new Vector2((float)this.Bounds.X,
                                (float)this.Bounds.Y));
            }
            //spriteBatch.End();
        }

        public void ChangeType(CellType type)
        {
            this.Type = type;

            switch (type)
            {
                case CellType.Bomb:
                    // TODO: add animation and fill
                    break;
                case CellType.Coin:
                    // TODO: add animation and fill
                    break;
                case CellType.Goal:
                    // TODO: add animation and fill
                    break;
                case CellType.Powerup:
                    // TODO: add animation and fill
                    break;
                case CellType.Safe:
                    this.Animation = null;
                    this.Fill = Color.Green;
                    break;
                case CellType.Test:
                    this.Animation = null;
                    this.Fill = Color.White;
                    break;
            }
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
