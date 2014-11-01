using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using TowerHunterEngine.Utils;

namespace TowerHunterEngine.Playfield
{
    public class Field : DrawableGameComponent, IDisposable
    {
        public Dictionary<string, Utils.AvailableColor> AvailableColors = new Dictionary<string, Utils.AvailableColor>
        {
            {"red",     new AvailableColor(RobotColors.Red, true)},
            {"green",   new AvailableColor(RobotColors.Green, true)},
            {"blue",    new AvailableColor(RobotColors.Blue, true)},
            {"cyan",    new AvailableColor(RobotColors.Cyan, true)},
            {"magenta", new AvailableColor(RobotColors.Magenta, true)},
            {"yellow",  new AvailableColor(RobotColors.Yellow, true)},
            {"orange",  new AvailableColor(RobotColors.Orange, true)},
        };

        SpriteBatch spriteBatch;
        private Point CellSize;
        private Game Parent;

        public Point Resolution { get; private set; }
        public Point Size { get; private set; }
        public Cell[,] Cells { get; set; }
        public bool MustUpdate { get; set; }
        public Dictionary<string, Utils.AnimatedTexture> AnimatedTextures { get; set; }
        public int BombAmount { get; set; }

        public Field(Game game, Point resolution, Point cellAmount, int initialBombs) : base(game)
        {
            this.Resolution = resolution;
            this.Size = cellAmount;
            CellSize.X = resolution.X / cellAmount.X;
            CellSize.Y = resolution.Y / cellAmount.Y;
            this.MustUpdate = false;
            this.Parent = game;
            this.AnimatedTextures = new Dictionary<string, Utils.AnimatedTexture>();
            this.BombAmount = initialBombs;
        }

        public Color GetFirstAvailableColor()
        {
            foreach (string s in AvailableColors.Keys)
            {
                if (AvailableColors[s].Available)
                {
                    AvailableColors[s].Available = false;
                    return AvailableColors[s].Value;
                }
            }
            throw new IndexOutOfRangeException("No more available colors.");
        }

        public void ResetCell(Color color)
        {
            foreach (Cell c in this.Cells)
            {
                if (c.Fill == color)
                {
                    c.ChangeType(CellType.Safe);
                    foreach (Utils.AvailableColor ac in AvailableColors.Values)
                    {
                        if (ac.Value == color)
                        {
                            ac.Available = true;
                        }
                    }
                }
            }
        }

        public void AddBomb()
        {
            Color usedColor = GetFirstAvailableColor();
            
            Point RandomPos = new Point();

            RandomPos.X = new Random(Guid.NewGuid().GetHashCode()).Next(Size.X);
            RandomPos.Y = new Random(Guid.NewGuid().GetHashCode()).Next(Size.Y);
            
            while (Cells[RandomPos.X, RandomPos.Y].Type == CellType.Bomb ||
                   IsNextToBomb(RandomPos))
            {
                RandomPos.X = new Random(Guid.NewGuid().GetHashCode()).Next(Size.X);
                RandomPos.Y = new Random(Guid.NewGuid().GetHashCode()).Next(Size.Y);
            }
            Cells[RandomPos.X, RandomPos.Y].ChangeType(CellType.Bomb, usedColor, AnimatedTextures["bomb"]);

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

                    Cell c = new Cell(Bounds);
                    Cells.SetValue(c, x, y);
                }
            }

            MazeGenerator.Do(this);
            this.MustUpdate = true;
        }

        public override void Initialize()
        {
            base.Initialize();

            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // init field
            GenerateRandom();

            for (int i = 0; i < this.BombAmount; i++)
            {
                AddBomb();
            }
        }

        protected override void LoadContent()
        {
            this.AnimatedTextures.Add("bomb", 
                                      new Utils.AnimatedTexture(Vector2.Zero, 
                                                                0f, 
                                                                1f,
                                                                0.5f,
                                                                new Point(100, 100)));

            this.AnimatedTextures["bomb"].Load(Parent.Content, "bomb", 20, 10);

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
                        Cell newCell = new Cell(Cells[x, y].Bounds, 
                                                Cells[x, y].Type, 
                                                Cells[x, y].Fill, 
                                                Cells[x, y].Animation);

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
