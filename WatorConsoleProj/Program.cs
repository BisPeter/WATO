//----------------------------------------------------------------------
// <copyright file=Program.cs company="FHWN.ac.at">
// Copyright (c) FHWN. All rights reserved.
// </copyright>
// <summary>This Project represents the console Project of a WATOR simulation, implemented with a Master-Worker Pattern</summary>
// <author>Matthias Mandl & Peter Vadle</author>
// -----------------------------------------------------------------------

namespace WatorConsoleProj
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using WATO;

    /// <summary>
    /// The program class with the entry point.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The entry point of this project.
        /// </summary>
        /// <param name="args">The params.</param>
        internal static void Main(string[] args)
        {
            //args = new string[] { "100", "2", "2:1.3:2.1:3.2:3.3:3" }; glider test 
            ArgsHandler argsHandler = new ArgsHandler();
            argsHandler.HandleArgs(args);            

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

            for (int i = 0; i < argsHandler.Pixels; i++)
            {
                b.Add(new List<bool>());
                for (int j = 0; j < argsHandler.Pixels; j++)
                {
                    if(args.Count() > 2)  // user set argument for trues
                    {
                        if (argsHandler.TempArray[j, i])
                            b.ElementAt(i).Add(true);
                        else
                            b.ElementAt(i).Add(false);
                    }
                    else // default, fill with random
                    {
                        if (random.NextDouble() > 0.5)
                            b.ElementAt(i).Add(true);
                        else
                            b.ElementAt(i).Add(false);
                    }                    
                }
            }
            Master m = new Master(argsHandler.ThreadsCount, b);
            m.Start();
        }
    }
}
