using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalculator
{
    static class ConsoleHelper
    {
        public static void WriteSuccess(string message) => WriteColored(message, ConsoleColor.Green);

        public static void WriteError(string message) => WriteColored(message, ConsoleColor.Red);

        private static void WriteColored(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
