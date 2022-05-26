using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WATO.Interfaces;

namespace WATO
{
    public class Worker
    {
        private Payload _payload;
        public event EventHandler<Payload> FireOnWorkerIsDone;
        private Thread _thread;
        private bool _running;
        private bool _working;

        public void Start()
        {
            if (_running) { throw new InvalidOperationException(); }
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

        public bool AcceptPayload(Payload payload)
        {
            if (_working) { throw new Exception(); }// return false; }
            _payload = payload;
            _working = true;

            return true;
        }

        void Work()
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

        private Payload CalculatePayload(Payload payload)
        {
            Payload payload2 = new Payload { RowNumber = payload.RowNumber };
            bool[,] calculatedDataChunk = new bool[payload.DataChunk.GetLength(0) - 2, payload.DataChunk.GetLength(1) - 2];
            for (int i = 1; i < payload.DataChunk.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < payload.DataChunk.GetLength(1) - 1; j++)
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
                        else if(neighbourCount > 3) { calculatedDataChunk[i - 1, j - 1] = false; }
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
