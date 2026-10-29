using System;
using System.IO;

namespace AlcoBarrier
{
    /// <summary>
    /// ДОБАВЛЕНО (этап C2): простой файловый логгер.
    /// Пишет в logs/alco-yyyy-MM-dd.log рядом с exe, один файл на день.
    /// Потокобезопасен (lock), при ошибке записи молчит — логирование
    /// не должно ломать приложение.
    /// </summary>
    public static class Logger
    {
        private static readonly object _lock = new object();
        private static readonly string _logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "logs");

        public static void Info(string message) => Write("INFO", message, null);
        public static void Warning(string message) => Write("WARN", message, null);
        public static void Error(string message, Exception ex = null) => Write("ERROR", message, ex);

        private static void Write(string level, string message, Exception ex)
        {
            try
            {
                lock (_lock)
                {
                    if (!Directory.Exists(_logDirectory))
                        Directory.CreateDirectory(_logDirectory);

                    string logFile = Path.Combine(_logDirectory, $"alco-{DateTime.Now:yyyy-MM-dd}.log");
                    string line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{level}] {message}";
                    if (ex != null)
                        line += Environment.NewLine + ex;
                    line += Environment.NewLine;

                    File.AppendAllText(logFile, line);
                }
            }
            catch
            {
                // Молча: логгер не должен бросать исключения.
            }
        }
    }
}