using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace WATO
{
    internal static class ImageSaver
    {
        private static int _bildcounter = 0;

        public static void Saveimages(List<Bitmap> bitmaps)
        {

            bitmaps.ForEach(bitmap =>
            {
                using (bitmap) { bitmap.Save(Directory.GetCurrentDirectory() + $"kukidrukki{_bildcounter}.bmp", ImageFormat.Bmp); }
                _bildcounter++;
            });
        }

        public static void Saveimage(Bitmap bitmap)
        {
            using (bitmap) { bitmap.Save(Directory.GetCurrentDirectory() + $"kukidrukki{_bildcounter}.bmp", ImageFormat.Bmp); }
            _bildcounter++;
        }
    }
}