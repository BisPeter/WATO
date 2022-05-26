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
        private List<List<bool>> _dataGrid;
        private int _pixelCount;
        private int _workerCount;
        private int _finishedWorkersCount;
        private bool _running;
        private Thread _thread;
        private bool _allWorkersFinished = true;
        private bool[,] _resultPublishedImage;
        private int _bildcounter = 0;
        private object _locker;
        private long _start;

        public Master(int pixelCount, int workerCount, List<List<bool>> dataGrid)
        {
            _locker = new object();
            _dataGrid = dataGrid;
            _pixelCount = pixelCount;
            _workers = CreateWorkers(workerCount);
            _workerCount = workerCount;
            _resultPublishedImage = new bool[pixelCount, pixelCount];
        }


        public void Start()
        {
            if (_running) { throw new InvalidOperationException(); }
            _running = true;
            _thread = new Thread(Worker);
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
            // TODO welche params brauchen die worker?

            return workers;
        }

        private void Worker_WorkerIsDone(object sender, Payload e)
        {
            lock (_locker)
            {
                FillPublishedCreatedImageMethod(e.DataChunk, e.RowNumber);
                _finishedWorkersCount++;
                if (_finishedWorkersCount == _workerCount)
                {
                    List<List<bool>> b = new List<List<bool>>();

                    for (int i = 0; i < _pixelCount; i++)
                    {
                        b.Add(new List<bool>());
                        for (int j = 0; j < _pixelCount; j++)
                        {
                            b.ElementAt(i).Add(_resultPublishedImage[i, j]);

                        }
                    }

                    _dataGrid = b;
                    BitmapCreator.AddImageToList(_resultPublishedImage);
                    
                    _finishedWorkersCount = 0;
                    _allWorkersFinished = true;
                    _bildcounter++;
                    if (_bildcounter % 50 == 0)
                    {
                        Console.WriteLine(TimeSpan.FromTicks(DateTime.Now.Ticks - _start).ToString());

                    }
                }

            }
        }

        private void FillPublishedCreatedImageMethod(bool[,] dataChunk, int rowNumber)
        {
            for (int i = 0; i < dataChunk.GetLength(0); i++)
            {
                for (int j = 0; j < dataChunk.GetLength(1); j++)
                {
                    _resultPublishedImage[rowNumber + i, j] = dataChunk[i, j];
                }
            }
        }

        public void Worker()
        {
            while (_running)
            {
                if (!_allWorkersFinished) { Thread.Sleep(5); continue; }
                _start = DateTime.Now.Ticks;
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
            if ((_pixelCount / _workerCount) % 1 != 0) { throw new Exception(); }
            int length = _pixelCount / _workerCount;
            List<Payload> payLoad = new List<Payload>();

            for (int counter = 0; counter < _workerCount; counter++)
            {
                bool[,] dataChunk = new bool[length + 2, _pixelCount + 2];

                AddToArray(dataChunk, GetRow(counter * length - 1), 0);

                for (int j = 0; j < length; j++)
                {
                    AddToArray(dataChunk, GetRow(j + counter * length), j + 1);
                }

                AddToArray(dataChunk, GetRow(counter * length + length), length + 1);

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
