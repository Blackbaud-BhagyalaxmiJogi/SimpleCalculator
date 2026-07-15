using SimpleCalculator.Calculation;
using SimpleCalculator.Logging;
using SimpleCalculator.Menu;
using SimpleCalculator.UI;

namespace SimpleCalculator
{
    internal class CalculatorApp
    {
        private readonly ExpressionCalculator calculator;
        private readonly HistoryLogger historyLogger;

        public CalculatorApp(ExpressionCalculator calculator, HistoryLogger historyLogger)
        {
            this.calculator = calculator;
            this.historyLogger = historyLogger;
        }

        public void Run()
        {
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("            Welcome to the Calculator             ");
            Console.WriteLine("--------------------------------------------------");

            RunMenuLoop();

            Console.WriteLine("Goodbye!");
        }

        private void RunMenuLoop()
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
                    EvaluateExpressionFromConsole();
                    return true;
                case MenuOptions.MenuEvaluateFromFile:
                    EvaluateExpressionsFromFile();
                    return true;
                case MenuOptions.MenuShowHistory:
                    historyLogger.ShowHistory();
                    return true;
                case MenuOptions.MenuClearHistory:
                    historyLogger.ClearHistory();
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


        private void EvaluateExpressionFromConsole()
        {
            Console.Clear();
            Console.WriteLine("Enter your full expression (e.g., 6+5*(4-2.6)/80%3^2):");
            Console.Write("> ");
            string input = Console.ReadLine();

            EvaluationOutcome outcome = TryEvaluateAndLog(input);

            if (outcome.Success)
            {
                ConsoleHelper.WriteSuccess($"\nResult: {outcome.Result}");
            }
            else
            {
                ConsoleHelper.WriteError($"Syntax/Math Error: {outcome.ErrorMessage}");
            }
        }


        private void EvaluateExpressionsFromFile()
        {
            Console.Clear();
            Console.WriteLine("Process Expressions From File");
            Console.Write("Enter the path to your .txt file (or filename if in the same folder): ");
            string inputFilePath = Console.ReadLine();

            if (!File.Exists(inputFilePath))
            {
                ConsoleHelper.WriteError("Error: File not found. Make sure the filename/path is correct.");
                return;
            }

            try
            {
                string[] lines = File.ReadAllLines(inputFilePath);
                ProcessExpressionLines(lines);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An I/O error occurred while reading the file: {ex.Message}");
            }
        }

        private void ProcessExpressionLines(string[] lines)
        {
            int separatedWidth = 40;
            Console.WriteLine($"\nFound {lines.Length} lines. Processing...\n");
            Console.WriteLine(new String('-', separatedWidth));

            int lineNumber = 1;
            foreach (string line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    ProcessSingleExpressionLine(line, lineNumber);
                }

                lineNumber++;
            }

            Console.WriteLine(new String('-', separatedWidth));
            Console.WriteLine("Batch processing complete.");
        }

        private void ProcessSingleExpressionLine(string line, int lineNumber)
        {
            EvaluationOutcome outcome = TryEvaluateAndLog(line);

            if (outcome.Success)
            {
                ConsoleHelper.WriteSuccess($"[{lineNumber}] SUCCESS: {outcome.Entry}");
            }
            else
            {
                ConsoleHelper.WriteError($"[{lineNumber}] ERROR on expression '{line.Trim()}': {outcome.ErrorMessage}");
            }
        }



        private EvaluationOutcome TryEvaluateAndLog(string expression)
        {
            string trimmedExpression = expression.Trim();

            try
            {
                double result = calculator.Evaluate(trimmedExpression);
                string entry = $"{trimmedExpression} = {result}";
                historyLogger.Log(entry);
                return EvaluationOutcome.Succeeded(result, entry);
            }
            catch (FormatException ex)
            {
                // bad syntax: mismatched parentheses, malformed structure, etc.
                return EvaluationOutcome.Failed(ex.Message);
            }
            catch (DivideByZeroException ex)
            {
                return EvaluationOutcome.Failed(ex.Message);
            }
            catch (IOException ex)
            {
                // Thrown by HistoryLogger if the log file can't be written to.
                return EvaluationOutcome.Failed($"Logging error: {ex.Message}");
            }
        }


    }
}
