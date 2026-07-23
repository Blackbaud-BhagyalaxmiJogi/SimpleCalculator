using SimpleCalculator.UI;
using System.IO;

namespace SimpleCalculator.Logging
{
    public class HistoryRepository : IHistoryWriter, IHistoryReader, IHistoryManager
    {
        private readonly string logFilePath;

        public HistoryRepository(string logFilePath)
        {
            this.logFilePath = logFilePath;
        }
        public void Log(string entry)
        {
            string logLine = $"[{DateTime.Now}] {entry}{Environment.NewLine}";

            try
            {
                File.AppendAllText(logFilePath, logLine);
            }
            catch (Exception ex)
            {
                throw new IOException($"Failed to write to history log file '{logFilePath}'.", ex);
            }
        }

        public bool HasHistory() => File.Exists(logFilePath);

        public string[] ReadAll()
        {
            return HasHistory() ? File.ReadAllLines(logFilePath) : Array.Empty<string>();
        }

        public void Clear()
        {
            if (File.Exists(logFilePath))
            {
                File.Delete(logFilePath);
            }
        }
        
    }
}
