using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using Drawing = System.Drawing;
using Imaging = System.Drawing.Imaging;
using System.Runtime.InteropServices;

using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;

namespace BombDefuserEngine.Utils
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

        static public Texture2D GenerateQRCode(GraphicsDevice device, string text)
        {
            QrEncoder encoder = new QrEncoder(ErrorCorrectionLevel.M);
            QrCode qrCode;
            encoder.TryEncode(text, out qrCode);

            GraphicsRenderer gRenderer = new GraphicsRenderer(
                new FixedModuleSize(2, QuietZoneModules.Two));

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            gRenderer.WriteToStream(qrCode.Matrix, Drawing.Imaging.ImageFormat.Bmp, ms);

            Drawing.Bitmap bmp = new Drawing.Bitmap(ms);

            Texture2D texture = new Texture2D(device, bmp.Width, bmp.Height);

            // lockbits improve performance
            Imaging.BitmapData data;
            data = bmp.LockBits(
                new Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
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
