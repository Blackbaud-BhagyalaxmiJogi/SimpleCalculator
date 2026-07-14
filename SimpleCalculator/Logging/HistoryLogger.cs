using System.IO;

namespace SimpleCalculator.Logging
{
    // Responsible for recording evaluated expressions to a log file on disk,
    // and reading/clearing that history on request.
    public class HistoryLogger
    {
        private readonly string logFilePath;

        public HistoryLogger(string logFilePath)
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

        public void ShowHistory()
        {
            Console.Clear();

            if (!File.Exists(logFilePath))
            {
                Console.WriteLine("History empty.");
                return;
            }

            string[] logEntries = File.ReadAllLines(logFilePath);
            foreach (string entry in logEntries)
            {
                Console.WriteLine(entry);
            }
        }

        public void ClearHistory()
        {
            if (File.Exists(logFilePath))
            {
                File.Delete(logFilePath);
            }

            Console.WriteLine("History wiped cleanly.");
        }
    }
}
