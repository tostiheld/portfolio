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
            this.ChangeType(type);

            this.Bounds = new Rectangle(position.X * size.X, position.Y * size.Y, size.X, size.Y);

            this.Borders = new bool[4] {false, false, false, false};
        }

        public int Value { get; set; }
        public Rectangle Bounds { get; private set; }
        public SquareType Type { get; private set; }
        public bool[] Borders { get; set; }
        public Texture2D Texture { get; set; }
        public Color Fill { get; private set; }

        public void ChangeType(SquareType type)
        {
            this.Type = type;

            switch (type)
            {
                case SquareType.Safe:
                    //this.Fill = new Color(0, 255, 0, 200);
                    this.Fill = Color.Green;
                    break;
                case SquareType.Bomb:
                    this.Fill = Color.Red;
                    break;
                case SquareType.Powerup:
                    this.Fill = Color.Orange;
                    break;
                case SquareType.Coin:
                    this.Fill = Color.Yellow;
                    break;
                case SquareType.Goal:
                    this.Fill = new Color(0, 0, 255);
                    //this.Fill = Color.Blue;
                    break;
                case SquareType.Test:
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
