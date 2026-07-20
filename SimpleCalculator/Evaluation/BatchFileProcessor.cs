using SimpleCalculator.Calculation;
using SimpleCalculator.UI;

namespace SimpleCalculator.Evaluation
{
    public class BatchFileProcessor : IExpressionRunner
    {
        private readonly ExpressionEvaluationService evaluationService;
        private const int SeparatorWidth = 40;

        public BatchFileProcessor(ExpressionEvaluationService evaluationService)
        {
            this.evaluationService = evaluationService;
        }

        public void Run()
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
            Console.WriteLine($"\nFound {lines.Length} lines. Processing...\n");
            Console.WriteLine(new string('-', SeparatorWidth));

            int lineNumber = 1;
            foreach (string line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    ProcessSingleExpressionLine(line, lineNumber);
                }

                lineNumber++;
            }

            Console.WriteLine(new string('-', SeparatorWidth));
            Console.WriteLine("Batch processing complete.");
        }

        private void ProcessSingleExpressionLine(string line, int lineNumber)
        {
            EvaluationOutcome outcome = evaluationService.TryEvaluateAndLog(line);

            if (outcome.Success)
            {
                ConsoleHelper.WriteSuccess($"[{lineNumber}] SUCCESS: {outcome.Entry}");
            }
            else
            {
                ConsoleHelper.WriteError($"[{lineNumber}] ERROR on expression '{line.Trim()}': {outcome.ErrorMessage}");
            }
        }
    }
}
