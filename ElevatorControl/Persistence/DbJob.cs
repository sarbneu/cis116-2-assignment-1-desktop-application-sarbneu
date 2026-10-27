using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading;
using MySql.Data.MySqlClient;

namespace ElevatorControl.Persistence
{
    public sealed class DbJob<T>
    {
        private readonly BackgroundWorker _worker = new();
        private readonly Func<T> _work;
        private readonly Action<T>? _callback;

        public DbJob(Func<T> work, Action<T>? callback = null)
        {
            _work = work;
            _callback = callback;
            _worker.DoWork += (_, _) =>
            {
                var result = _work();
                _worker.RunWorkerCompleted += (_, e) => _callback?.Invoke((T)e.Result);
            };
        }

        public void Run()
        {
            _worker.RunWorkerAsync();
        }
    }
}
