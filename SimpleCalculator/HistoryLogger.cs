using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalculator
{
    static class HistoryLogger
    {
        private const string LogFilePath = "expression_history.txt";

        public static void Log(string entry)
        {
            File.AppendAllText(LogFilePath, $"[{DateTime.Now}] {entry}\n");
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
