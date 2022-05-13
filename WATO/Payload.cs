using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WATO
{
    public class Payload
    {
        public bool[,] DataChunk { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
    }
}
