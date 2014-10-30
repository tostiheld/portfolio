using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace TowerHunterEngine.Playfield
{
    public static class MazeGenerator
    {
        [Flags]
        public enum Directions
        {
            N = 1,
            S = 2,
            E = 4,
            W = 8
        }

        private static Dictionary<Directions, int> DX;
        private static Dictionary<Directions, int> DY;
        private static Dictionary<Directions, Directions> OPPOSITE;

        public static void Do(Field field)
        {
            DX = new Dictionary<Directions, int>
            {
                { Directions.N, 0 },
                { Directions.S, 0 },
                { Directions.E, 1 },
                { Directions.W, -1 }
            };

            DY = new Dictionary<Directions, int>
            {
                { Directions.N, -1 },
                { Directions.S, 1 },
                { Directions.E, 0 },
                { Directions.W, 0 }
            };

            OPPOSITE = new Dictionary<Directions, Directions>
            {
                { Directions.N, Directions.S },
                { Directions.S, Directions.N },
                { Directions.E, Directions.W },
                { Directions.W, Directions.E }
            };

            Point randomPos = new Point();
            randomPos.X = new Random(DateTime.Now.Millisecond).Next(0, field.Size.X);
            randomPos.Y = new Random(DateTime.Now.Millisecond).Next(0, field.Size.Y);
            CarvePassages(randomPos.X, randomPos.Y, field.Cells);
            ValuesToBorders(field);
        }

        private static void CarvePassages(int cx, int cy, Cell[,] cells)
        {
            var directions = new List<Directions>
            {
                Directions.N,
                Directions.S,
                Directions.E,
                Directions.W,
            }
            .OrderBy(x => Guid.NewGuid());

            foreach (var direction in directions)
            {
                int nx = cx + DX[direction];
                int ny = cy + DY[direction];

                if (IsOutOfBounds(nx, ny, cells))
                    continue;

                if (cells[nx, ny].Value != 0)
                    continue;

                cells[cx, cy].Value |= (int)direction;
                cells[nx, ny].Value |= (int)OPPOSITE[direction];

                CarvePassages(nx, ny, cells);
            }
        }

        private static void ValuesToBorders(Field field)
        {
            int rows = field.Cells.GetLength(1);
            int columns = field.Cells.GetLength(0);

            // top border

            for (int i = 0; i < columns; i++)
            {
                field.Cells[i, 0].Borders[0] = true;
            }

            // bottom border
            for (int i = 0; i < columns; i++)
            {
                field.Cells[i, rows - 1].Borders[2] = true;
            }

            for (int y = 0; y < rows; y++)
            {
                field.Cells[0, y].Borders[3] = true;
                for (int x = 0; x < columns; x++)
                {
                    var directions = (Directions)field.Cells[x, y].Value;

                    if (!directions.HasFlag(Directions.N))
                        field.Cells[x, y].Borders[0] = true;

                    if (!directions.HasFlag(Directions.E))
                        field.Cells[x, y].Borders[1] = true;
                }
            }
        }

        private static bool IsOutOfBounds(int x, int y, Cell[,] cells)
        {
            if (x < 0 || x > cells.GetLength(0) - 1)
                return true;

            if (y < 0 || y > cells.GetLength(1) - 1)
                return true;

            return false;
        }
    }
}
