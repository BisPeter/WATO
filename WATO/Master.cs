using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using WATO.Interfaces;

namespace WATO
{
    public class Master : IThread
    {
        private List<Worker> _workers;
        private List<List<bool>> _dataGrid; // List of rows
        private int _workerCount;
        private int _finishedWorkersCount;
        private bool _running;
        private Thread _thread;
        private bool _allWorkersFinished = true;
        private int _bildcounter = 0;
        private object _locker;
        private long _start;
        private readonly Tracer _tracer;

        public Master(int workerCount, List<List<bool>> dataGrid)
        {
            _locker = new object();
            _dataGrid = dataGrid;
            _workers = CreateWorkers(workerCount);
            _workerCount = workerCount;
            _tracer = new Tracer();
        }


        public void Start()
        {
            if (_running) { throw new InvalidOperationException(); }
            _start = DateTime.Now.Ticks;
            _running = true;
            _thread = new Thread(Work);
            _thread.Start();
        }

        public void Stop()
        {
            if (!_running) { throw new InvalidOperationException(); }
            if (_thread != Thread.CurrentThread)
            {
                _thread.Join();
                _running = false;
            }
        }

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

        private void Worker_WorkerIsDone(object sender, Payload e)
        {
            lock (_locker)
            {
                FillPublishedCreatedImage(e.DataChunk, e.RowNumber);
                _finishedWorkersCount++;
                if (_finishedWorkersCount == _workerCount)
                {
                    _bildcounter++;

                    if (_bildcounter % BitmapCreator.PicturesTillSave == 0)
                    {
                        _tracer.TraceMessage("At [" + DateTime.Now.ToString("hh:mm:ss.fff")+ "] Timespan for " + BitmapCreator.PicturesTillSave + " Rounds: " + TimeSpan.FromTicks(DateTime.Now.Ticks - _start).ToString() + " with " + _workerCount + " workers with " + _dataGrid.Count + " rows and " + _dataGrid.ElementAt(0).Count() + " columns");
                        //Console.WriteLine(TimeSpan.FromTicks(DateTime.Now.Ticks - _start).ToString()); // Console option
                        _start = DateTime.Now.Ticks;
                    }

                    BitmapCreator.AddImageToList(_dataGrid);
                    
                    _finishedWorkersCount = 0;
                    _allWorkersFinished = true;
                }
            }
        }

        private void FillPublishedCreatedImage(bool[,] dataChunk, int rowNumber)
        {
            for (int i = 0; i < dataChunk.GetLength(0); i++)
            {
                for (int j = 0; j < dataChunk.GetLength(1); j++)
                {
                    // replace our datagrid at position with datachunk value from worker
                    _dataGrid[rowNumber+i][j] = dataChunk[i, j];
                }
            }
        }

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

        private void DistributeWork(List<Payload> tasks)
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                _workers[i].AcceptPayload(tasks[i]);
            }
        }

        private List<Payload> SplitDataGrid()
        {
            if (( _dataGrid.Count / _workerCount) % 1 != 0) { throw new Exception(); }
            int length =  _dataGrid.Count / _workerCount;
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

        private void AddToArray(bool[,] dataArray, bool[] row, int rowNumber)
        {
            for (int i = 0; i < row.Length; i++)
            {
                dataArray[rowNumber, i] = row[i];
            }
        }

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
