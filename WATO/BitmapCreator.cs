using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace WATO
{
    public static class BitmapCreator
    {
        private static List<Bitmap> _bitmaps = new List<Bitmap>();

        private static List<IEnumerable<IEnumerable<bool>>> _bools = new List<IEnumerable<IEnumerable<bool>>>();

        public static void CreateBimapImagefromPayloadBoolArrayPlease(IEnumerable<IEnumerable<bool>> dataGrid)
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

        internal static void AddImageToList(List<List<bool>> dataGrid)
        {
            var deepCopyDataGrid = new List<List<bool>>(dataGrid.Select(d =>new List<bool>(d.Select(dd => dd))));

            _bools.Add(deepCopyDataGrid);

            if(_bools.Count % 10 == 0)
            {
                _bools.ForEach(x => CreateBimapImagefromPayloadBoolArrayPlease(x));
                ImageSaver.Saveimages(_bitmaps); 
                _bitmaps.Clear();
            }
        }
    }
}
