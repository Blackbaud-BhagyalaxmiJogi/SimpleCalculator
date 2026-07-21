

using SimpleCalculator.Evaluation;
using SimpleCalculator.Logging;
using SimpleCalculator.UI;

namespace SimpleCalculator.Menu
{
    public class MenuController
    {
        private readonly Dictionary<string, IMenuCommand> commands;
        public MenuController(EvaluateManuallyCommand evaluateManually, EvaluateFromFileCommand evaluateFromFile, ShowHistoryCommand showHistory, ClearHistoryCommand clearHistory, ExitCommand exit)
        {
            commands = new Dictionary<string, IMenuCommand>
            {
                {MenuOptions.MenuEvaluateManually, evaluateManually},
                {MenuOptions.MenuEvaluateFromFile, evaluateFromFile},
                {MenuOptions.MenuShowHistory, showHistory},
                {MenuOptions.MenuClearHistory, clearHistory},
                {MenuOptions.MenuExit, exit}
            };
        }

        public void RunMenuLoop()
        {
            bool isRunning = true;

            while (isRunning)
            {
                DisplayMenu();
                string choice = Console.ReadLine();
                isRunning = HandleMenuChoice(choice);
            }
        }

        private bool HandleMenuChoice(string choice)
        {
            if (commands.TryGetValue(choice, out IMenuCommand command))
            {
                return command.Execute();
            }

            ConsoleHelper.WriteError("Invalid selection.");
            return true;


        }

        private void DisplayMenu()
        {
            Console.WriteLine("\nMAIN MENU:");
            Console.WriteLine("1. Enter an Expression Manually");
            Console.WriteLine("2. Process Expressions from a File (.txt)");
            Console.WriteLine("3. View Evaluation History File");
            Console.WriteLine("4. Clear History");
            Console.WriteLine("5. Exit");
            Console.Write("Choice: ");
        }
    }
}
