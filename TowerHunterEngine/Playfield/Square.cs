/*
 *
 * File created by: Thomas Schraven
 * Creation date: 26-09-2014
 * 
 * Last modified by: Thomas Schraven
 * Last modification date: 26-09-2014
 * 
 */

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace TowerHunterEngine.Playfield
{
    public class Square : IDisposable
    {
        public Square(Point position, Point size, SquareType type)
        {
            this.Value = 0;
            this.Type = type;

            this.Bounds = new Rectangle(position.X * size.X, position.Y * size.Y, size.X, size.Y);

            this.Borders = new bool[4] {false, false, false, false};
        }

        public int Value { get; set; }
        public Rectangle Bounds { get; private set; }
        public SquareType Type { get; private set; }
        public bool[] Borders { get; set; }
        public Texture2D Texture { get; set; }


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
