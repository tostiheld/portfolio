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
        /*
        public bool[] Borders
        {
            get
            {
                return this._borders;
            }
            set
            {
                this._borders = value;
                if (_borders[1]) // east border
                {
                    // 20% naar links
                    float y = this.AnimationPosition.Y;
                    float x = this.AnimationPosition.X;
                    this.AnimationPosition = new Vector2(x - (float)(Bounds.Width * 0.1), y);
                    
                }
                else if (_borders[2]) // south border
                {
                    // 20% naar boven
                    float y = this.AnimationPosition.Y;
                    float x = this.AnimationPosition.X;
                    this.AnimationPosition = new Vector2(x, y - (float)(Bounds.Height * 0.1));
                }
            }
        }*/
        public Texture2D Texture { get; set; }
        public Utils.AnimatedTexture Animation { get; private set; }
        public Vector2 AnimationPosition { get; private set; }
        public Color Fill { get; private set; }


        public Cell(Rectangle bounds, CellType type, Utils.AnimatedTexture anim)
        {
            this.Bounds = bounds;
            this.ChangeType(type, anim);
            this._borders = new bool[4] { false, false, false, false };
            this.Borders = _borders;

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

        public void ChangeType(CellType type, Utils.AnimatedTexture anim)
        {
            this.Type = type;

            switch (type)
            {
                case CellType.Bomb:
                    this.Animation = anim;
                    this.Fill = Color.Red;
                    break;
                case CellType.Coin:
                    this.Animation = null;
                    this.Fill = Color.Yellow;
                    break;
                case CellType.Goal:
                    this.Animation = null;
                    this.Fill = Color.Blue;
                    break;
                case CellType.Powerup:
                    this.Animation = null;
                    this.Fill = Color.Orange;
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
