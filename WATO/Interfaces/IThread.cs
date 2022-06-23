//----------------------------------------------------------------------
// <copyright file=IThread.cs company="FHWN.ac.at">
// Copyright (c) FHWN. All rights reserved.
// </copyright>
// <summary>This Project represents a WATOR simulation, implemented with a Master-Worker Pattern</summary>
// <author>Matthias Mandl & Peter Vadle</author>
// -----------------------------------------------------------------------

namespace WATO.Interfaces
{
    /// <summary>
    /// Represents a thread.
    /// </summary>
    public interface IThread
    {
        /// <summary>
        /// Stops the worker and thread.
        /// </summary>
        void Stop();

        /// <summary>
        /// Starts the worker and thread.
        /// </summary>
        void Start();

        /// <summary>
        /// Worker of this thread.
        /// </summary>
        void Work();
    }
}
