using System.IO;

namespace SimpleCalculator
{
    // Responsible for recording evaluated expressions to a log file on disk,
    // and reading/clearing that history on request.
    static class HistoryLogger
    {
        private const string LogFilePath = "expression_history.txt";

        public static void Log(string entry)
        {
            string logLine = $"[{DateTime.Now}] {entry}{Environment.NewLine}";

            try
            {
                File.AppendAllText(LogFilePath, logLine);
            }
            catch (Exception ex)
            {
                throw new IOException($"Failed to write to history log file '{LogFilePath}'.", ex);
            }
        }

        public static void ShowHistory()
        {
            Console.Clear();

            if (!File.Exists(LogFilePath))
            {
                Console.WriteLine("History empty.");
                return;
            }

            string[] logEntries = File.ReadAllLines(LogFilePath);
            foreach (string entry in logEntries)
            {
                Console.WriteLine(entry);
            }
        }

        public static void ClearHistory()
        {
            if (File.Exists(LogFilePath))
            {
                File.Delete(LogFilePath);
            }

            Console.WriteLine("History wiped cleanly.");
        }
    }
}
