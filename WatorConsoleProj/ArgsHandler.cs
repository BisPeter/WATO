//----------------------------------------------------------------------
// <copyright file=ArgsHandler.cs company="FHWN.ac.at">
// Copyright (c) FHWN. All rights reserved.
// </copyright>
// <summary>This Project represents the console Project of a WATOR simulation, implemented with a Master-Worker Pattern</summary>
// <author>Matthias Mandl & Peter Vadle</author>
// -----------------------------------------------------------------------

namespace WatorConsoleProj
{
    using System;
    using System.Linq;

    /// <summary>
    /// Represents a handler for console args.
    /// </summary>
    public class ArgsHandler
    {
        /// <summary>
        /// The pixels of one dimension.
        /// </summary>
        private int _pixels = 1000;

        /// <summary>
        /// The threads count.
        /// </summary>
        private int _threadsCount = 4;

        /// <summary>
        /// The temporary array for the initial input values.
        /// </summary>
        private bool[,] _tempArray = new bool[0, 0];

        /// <summary>
        /// Handles the conosle arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public void HandleArgs(string[] args)
        {
            if (args.Count() > 0)
            {
                if (int.TryParse(args[0], out int inputPixels))
                {
                    if (inputPixels > 100000 || inputPixels < 2)
                        Console.WriteLine("inputPixels must be between 2 and 100000 => so we took default: " + _pixels);
                    else
                        _pixels = inputPixels;
                }
                else
                    Console.WriteLine("inputPixels must be integer => so we took default: " + _pixels);
            }
            if (args.Count() > 1)
            {
                if (int.TryParse(args[1], out int inputThreads))
                {
                    if (inputThreads > 1024 || inputThreads < 1)
                        Console.WriteLine("inputThreads must be between 1 and 1024 => so we took default: " + _threadsCount);
                    else
                        _threadsCount = inputThreads;
                }
                else
                    Console.WriteLine("inputThreads must be integer => so we took default: " + _threadsCount);
            }
            if (args.Count() > 2)
            {
                _tempArray = new bool[_pixels, _pixels];

                // 2:4.6:6.14:12

                string[] trues = args[2].Split('.');

                if (trues.Any(tr =>!tr.Contains(':')))
                    Console.WriteLine("Something of your input was wrong, we only accept correct formatted e.g. 2:4.6:6.14:12, we try to insert what is right");

                foreach (var item in trues)
                {
                    string[] vals = item.Split(':');

                    if (vals.Length == 2 && int.TryParse(vals[0], out int x))
                        if (int.TryParse(vals[1], out int y))
                        {
                            try
                            {
                                _tempArray[x, y] = true;
                            }
                            catch
                            {
                                Console.WriteLine("Couldn't insert value: " + item);
                            }
                        }
                }
            }
        }

        /// <summary>
        /// Gets the pixels of one dimension.
        /// </summary>
        public int Pixels
        {
            get { return _pixels; }
        }

        /// <summary>
        /// Gets the threads count.
        /// </summary>
        public int ThreadsCount
        {
            get { return _threadsCount; }
        }

        /// <summary>
        /// Gets the temporary array for the initial input values.
        /// </summary>
        public bool[,] TempArray
        {
            get { return _tempArray; }
        }
    }
}
