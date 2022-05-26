using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace WATO
{
    public static class BitmapCreator
    {
        private static List<Bitmap> _bitmaps = new List<Bitmap>();
        private static List<bool[,]> _bools = new List<bool[,]>();

        public static void CreateBimapImagefromPayloadBoolArrayPlease(bool[,] array)
        {
            Bitmap bi = new Bitmap(array.GetLength(0), array.GetLength(1));

            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (array[i, j])
                        bi.SetPixel(i, j, System.Drawing.Color.Red);
                    else
                        bi.SetPixel(i, j, System.Drawing.Color.Green);

                }
            }

            _bitmaps.Add(bi);                    
        }

        internal static void AddImageToList(bool[,] resultPublishedImage)
        {
            _bools.Add(resultPublishedImage);
            if(_bools.Count % 50 == 0)
            {
                _bools.ForEach(x => CreateBimapImagefromPayloadBoolArrayPlease(x));
                ImageSaver.Saveimages(_bitmaps); 
                _bitmaps.Clear();
            }
        }
    }
}
