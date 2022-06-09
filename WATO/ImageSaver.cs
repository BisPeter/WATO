using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WATO
{
    internal static class ImageSaver
    {
        private static int _bildcounter = 0;

        public static void Saveimages(List<Bitmap> bitmaps)
        {
            Parallel.For(0, bitmaps.Count, i =>
             {
                 var curr = bitmaps.ElementAt(i);

                 using (curr) { curr.Save(Directory.GetCurrentDirectory() + $"kukidrukki{i}.bmp", ImageFormat.Bmp); }
             });
        }

        public static void Saveimage(Bitmap bitmap)
        {
            using (bitmap) { bitmap.Save(Directory.GetCurrentDirectory() + $"kukidrukki{_bildcounter}.bmp", ImageFormat.Bmp); }
            _bildcounter++;
        }
    }
}