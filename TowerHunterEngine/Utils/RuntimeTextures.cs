using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using Drawing = System.Drawing;
using Imaging = System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace TowerHunterEngine.Utils
{
    static class RuntimeTextures
    {
        static private Drawing.Color ConvertXNAColor(Color color)
        {
            return Drawing.Color.FromArgb(color.A, color.B, color.G, color.R);;
        }

        // method to create basic colored square
        static public Texture2D Basic(GraphicsDevice device, Color color)
        {
            Texture2D texture = new Texture2D(device, 1, 1);
            List<Color> c = new List<Color>(1);
            c.Add(color);
            texture.SetData<Color>(c.ToArray());
            return texture;
        }

        // TODO: check for performance issues
        static public Texture2D BasicBordered(GraphicsDevice device, Color fill, bool[] borders)
        {
            // texture size and adjusted int for use in arrays
            int size = 5;
            int arraysize = size - 1;

            Drawing.Color usedColor = ConvertXNAColor(fill);
            
            // create bitmap in memory rather than loading it from disk
            // 'if' statements are for detecting which borders are drawn
            
            // GetPixel() and SetPixel() are the worst for performance,
            // but oh well..

            Drawing.Bitmap bmp = new Drawing.Bitmap(size, size);

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    bmp.SetPixel(x, y, usedColor);
                }
            }

            if (borders[0]) // north border
            {
                for (int i = 0; i < size; i++)
                {
                    bmp.SetPixel(i, 0, Drawing.Color.Black);
                }
            }
            if (borders[1]) // east border
            {
                for (int i = 0; i < size; i++)
                {
                    bmp.SetPixel(arraysize, i, Drawing.Color.Black);
                }
            }            
            if (borders[2]) // south border
            {
                for (int i = 0; i < size; i++)
                {
                    bmp.SetPixel(i, arraysize, Drawing.Color.Black);
                }
            }
            if (borders[3]) // west border
            {
                for (int i = 0; i < size; i++)
                {
                    bmp.SetPixel(0, i, Drawing.Color.Black);
                }
            }
            // correction of one pixel
            // a gap of one pixel exists if the north and east border
            // are not drawn
            if (!borders[0] && !borders[1])
            {
                bmp.SetPixel(arraysize, 0, Drawing.Color.Black);
            }
            
            Texture2D texture = new Texture2D(device, size, size);

            // lockbits improve performance
            Imaging.BitmapData data;
            data = bmp.LockBits(
                new Drawing.Rectangle(0, 0, size, size), 
                Imaging.ImageLockMode.ReadOnly,
                bmp.PixelFormat);

            int bufferSize = data.Height * data.Stride;
            byte[] bytes = new byte[bufferSize];
            Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);
            
            texture.SetData<byte>(bytes);

            return texture;
        }

        static public Texture2D ShadowedBackground(GraphicsDevice device, Color fill, Point size)
        {
            Drawing.Bitmap bmp = new Drawing.Bitmap(size.X, size.Y);
            Drawing.Color usedColor = ConvertXNAColor(fill);

            for (int x = 0; x < size.X - 1; x++)
            {
                for (int y = 0; y < size.Y - 1; y++)
                {
                    bmp.SetPixel(x, y, usedColor);
                }
            }

            for (int x = 1; x < size.X; x++)
            {
                bmp.SetPixel(x, size.Y - 1, Drawing.Color.Gray);
            }

            for (int y = 1; y < size.Y; y++)
            {
                bmp.SetPixel(size.X - 1, y, Drawing.Color.Gray);
            }

            Texture2D texture = new Texture2D(device, size.X, size.Y);

            // lockbits improve performance
            Imaging.BitmapData data;
            data = bmp.LockBits(
                new Drawing.Rectangle(0, 0, size.X, size.Y),
                Imaging.ImageLockMode.ReadOnly,
                bmp.PixelFormat);

            int bufferSize = data.Height * data.Stride;
            byte[] bytes = new byte[bufferSize];
            Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);

            texture.SetData<byte>(bytes);

            return texture;
        }
    }
}
