//----------------------------------------------------------------------
// <copyright file=ImageSaver.cs company="FHWN.ac.at">
// Copyright (c) FHWN. All rights reserved.
// </copyright>
// <summary>This Project represents a WATOR simulation, implemented with a Master-Worker Pattern</summary>
// <author>Matthias Mandl & Peter Vadle</author>
// -----------------------------------------------------------------------

namespace WATO
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a saver of images to files.
    /// </summary>
    public static class ImageSaver
    {
        /// <summary>
        /// The current pictures count
        /// </summary>
        private static int _pictureCount = 0;

        /// <summary>
        /// Saves the specified bitmaps to files.
        /// </summary>
        /// <param name="bitmaps">The bitmaps.</param>
        /// <param name="maximalThreadsCount">The maximal threads count.</param>
        public static void Saveimages(List<Bitmap> bitmaps, int maximalThreadsCount)
        {
            var pOptions = new ParallelOptions();
            // Gets or sets the maximum number of concurrent tasks enabled by this ParallelOptions instance.
            pOptions.MaxDegreeOfParallelism = maximalThreadsCount; // maximal Threads

            Parallel.For(0, bitmaps.Count, pOptions, i =>
             {
                 var curr = bitmaps.ElementAt(i);

                 using (curr) { curr.Save(Directory.GetCurrentDirectory() + $"/Picture{_pictureCount + i}.bmp", ImageFormat.Bmp); }
             });

            _pictureCount = _pictureCount + bitmaps.Count;
        }
    }
}
