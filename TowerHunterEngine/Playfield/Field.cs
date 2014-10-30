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

        public Point Resolution { get; private set; }
        public Point Size { get; private set; }
        public Cell[,] Cells { get; set; }
        public bool MustUpdate { get; set; }

        public Field(Game game, Point resolution, Point cellAmount) : base(game)
        {
            this.Resolution = resolution;
            this.Size = cellAmount;
            CellSize.X = resolution.X / cellAmount.X;
            CellSize.Y = resolution.Y / cellAmount.Y;
            this.MustUpdate = false;

            // init field
            GenerateRandom();            
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

                    Cell c = new Cell(Bounds, CellType.Safe);
                    Cells.SetValue(c, x, y);
                }
            }

            MazeGenerator.Do(this);
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
                        Cell newCell = new Cell(Cells[x, y].Bounds, Cells[x, y].Type);
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
