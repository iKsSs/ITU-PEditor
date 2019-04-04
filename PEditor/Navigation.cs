using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace PEditor
{
    class Navigation
    {

        private Bitmap bitmap;

        public Navigation(Bitmap bitmap)
        {
            this.bitmap = bitmap;
        }

        public Bitmap drawNavigation()
        {
            int width = 256;
            int height = 100;
            return resizeImage(bitmap, width, height);
        }

        private Bitmap resizeImage(Bitmap image,int canvasWidth, int canvasHeight)
        {
            int originalWidth = image.Width;
            int originalHeight = image.Height;
        

            Bitmap thumbnail =
                new Bitmap(canvasWidth, canvasHeight); // changed parm names
            Graphics graphic = Graphics.FromImage(thumbnail);

            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphic.CompositingQuality = CompositingQuality.HighQuality;

            /* ------------------ new code --------------- */

            // Figure out the ratio
            double ratioX = (double)canvasWidth / (double)originalWidth;
            double ratioY = (double)canvasHeight / (double)originalHeight;
            // use whichever multiplier is smaller
            double ratio = ratioX < ratioY ? ratioX : ratioY;

            // now we can get the new height and width
            int newHeight = Convert.ToInt32(originalHeight * ratio);
            int newWidth = Convert.ToInt32(originalWidth * ratio);

            // Now calculate the X,Y position of the upper-left corner 
            // (one of these will always be zero)
            int posX = Convert.ToInt32((canvasWidth - (originalWidth * ratio)) / 2);
            int posY = Convert.ToInt32((canvasHeight - (originalHeight * ratio)) / 2);

            graphic.Clear(Color.FromArgb(255, 240, 240, 240)); // white padding
            graphic.DrawImage(image, posX, posY, newWidth, newHeight);
            graphic.Dispose();
            return thumbnail;
        }
    }   
}
