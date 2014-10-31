using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerHunterEngine.Playfield
{
    public class Field : DrawableGameComponent, IDisposable
    {
        SpriteBatch spriteBatch;
        private Point CellSize;
        private Game Game;

        public Point Resolution { get; private set; }
        public Point Size { get; private set; }
        public Cell[,] Cells { get; set; }
        public List<Bomb> Bombs { get; private set; }
        public bool MustUpdate { get; set; }
        public Dictionary<string, Utils.AnimatedTexture> AnimatedTextures { get; set; }

        public Field(Game game, Point resolution, Point cellAmount, int initialBombs) : base(game)
        {
            this.Resolution = resolution;
            this.Size = cellAmount;
            CellSize.X = resolution.X / cellAmount.X;
            CellSize.Y = resolution.Y / cellAmount.Y;
            this.MustUpdate = false;
            this.Bombs = new List<Bomb>();
            this.Game = game;

            // init field
            GenerateRandom();

            for (int i = 0; i < initialBombs; i++)
            {
                AddBomb();
            }
        }

        public void AddBomb()
        {
            Bomb b = new Bomb();
            Point RandomPos = new Point();

            RandomPos.X = new Random(Guid.NewGuid().GetHashCode()).Next(Size.X);
            RandomPos.Y = new Random(Guid.NewGuid().GetHashCode()).Next(Size.Y);
            
            while (Cells[RandomPos.X, RandomPos.Y].Type == CellType.Bomb ||
                   IsNextToBomb(RandomPos))
            {
                RandomPos.X = new Random(Guid.NewGuid().GetHashCode()).Next(Size.X);
                RandomPos.Y = new Random(Guid.NewGuid().GetHashCode()).Next(Size.Y);
            }

            Cells[RandomPos.X, RandomPos.Y].ChangeType(CellType.Bomb, AnimatedTextures["bomb"]);
            this.Bombs.Add(b);

            this.MustUpdate = true;
        }

        private bool IsNextToBomb(Point position)
        {
            if (position.X - 1 > 0 && position.X + 1 < Cells.GetLength(0))
            {
                if (Cells[position.X + 1, position.Y].Type == CellType.Bomb)
                {
                    return true;
                }
                else if (Cells[position.X - 1, position.Y].Type == CellType.Bomb)
                {
                    return true;
                }
            }

            if (position.Y - 1 > 0 && position.Y + 1 < Cells.GetLength(1))
            {
                if (Cells[position.X, position.Y + 1].Type == CellType.Bomb)
                {
                    return true;
                }
                else if (Cells[position.X, position.Y - 1].Type == CellType.Bomb)
                {
                    return true;
                }
            }

            return false;
        }

        public void GenerateRandom()
        {
            this.Cells = new Cell[this.Size.X, this.Size.Y];

            for (int x = 0; x < this.Size.X; x++)
            {
                for (int y = 0; y < this.Size.Y; y++)
                {
                    Rectangle Bounds = new Rectangle(
                        x * CellSize.X, y * CellSize.Y,
                        CellSize.X, CellSize.Y);

                    Cell c = new Cell(Bounds, CellType.Safe, null);
                    Cells.SetValue(c, x, y);
                }
            }

            MazeGenerator.Do(this);
            if (this.Bombs.Count != 0)
                this.Bombs.Clear();
            this.MustUpdate = true;
        }

        public override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (this.MustUpdate)
            {
                for (int x = 0; x < Cells.GetLength(0); x++)
                {
                    for (int y = 0; y < Cells.GetLength(1); y++)
                    {
                        Cell newCell = new Cell(Cells[x, y].Bounds, Cells[x, y].Type, null);
                        newCell.Borders = Cells[x, y].Borders;

                        Cells[x, y] = null;

                        Cells[x, y] = newCell;
                        Cells[x, y].Texture =
                            Utils.RuntimeTextures.BasicBordered(
                                GraphicsDevice,
                                Cells[x, y].Fill,
                                Cells[x, y].Borders);
                    }
                }

                this.MustUpdate = false;
            }

            for (int x = 0; x < Cells.GetLength(0); x++)
            {
                for (int y = 0; y < Cells.GetLength(1); y++)
                {
                    Cells[x, y].UpdateAnimation(gameTime);
                }
            }
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.NonPremultiplied,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullNone); // these are settings to scale up textures without blurring them

            for (int x = 0; x < Cells.GetLength(0); x++)
            {
                for (int y = 0; y < Cells.GetLength(1); y++)
                {
                    spriteBatch.Draw(
                        Cells[x, y].Texture,
                        Cells[x, y].Bounds,
                        Color.White);

                    Cells[x, y].DrawAnimation(spriteBatch);
                }
            }

            spriteBatch.End();
            base.Draw(gameTime);
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
                foreach (Cell c in Cells)
                {
                    if (c != null)
                    {
                        c.Dispose();
                    }
                }
            }
        }
    }
}
