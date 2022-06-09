using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WATO;

namespace WatorConsoleProj
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<List<bool>> b = new List<List<bool>>();
            //{
            //    new List<bool>{ false,true,false,false,false,false,false,false,false },
            //    new List<bool>{ false,false,true,false,false,false,false,false,false },
            //    new List<bool>{ true, true, true,false,false,false,false,false,false },
            //    new List<bool>{ false, false, false,false,false,false,false,false,false },
            //    new List<bool>{ false, false, false,false,false,false,false,false,false },
            //    new List<bool>{ false, false, false,false,false,false,false,false,false },
            //    new List<bool>{ false, false, false,false,false,false,false,false,false },
            //    new List<bool>{ false, false, false,false,false,false,false,false,false },
            //    new List<bool>{ false, false, false,false,false,false,false,false,false },
            //    new List<bool>{ false, false, false,false,false,false,false,false,false },
            //};
            var random = new Random();



            for (int i = 0; i < 10000; i++)
            {
                b.Add(new List<bool>());
                for (int j = 0; j < 10000; j++)
                {
                    if (random.NextDouble() > 0.5)
                        b.ElementAt(i).Add(true);
                    else
                        b.ElementAt(i).Add(false);
                }
            }
            Master m = new Master(10000, 8, b);
            m.Start();
        }
    }
}
