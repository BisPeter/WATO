//----------------------------------------------------------------------
// <copyright file=Master.cs company="FHWN.ac.at">
// Copyright (c) FHWN. All rights reserved.
// </copyright>
// <summary>This Project represents a WATOR simulation, implemented with a Master-Worker Pattern</summary>
// <author>Matthias Mandl & Peter Vadle</author>
// -----------------------------------------------------------------------

namespace WATO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using WATO.Interfaces;

    /// <summary>
    /// Represents the master of the Master - Worker pattern.
    /// </summary>
    public class Master : IThread
    {
        /// <summary>
        /// The workers.
        /// </summary>
        private List<Worker> _workers;

        /// <summary>
        /// The data grid, where to save the results of the worker
        /// When all worker are finished, this is the current picture.
        /// </summary>
        private List<List<bool>> _dataGrid;// List of rows

        /// <summary>
        /// The worker count.
        /// </summary>
        private readonly int _workerCount;

        /// <summary>
        /// The finished workers count.
        /// </summary>
        private int _finishedWorkersCount;

        /// <summary>
        /// If the thread isrunning.
        /// </summary>
        private bool _running;

        /// <summary>
        /// The thread.
        /// </summary>
        private Thread _thread;

        /// <summary>
        /// If all workers are finished.
        /// </summary>
        private bool _allWorkersFinished = true;

        /// <summary>
        /// The counter of calculated pictures.
        /// </summary>
        private int _pictureCounter = 0;

        /// <summary>
        /// The locker.
        /// </summary>
        private readonly object _locker;

        /// <summary>
        /// The start ticks.
        /// </summary>
        private long _start;

        /// <summary>
        /// The tracer.
        /// </summary>
        private readonly Tracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Master"/> class.
        /// </summary>
        /// <param name="workerCount">The worker count.</param>
        /// <param name="dataGrid">The initial data grid.</param>
        public Master(int workerCount, List<List<bool>> dataGrid)
        {
            _locker = new object();
            _dataGrid = dataGrid;
            _workers = CreateWorkers(workerCount);
            _workerCount = workerCount;
            _tracer = new Tracer();
        }

        /// <summary>
        /// Starts the worker and thread.
        /// </summary>
        public void Start()
        {
            if (_running) { throw new InvalidOperationException(); }
            _start = DateTime.Now.Ticks;
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
        /// Creates the workers.
        /// </summary>
        /// <param name="workerCount">The worker count.</param>
        /// <returns>The list of worker.</returns>
        private List<Worker> CreateWorkers(int workerCount)
        {
            List<Worker> workers = new List<Worker>();
            for (int i = 0; i < workerCount; i++)
            {
                Worker worker = new Worker();
                worker.FireOnWorkerIsDone += Worker_WorkerIsDone;
                worker.Start();
                workers.Add(worker);
            }

            return workers;
        }

        /// <summary>
        /// Worker is done.
        /// If all are done, save the bitmap.
        /// </summary>
        /// <param name="sender">The worker.</param>
        /// <param name="e">The payload of the worker.</param>
        private void Worker_WorkerIsDone(object sender, Payload e)
        {
            lock (_locker)
            {
                FillPublishedCreatedImage(e.DataChunk, e.RowNumber);
                _finishedWorkersCount++;
                if (_finishedWorkersCount == _workerCount)
                {
                    _pictureCounter++;

                    if (_pictureCounter % BitmapCreator.PicturesTillSave == 0)
                    {
                        _tracer.TraceMessage("At [" + DateTime.Now.ToString("hh:mm:ss.fff") + "] Timespan for " + BitmapCreator.PicturesTillSave + " Rounds: " + TimeSpan.FromTicks(DateTime.Now.Ticks - _start).ToString() + " with " + _workerCount + " workers with " + _dataGrid.Count + " rows and " + _dataGrid.ElementAt(0).Count() + " columns");
                        //Console.WriteLine(TimeSpan.FromTicks(DateTime.Now.Ticks - _start).ToString()); // Console option
                        _start = DateTime.Now.Ticks;
                    }

                    BitmapCreator.AddImageToList(_dataGrid, _workerCount);

                    _finishedWorkersCount = 0;
                    _allWorkersFinished = true;
                }
            }
        }

        /// <summary>
        /// Fills the _dataGrid with the incomming values from the worker.
        /// </summary>
        /// <param name="dataChunk">The data chunk.</param>
        /// <param name="rowNumber">The row number where to insert.</param>
        private void FillPublishedCreatedImage(bool[,] dataChunk, int rowNumber)
        {
            for (int i = 0; i < dataChunk.GetLength(0); i++)
            {
                for (int j = 0; j < dataChunk.GetLength(1); j++)
                {
                    // replace our datagrid at position with datachunk value from worker
                    _dataGrid[rowNumber + i][j] = dataChunk[i, j];
                }
            }
        }

        /// <summary>
        /// Worker of this thread.
        /// </summary>
        public void Work()
        {
            while (_running)
            {
                if (!_allWorkersFinished) { Thread.Sleep(5); continue; }
                List<Payload> tasks = SplitDataGrid();
                DistributeWork(tasks);
                _allWorkersFinished = false;
            }
        }

        /// <summary>
        /// Distributes the payloads to the workers.
        /// </summary>
        /// <param name="tasks">The payloads.</param>
        private void DistributeWork(List<Payload> tasks)
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                _workers[i].AcceptPayload(tasks[i]);
            }
        }

        /// <summary>
        /// Splits the data grid.
        /// </summary>
        /// <returns>The payloads.</returns>
        private List<Payload> SplitDataGrid()
        {
            if ((_dataGrid.Count / _workerCount) % 1 != 0) { throw new Exception(); }
            int length = _dataGrid.Count / _workerCount;
            List<Payload> payLoad = new List<Payload>();

            for (int counter = 0; counter < _workerCount; counter++)
            {
                bool[,] dataChunk = new bool[length + 2, _dataGrid.Count + 2];

                AddToArray(dataChunk, GetRow(counter * length - 1), 0); // Getting upper Ghost Boundary 

                for (int j = 0; j < length; j++)
                {
                    AddToArray(dataChunk, GetRow(j + counter * length), j + 1); // Getting actual rows
                }

                AddToArray(dataChunk, GetRow(counter * length + length), length + 1); // Getting lower Ghost Boundary

                payLoad.Add(new Payload { DataChunk = dataChunk, RowNumber = counter * length });
            }

            return payLoad;
        }

        /// <summary>
        /// Adds a row to the 2d array.
        /// </summary>
        /// <param name="dataArray">The 2d data array.</param>
        /// <param name="row">The row.</param>
        /// <param name="rowNumber">The row number.</param>
        private void AddToArray(bool[,] dataArray, bool[] row, int rowNumber)
        {
            for (int i = 0; i < row.Length; i++)
            {
                dataArray[rowNumber, i] = row[i];
            }
        }

        /// <summary>
        /// Gets the row.
        /// </summary>
        /// <param name="row">The index of the row.</param>
        /// <returns>The row of the data grid.</returns>
        private bool[] GetRow(int row)
        {
            List<bool> ret = new List<bool>();
            int rowNumber = row;
            if (row < 0) { rowNumber = _dataGrid.Count - 1; }
            if (row > _dataGrid.Count - 1) { rowNumber = 0; }
            ret.Add(_dataGrid.ElementAt(rowNumber).Last());
            ret.AddRange(_dataGrid.ElementAt(rowNumber));
            ret.Add(_dataGrid.ElementAt(rowNumber).FirstOrDefault());
            return ret.ToArray();
        }
    }
}
