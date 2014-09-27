/*
 *
 * File created by: Thomas Schraven
 * Creation date: 27-09-2014
 * 
 * Last modified by: Thomas Schraven
 * Last modification date: 27-09-2014
 * 
 * algorithm: (recursive backtracking)
 * http://weblog.jamisbuck.org/2010/12/27/maze-generation-recursive-backtracking
 * 
 */

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;

namespace TowerHunterEngine.Playfield
{
    public static class Generator
    {
        [Flags]
        public enum Directions
        {
            N = 1,
            S = 2,
            E = 4,
            W = 8
        }

        private static Square[,] tempgrid;
        private static Dictionary<Directions, int> DX;
        private static Dictionary<Directions, int> DY;
        private static Dictionary<Directions, Directions> OPPOSITE;

        public static Field Generate(Field field)
        {
            // don't touch an empty or already generated field
            if (field == null)
                throw new InvalidOperationException(Utils.Messages.FieldEmpty);
            /*else if (field.Generated)
                throw new InvalidOperationException(Utils.Messages.FieldGenerated);*/
            
            tempgrid = new Square[field.Size.X, field.Size.Y];
            for (int x = 0; x < field.Size.X; x++)
            {
                for (int y = 0; y < field.Size.Y; y++)
                {
                    Square s = new Square(
                        new Point(x, y),
                        field.squareSize,
                        SquareType.Safe);
                    tempgrid.SetValue(s, x, y);
                }
            }

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

            CarvePassages(0, 0, ref tempgrid);

            field.Generated = true;
            field.Grid = tempgrid;
            // TODO: conversion is not needed if the generation is done right
            field = ValuesToBorders(field);
            return field;
        }

        private static void CarvePassages(int cx, int cy, ref Square[,] grid)
        {
            var directions = new List<Directions>
            {
                Directions.N,
                Directions.S,
                Directions.E,
                Directions.W,
            }
            .OrderBy(x => Guid.NewGuid());

            foreach(var direction in directions)
            {
                int nx = cx + DX[direction];
                int ny = cy + DY[direction];

                if (IsOutOfBounds(nx, ny, grid))
                    continue;

                if (grid[nx, ny].Value != 0)
                    continue;

                grid[cx, cy].Value |= (int)direction;
                grid[nx, ny].Value |= (int)OPPOSITE[direction];

                CarvePassages(nx, ny, ref grid);
            }
        }

        private static Field ValuesToBorders(Field input)
        {
            int rows = input.Grid.GetLength(1);
            int columns = input.Grid.GetLength(0);

            // top border
            
            for (int i = 0; i < columns; i++)
            {
                input.Grid[i, 0].Borders[0] = true;
            }

            // bottom border
            for (int i = 0; i < columns; i++)
            {
                input.Grid[i, rows - 1].Borders[2] = true;
            }

            for (int y = 0; y < rows; y++)
            {
                input.Grid[0, y].Borders[3] = true;
                for (int x = 0; x < columns; x++)
                {
                    var directions = (Directions)input.Grid[x, y].Value;

                    if (!directions.HasFlag(Directions.N))
                        input.Grid[x, y].Borders[0] = true;

                    if (!directions.HasFlag(Directions.E))
                        input.Grid[x, y].Borders[1] = true;
                }
            }

            return input;
        }

        private static bool IsOutOfBounds(int x, int y, Square[,] grid)
        {
            if (x < 0 || x > grid.GetLength(0) - 1)
                return true;

            if (y < 0 || y > grid.GetLength(1) - 1)
                return true;

            return false;
        }
    }
}
