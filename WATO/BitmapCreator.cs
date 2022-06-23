//----------------------------------------------------------------------
// <copyright file=BitmapCreator.cs company="FHWN.ac.at">
// Copyright (c) FHWN. All rights reserved.
// </copyright>
// <summary>This Project represents a WATOR simulation, implemented with a Master-Worker Pattern</summary>
// <author>Matthias Mandl & Peter Vadle</author>
// -----------------------------------------------------------------------

namespace WATO
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a Creator of Bitmaps, which can save them in a file.
    /// </summary>
    public static class BitmapCreator
    {
        /// <summary>
        /// The finished bitmaps.
        /// </summary>
        private static List<Bitmap> _bitmaps = new List<Bitmap>();

        /// <summary>
        /// The list of 2d bools list = raw data.
        /// </summary>
        private static List<IEnumerable<IEnumerable<bool>>> _bools = new List<IEnumerable<IEnumerable<bool>>>();

        /// <summary>
        /// Gets or sets the PicturesTillSave
        /// Gets or sets how many bitmaps to store before saving them as files..
        /// </summary>
        public static int PicturesTillSave { get; set; } = 10;

        /// <summary>
        /// Creates the bimapfrom list of lists.
        /// </summary>
        /// <param name="dataGrid">The data grid = raw data.</param>
        public static void CreateBimapfromListOfLists(IEnumerable<IEnumerable<bool>> dataGrid)
        {
            var columnsNumber = dataGrid.Count();
            var rowsNumber = dataGrid.ElementAt(0).Count();

            Bitmap bi = new Bitmap(columnsNumber, rowsNumber);

            for (int i = 0; i < rowsNumber; i++) // row
            {
                for (int j = 0; j < columnsNumber; j++) // column
                {
                    if (dataGrid.ElementAt(i).ElementAt(j))
                    {
                        //   x=j=column, y=i=row  
                        bi.SetPixel(j, i, System.Drawing.Color.Red);
                    }
                    else
                    {
                        bi.SetPixel(j, i, System.Drawing.Color.Green);
                    }
                }
            }

            _bitmaps.Add(bi);
        }

        /// <summary>
        /// Adds the image to list.
        /// When pictures till save reached => create the bitmaps and save them as files.
        /// </summary>
        /// <param name="dataGrid">The data grid.</param>
        /// <param name="maximalThreadsCount">The maximal threads count.</param>
        public static void AddImageToList(List<List<bool>> dataGrid, int maximalThreadsCount)
        {
            var deepCopyDataGrid = new List<List<bool>>(dataGrid.Select(d => new List<bool>(d.Select(dd => dd))));

            _bools.Add(deepCopyDataGrid);

            if (_bools.Count % PicturesTillSave == 0)
            {
                foreach (var item in _bools)
                    CreateBimapfromListOfLists(item);
                ImageSaver.Saveimages(_bitmaps, maximalThreadsCount);
                _bitmaps.Clear();
                _bools.Clear();
            }
        }
    }
}
