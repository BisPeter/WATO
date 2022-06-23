//----------------------------------------------------------------------
// <copyright file=Worker.cs company="FHWN.ac.at">
// Copyright (c) FHWN. All rights reserved.
// </copyright>
// <summary>This Project represents a WATOR simulation, implemented with a Master-Worker Pattern</summary>
// <author>Matthias Mandl & Peter Vadle</author>
// -----------------------------------------------------------------------

namespace WATO
{
    using System;
    using System.Threading;
    using WATO.Interfaces;

    /// <summary>
    /// Represents a Worker of the Master - Worker pattern.
    /// </summary>
    public class Worker : IThread
    {
        /// <summary>
        /// The current payload to calculate..
        /// </summary>
        private Payload _payload;

        /// <summary>
        /// Occurs when [fire on worker is done].
        /// </summary>
        public event EventHandler<Payload> FireOnWorkerIsDone;

        /// <summary>
        /// The thread.
        /// </summary>
        private Thread _thread;

        /// <summary>
        /// If the thread is running.
        /// </summary>
        private bool _running;

        /// <summary>
        /// If the worker is working.
        /// </summary>
        private bool _working;

        /// <summary>
        /// Starts the worker and thread.
        /// </summary>
        public void Start()
        {
            if (_running) { throw new InvalidOperationException(); }
            _running = true;
            _thread = new Thread(Work);
            _thread.Start();
        }

        /// <summary>
        /// Stops the worker and thread.
        /// </summary>
        public void Stop()
        {
            if (!_running) { throw new InvalidOperationException(); }
            if (_thread != Thread.CurrentThread)
            {
                _thread.Join();
                _running = false;

            }
        }

        /// <summary>
        /// Accepts the payload, which was given from the master.
        /// </summary>
        /// <param name="payload">The payload.</param>
        public void AcceptPayload(Payload payload)
        {
            if (_working) { throw new ArgumentException("[AcceptPayload]: cannot get payload when still working"); }
            _payload = payload;
            _working = true;
        }

        /// <summary>
        /// Worker of this thread.
        /// </summary>
        public void Work()
        {
            while (_running)
            {
                if (!_working) { Thread.Sleep(5); continue; }
                Payload payload = CalculatePayload(_payload);

                _working = false;
                _payload = null;
                FireOnWorkerIsDone(this, payload);
            }
        }

        /// <summary>
        /// Calculates the given payload from the master and returns
        /// the calculated payload.
        /// </summary>
        /// <param name="payload">The payload from the master.</param>
        /// <returns>the calculated payload.</returns>
        private Payload CalculatePayload(Payload payload)
        {
            Payload payload2 = new Payload { RowNumber = payload.RowNumber };
            bool[,] calculatedDataChunk = new bool[payload.DataChunk.GetLength(0) - 2, payload.DataChunk.GetLength(1) - 2];
            for (int i = 1; i < payload.DataChunk.GetLength(0) - 1; i++) // Go through rows
            {
                for (int j = 1; j < payload.DataChunk.GetLength(1) - 1; j++) // Go through every value in 1 row (=columns)
                {
                    int neighbourCount = 0;

                    if (payload.DataChunk[i - 1, j] == true) { neighbourCount++; }
                    if (payload.DataChunk[i - 1, j - 1] == true) { neighbourCount++; }
                    if (payload.DataChunk[i, j - 1] == true) { neighbourCount++; }
                    if (payload.DataChunk[i + 1, j] == true) { neighbourCount++; }
                    if (payload.DataChunk[i + 1, j + 1] == true) { neighbourCount++; }
                    if (payload.DataChunk[i, j + 1] == true) { neighbourCount++; }
                    if (payload.DataChunk[i + 1, j - 1] == true) { neighbourCount++; }
                    if (payload.DataChunk[i - 1, j + 1] == true) { neighbourCount++; }
                    if (payload.DataChunk[i, j] == true)
                    {
                        if (neighbourCount < 2) { calculatedDataChunk[i - 1, j - 1] = false; }
                        else if (neighbourCount == 3 || neighbourCount == 2) { calculatedDataChunk[i - 1, j - 1] = true; }
                        else if (neighbourCount > 3) { calculatedDataChunk[i - 1, j - 1] = false; }
                    }
                    else
                    {
                        if (neighbourCount == 3) { calculatedDataChunk[i - 1, j - 1] = true; }
                    }
                }
            }

            payload2.DataChunk = calculatedDataChunk;

            return payload2;
        }
    }
}
