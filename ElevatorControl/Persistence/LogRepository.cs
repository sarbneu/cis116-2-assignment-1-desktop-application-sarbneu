using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace ElevatorControl.Persistence
{
    public sealed class LogRepository
    {
        private readonly AppSettings _settings;

        public LogRepository(AppSettings settings)
        {
            _settings = settings;
        }

        public DataTable LoadLogs()
        {
            var table = new DataTable();
            table.Columns.Add("id", typeof(int));
            table.Columns.Add("event_time", typeof(DateTime));
            table.Columns.Add("elevator_id", typeof(string));
            table.Columns.Add("event_type", typeof(string));
            table.Columns.Add("message", typeof(string));
            return table;
        }

        public void SaveEntries(IEnumerable<string> entries) { }
    }
}
