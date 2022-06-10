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

        public static void Saveimages(List<Bitmap> bitmaps, int maximalThreadsCount)
        {
            var pOptions = new ParallelOptions();
            // Gets or sets the maximum number of concurrent tasks enabled by this ParallelOptions instance.
            pOptions.MaxDegreeOfParallelism = maximalThreadsCount; // maximal Threads

            Parallel.For(0, bitmaps.Count, pOptions, i =>
             {
                 var curr = bitmaps.ElementAt(i);

                 using (curr) { curr.Save(Directory.GetCurrentDirectory() + $"/Picture{i}.bmp", ImageFormat.Bmp); }
             });
        }
    }
}