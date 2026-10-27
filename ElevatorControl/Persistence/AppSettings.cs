using System;
using System.IO;
using System.Text.Json;

namespace ElevatorControl.Persistence
{
    public sealed class AppSettings
    {
        public string Server { get; set; } = "localhost";
        public int Port { get; set; } = 3306;
        public string Database { get; set; } = "elevator_db";
        public string User { get; set; } = "root";
        public string Password { get; set; } = "CHANGE_ME";

        public static string SettingsPath => Path.Combine(AppContext.BaseDirectory, "appsettings.json");

        public static AppSettings Load()
        {
            try
            {
                var path = SettingsPath;
                if (!File.Exists(path))
                {
                    return new AppSettings();
                }

                var json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
            }
            catch
            {
                return new AppSettings();
            }
        }

        public string ConnectionString =>
            $"Server={Server};Port={Port};Database={Database};Uid={User};Pwd={Password};";
    }
}
