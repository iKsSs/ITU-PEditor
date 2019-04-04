using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace PEditor
{
    class Filters
    {
        private Bitmap bitmap;

        public Filters(ref Bitmap bitmap)
        {
            this.bitmap = bitmap;
        }

        public void setRef(ref Bitmap bitmap)
        {
            this.bitmap = bitmap;
        }

        private double thresoldCalc(int threshold)
        {
            return Math.Pow(((100 + (double)threshold) / 100), 2);
        }

        unsafe public void setContrast(int threshold)
        {
            BitmapData bd;
            double contrast;

            if (threshold < -100 || threshold > 100)
            {
                throw (new Exception("Neplatná hodnota\n"));
            }

            bd = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            contrast = thresoldCalc(threshold);

            for (int y = 0; y < bitmap.Height; ++y)
            {
                int* ptr = (int*)((int)bd.Scan0 + y * bd.Stride);

                for (int x = 0; x < bitmap.Width; ++x)
                {
                    Color color;
                    int R, G, B;

                    color = Color.FromArgb(*ptr);   //get color from pointer

                    R = Math.Min(Math.Max(0, (int)((((((double)color.R / 255) - 0.5) * contrast) + 0.5) * 255)), 255);
                    G = Math.Min(Math.Max(0, (int)((((((double)color.G / 255) - 0.5) * contrast) + 0.5) * 255)), 255);
                    B = Math.Min(Math.Max(0, (int)((((((double)color.B / 255) - 0.5) * contrast) + 0.5) * 255)), 255);

                    color = Color.FromArgb(R, G, B);  //set new color

                    *ptr = color.ToArgb();

                    ptr++;
                }
            }

            bitmap.UnlockBits(bd);
        }

        unsafe public void setBrightness(int brightness)
        {
            BitmapData bd;

            if (brightness < -255 || brightness > 255)
            {
                throw (new Exception("Neplatná hodnota\n"));
            }

            bd = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);


            for (int y = 0; y < bitmap.Height; ++y)
            {
                int* ptr = (int*)((int)bd.Scan0 + y * bd.Stride);

                for (int x = 0; x < bitmap.Width; ++x)
                {
                    Color color;
                    int R, G, B;

                    color = Color.FromArgb(*ptr);   //get color from pointer

                    R = Math.Min(Math.Max(0, color.R + brightness), 255);
                    G = Math.Min(Math.Max(0, color.G + brightness), 255);
                    B = Math.Min(Math.Max(0, color.B + brightness), 255);
                    
                    color = Color.FromArgb(R, G, B);

                    *ptr = color.ToArgb();

                    ptr++;
                }
            }

            bitmap.UnlockBits(bd);
        }

        unsafe public void toGrayScale()
        {
            BitmapData bd;

            bd = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            
            for (int y = 0; y < bitmap.Height; ++y)
            {
                int* ptr = (int*)((int)bd.Scan0 + y * bd.Stride);

                for (int x = 0; x < bitmap.Width; ++x)
                {
                    Color color;
                    int R, G, B;

                    color = Color.FromArgb(*ptr);

                    R = G = B = R = Math.Min(Math.Max(0, (int)(color.R * 0.21 + color.G * 0.72 + color.B * 0.07)), 255);

                    color = Color.FromArgb(R, G, B);

                    *ptr = color.ToArgb();

                    ptr++;
                }
            }

            bitmap.UnlockBits(bd);
        }

        unsafe public void toInvert()
        {
            BitmapData bd;

            bd = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            for (int y = 0; y < bitmap.Height; ++y)
            {
                int* ptr = (int*)((int)bd.Scan0 + y * bd.Stride);

                for (int x = 0; x < bitmap.Width; ++x)
                {
                    Color color;
                    int R, G, B;

                    color = Color.FromArgb(*ptr);

                    R = Math.Min(Math.Max(0, 255 - color.R), 255);
                    G = Math.Min(Math.Max(0, 255 - color.G), 255);
                    B = Math.Min(Math.Max(0, 255 - color.B), 255);

                    color = Color.FromArgb(R, G, B);

                    *ptr = color.ToArgb();

                    ptr++;
                }
            }

            bitmap.UnlockBits(bd);
        }

        unsafe public void toBlackAndWhite(int threshold)
        {
            BitmapData bd;

            bd = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            for (int y = 0; y < bitmap.Height; ++y)
            {
                int* ptr = (int*)((int)bd.Scan0 + y * bd.Stride);

                for (int x = 0; x < bitmap.Width; ++x)
                {
                    Color color;

                    color = Color.FromArgb(*ptr);

                    if ((int)(color.R * 0.21 + color.G * 0.72 + color.B * 0.07) > threshold)
                    {
                        color = Color.White;
                    }
                    else
                    {
                        color = Color.Black;
                    }

                    *ptr = color.ToArgb();

                    ptr++;
                }
            }

            bitmap.UnlockBits(bd);
        }

        public unsafe void blur(int effect)
        {
            BitmapData bd = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            Random rand = new Random();

            for (int y = 0; y < bitmap.Height; ++y)
            {
                int* ptr = (int*)((int)bd.Scan0 + y * bd.Stride);

                for (int x = 0; x < bitmap.Width; x++)
                {
                    int x2 = Math.Max(Math.Min(bitmap.Width - 1, rand.Next(-effect, effect) + x), 0);
                    int y2 = Math.Max(Math.Min(bitmap.Height - 1, rand.Next(-effect, effect) + y), 0);
                    int* ptr2 = (int*)((int)bd.Scan0 + y2 * bd.Stride + x2 * 4);
                    Color color = Color.FromArgb(*ptr);
                    *ptr2 = color.ToArgb();
                    ptr++;
                }
            }

            bitmap.UnlockBits(bd);
        }

        public void setRotateFlip(int dir)
        {
            switch (dir)
            {
                case 1:
                    bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone); break;
                case 2:
                    bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone); break;
                case 3:
                    bitmap.RotateFlip(RotateFlipType.RotateNoneFlipX); break;
                case 4:
                    bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY); break;
                case 5:
                    bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone); break;
                default: break;
            }
        }
    }
}
