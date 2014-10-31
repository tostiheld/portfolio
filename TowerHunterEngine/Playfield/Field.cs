using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TowerHunterEngine.Playfield
{
    public class Field : DrawableGameComponent, IDisposable
    {
        public Dictionary<string, bool> availableColors = new Dictionary<string, bool>
        {
            {"red", true},
            {"green", true},
            {"blue", true},
            {"cyan", true},
            {"yellow", true},
            {"magenta", true}
        };

        SpriteBatch spriteBatch;
        private Point CellSize;
        private Game Engine;

        public Point Resolution { get; private set; }
        public Point Size { get; private set; }
        public Cell[,] Cells { get; set; }
        public List<CellType> CellTypes { get; private set; }
        public bool MustUpdate { get; set; }
        public Dictionary<string, Utils.AnimatedTexture> AnimatedTextures { get; set; }

        public Field(Game game, Point resolution, Point cellAmount, int initialBombs) : base(game)
        {
            this.Resolution = resolution;
            this.Size = cellAmount;
            CellSize.X = resolution.X / cellAmount.X;
            CellSize.Y = resolution.Y / cellAmount.Y;
            this.MustUpdate = false;
            this.CellTypes = new List<CellType>(initialBombs);
            this.Engine = game;
            this.AnimatedTextures = new Dictionary<string, Utils.AnimatedTexture>();
        }

        private string GetFirstAvailableColor()
        {
            foreach (string s in availableColors.Keys)
            {
                if (availableColors[s])
                    availableColors[s] = false;
                    return s;
            }
            return "";
        }

        public void AddBomb()
        {
            string usedColor = GetFirstAvailableColor();
            if (usedColor == "")
                throw new IndexOutOfRangeException("No more colors available.");
            
            Point RandomPos = new Point();

            RandomPos.X = new Random(Guid.NewGuid().GetHashCode()).Next(Size.X);
            RandomPos.Y = new Random(Guid.NewGuid().GetHashCode()).Next(Size.Y);
            
            while (Cells[RandomPos.X, RandomPos.Y].Type == CellVariants.Bomb ||
                   IsNextToBomb(RandomPos))
            {
                RandomPos.X = new Random(Guid.NewGuid().GetHashCode()).Next(Size.X);
                RandomPos.Y = new Random(Guid.NewGuid().GetHashCode()).Next(Size.Y);
            }

            CellType c = new CellType(usedColor, RandomPos);
            Cells[RandomPos.X, RandomPos.Y].ChangeType(CellVariants.Bomb, AnimatedTextures["bomb"]);
            this.CellTypes.Add(c);

            this.MustUpdate = true;
        }

        private bool IsNextToBomb(Point position)
        {
            if (position.X - 1 > 0 && position.X + 1 < Cells.GetLength(0))
            {
                if (Cells[position.X + 1, position.Y].Type == CellVariants.Bomb)
                {
                    return true;
                }
                else if (Cells[position.X - 1, position.Y].Type == CellVariants.Bomb)
                {
                    return true;
                }
            }

            if (position.Y - 1 > 0 && position.Y + 1 < Cells.GetLength(1))
            {
                if (Cells[position.X, position.Y + 1].Type == CellVariants.Bomb)
                {
                    return true;
                }
                else if (Cells[position.X, position.Y - 1].Type == CellVariants.Bomb)
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

                    Cell c = new Cell(Bounds, CellVariants.Safe, null);
                    Cells.SetValue(c, x, y);
                }
            }

            MazeGenerator.Do(this);
            if (this.CellTypes.Count != 0)
                this.CellTypes.Clear();
            this.MustUpdate = true;
        }

        public override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // init field
            GenerateRandom();

            for (int i = 0; i < this.CellTypes.Count; i++)
            {
                AddBomb();
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.AnimatedTextures.Add("bomb", 
                                      new Utils.AnimatedTexture(Vector2.Zero, 
                                                                0f, 
                                                                1f,
                                                                0.5f,
                                                                new Point(100, 100)));

            this.AnimatedTextures["bomb"].Load(Engine.Content, "bomb", 20, 15);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (this.MustUpdate)
            {
                for (int x = 0; x < Cells.GetLength(0); x++)
                {
                    for (int y = 0; y < Cells.GetLength(1); y++)
                    {
                        Cell newCell = new Cell(Cells[x, y].Bounds, Cells[x, y].Type, Cells[x, y].Animation);
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
