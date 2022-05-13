using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WATO
{
    public class Worker
    {
        private Payload _payload;

        public Payload Payload { get => _payload; set => _payload = value; }
    }
}
