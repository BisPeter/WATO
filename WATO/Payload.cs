//----------------------------------------------------------------------
// <copyright file=Payload.cs company="FHWN.ac.at">
// Copyright (c) FHWN. All rights reserved.
// </copyright>
// <summary>This Project represents a WATOR simulation, implemented with a Master-Worker Pattern</summary>
// <author>Matthias Mandl & Peter Vadle</author>
// -----------------------------------------------------------------------

namespace WATO
{
    /// <summary>
    /// The payload of a worker.
    /// </summary>
    public class Payload
    {
        /// <summary>
        /// Gets or sets the data chunk..
        /// </summary>
        public bool[,] DataChunk { get; set; }

        /// <summary>
        /// Gets or sets the row number, where to insert in the data grid..
        /// </summary>
        public int RowNumber { get; set; }
    }
}
