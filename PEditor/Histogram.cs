using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace PEditor
{
    class Histogram
    {
        private int[] histogramR = new int[256];
        private int[] histogramG = new int[256];
        private int[] histogramB = new int[256];

        private Bitmap bitmap;

        private int max;

        public Histogram(Bitmap bitmap)
        {
            this.bitmap = bitmap;
        }

        public void setRef(Bitmap bitmap)
        {
            this.bitmap = bitmap;
        }

        unsafe public void createHistogram()
        {
            BitmapData bd;

            max = 0;
            bd = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            for (int y = 0; y < bitmap.Height; ++y)
            {
                int* ptr = (int*)((int)bd.Scan0 + y * bd.Stride);

                for (int x = 0; x < bitmap.Width; ++x)
                {
                    Color color = Color.FromArgb(*ptr);

                    histogramR[color.R] = histogramR[color.R] + 1;
                    histogramG[color.G] = histogramG[color.G] + 1;
                    histogramB[color.B] = histogramB[color.B] + 1;

                    ptr++;
                }
            }

            bitmap.UnlockBits(bd);
        }

        public Bitmap drawHistogram()
        {
            Bitmap bmp = new Bitmap(256, 100);
            Graphics g = Graphics.FromImage(bmp);

            max = histogramB.Max();

            if (histogramG.Max() > max)
            {
                max = histogramG.Max();
            }

            if (histogramR.Max() > max)
            {
                max = histogramR.Max();
            }

            for (int i = 0; i < 255; ++i)
            {
                g.DrawLine(new Pen(Color.Red, 1), new Point(i, (int)(100 - (((double)histogramR[i] / max) * 100))), new Point(i + 1, (int)(100 - (((double)histogramR[i + 1] / max ) * 100))));
                g.DrawLine(new Pen(Color.Green, 1), new Point(i, (int)(100 - (((double)histogramG[i] / max) * 100))), new Point(i + 1, (int)(100 - (((double)histogramG[i + 1] / max) * 100))));
                g.DrawLine(new Pen(Color.Blue, 1), new Point(i, (int)(100 - (((double)histogramB[i] / max) * 100))), new Point(i + 1, (int)(100 - (((double)histogramB[i + 1] / max) * 100))));
            }

            g.Dispose();

            return bmp;
        }
    }   
}
