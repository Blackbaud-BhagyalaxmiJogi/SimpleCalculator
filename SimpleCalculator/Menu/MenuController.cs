

using SimpleCalculator.Evaluation;
using SimpleCalculator.Logging;
using SimpleCalculator.UI;

namespace SimpleCalculator.Menu
{
    public class MenuController
    {
        private readonly ConsoleExpressionRunner consoleRunner;
        private readonly BatchFileProcessor batchProcessor;
        private readonly HistoryPresenter historyPresenter;
        private readonly IHistoryManager historyManager;

        public MenuController(ConsoleExpressionRunner consoleRunner, BatchFileProcessor batchProcessor,
                               HistoryPresenter historyPresenter, IHistoryManager historyManager)
        {
            this.consoleRunner = consoleRunner;
            this.batchProcessor = batchProcessor;
            this.historyPresenter = historyPresenter;
            this.historyManager = historyManager;
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
            switch (choice)
            {
                case MenuOptions.MenuEvaluateManually:
                    consoleRunner.Run();
                    return true;
                case MenuOptions.MenuEvaluateFromFile:
                    batchProcessor.Run();
                    return true;
                case MenuOptions.MenuShowHistory:
                    historyPresenter.ShowHistory();
                    return true;
                case MenuOptions.MenuClearHistory:
                    historyManager.Clear();
                    return true;
                case MenuOptions.MenuExit:
                    return false;
                default:
                    ConsoleHelper.WriteError("Invalid selection.");
                    return true;
            }
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
