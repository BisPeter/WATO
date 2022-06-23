//----------------------------------------------------------------------
// <copyright file="Tracer.cs" company="FHWN.ac.at">
// Copyright (c) FHWN. All rights reserved.
// </copyright>
// <summary>This Project represents M Consumer and N Producer, each with their own thread.</summary>
// <author>Matthias Mandl</author>
// -----------------------------------------------------------------------

namespace WATO
{
    using System.IO;

    /// <summary>
    /// Defines the <see cref="Tracer" />.
    /// </summary>
    public class Tracer
    {
        /// <summary>
        /// Defines the _path.
        /// </summary>
        private readonly string _path;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tracer"/> class.
        /// </summary>
        public Tracer()
        {
            _path = Directory.GetCurrentDirectory() + @"\trace.txt";
        }

        /// <summary>
        /// Traces the given message.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        public void TraceMessage(string message)
        {
            File.AppendAllText(_path, message + System.Environment.NewLine);
        }
    }
}
