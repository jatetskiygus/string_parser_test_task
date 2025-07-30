using System;
using System.IO;

namespace test_task.Utils
{
    public static class Logger
    {
        private static readonly string LogFilePath = "error_log.txt";

        public static void LogError(string message)
        {
            WriteLog("ERROR", message);
        }

        public static void LogInfo(string message)
        {
            WriteLog("INFO", message);
        }

        public static void ClearLog()
        {
            File.WriteAllText(LogFilePath, string.Empty);
        }

        private static void WriteLog(string level, string message)
        {
            var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}";
            File.AppendAllText(LogFilePath, logEntry + Environment.NewLine);
        }
    }
}