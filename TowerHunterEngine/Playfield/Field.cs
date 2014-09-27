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
using System.Collections.Generic;
using System.Collections;

namespace TowerHunterEngine.Playfield
{
    public class Field : IDisposable
    {
        private int towerCount;

        public Point squareSize;

        public Field(Point resolution, Point numSquares, int numTowers)
        {
            this.Resolution = resolution;
            towerCount = numTowers;

            squareSize.X = resolution.X / numSquares.X;
            squareSize.Y = resolution.Y / numSquares.Y;

            this.Size = numSquares;

            this.Generated = false;
        }

        public Point Resolution
        {
            get;
            private set;
        }

        public Point Size
        {
            get;
            private set;
        }

        public Square[,] Grid
        {
            get;
            set;
        }

        public List<Tower> Towers
        {
            get;
            private set;
        }

        public bool Generated
        {
            get;
            set;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (Square s in Grid)
                {
                    if (s != null)
                    {
                        s.Dispose();
                    }
                }
            }
        }
    }
}
