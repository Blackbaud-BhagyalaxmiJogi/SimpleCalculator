namespace SimpleCalculator.UI
{
    // Small helper around Console output so color-printing logic isn't duplicated
    // across every method that needs to show a success or error message.
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
