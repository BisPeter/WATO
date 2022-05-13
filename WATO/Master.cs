using System;
using System.Collections.Generic;
using System.Linq;
using WATO.Interfaces;

namespace WATO
{
    public class Master : IThread
    {
        private List<Worker> _workers;
        private List<List<bool>> _dataGrid;
        private int _pixelCount;
        private int _workerCount;

        public Master(int pixelCount, int workerCount, List<List<bool>> dataGrid)
        {
            _dataGrid = dataGrid;
            _pixelCount = pixelCount;
            _workers = CreateWorkers(workerCount);
            _workerCount = workerCount;
        }

        public int Delay { get; set; }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }



        private List<Worker> CreateWorkers(int workerCount)
        {
            List<Worker> workers = new List<Worker>();
            for (int i = 0; i < workerCount; i++)
            {
                workers.Add(new Worker());
            }
            // TODO welche params brauchen die worker?

            return workers;
        }

        public void Worker()
        {
            List<Payload> tasks = SplitDataGrid();

            throw new NotImplementedException();
        }

        private List<Payload> SplitDataGrid()
        {
            if ((_pixelCount / _workerCount) % 1 != 0) { throw new Exception(); }
            int length = _pixelCount / _workerCount;



            for (int counter = 0; counter < _workerCount; counter++)
            {
                bool[,] dataChunk = new bool[_pixelCount + 2, length + 2];

                AddToArray(dataChunk, GetRow(counter * length - 1), 0);

                for (int j = 0; j < length; j++)
                {
                    AddToArray(dataChunk, GetRow(j + counter * length), j + 1);
                }

                AddToArray(dataChunk, GetRow(counter * length + length), length + 1);



            }
            throw new NotImplementedException();
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
