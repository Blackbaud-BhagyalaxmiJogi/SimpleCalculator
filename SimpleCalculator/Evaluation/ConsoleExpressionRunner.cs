using SimpleCalculator.Calculation;
using SimpleCalculator.UI;

namespace SimpleCalculator.Evaluation
{
    public class ConsoleExpressionRunner
    {
        private readonly ExpressionEvaluationService evaluationService;

        public ConsoleExpressionRunner(ExpressionEvaluationService evaluationService)
        {
            this.evaluationService = evaluationService;
        }

        public void Run()
        {
            Console.Clear();
            Console.WriteLine("Enter your full expression (e.g., 6+5*(4-2.6)/80%3^2):");
            Console.Write("> ");
            string input = Console.ReadLine();

            EvaluationOutcome outcome = evaluationService.TryEvaluateAndLog(input);

            if (outcome.Success)
            {
                ConsoleHelper.WriteSuccess($"\nResult: {outcome.Result}");
            }
            else
            {
                ConsoleHelper.WriteError($"Syntax/Math Error: {outcome.ErrorMessage}");
            }
        }
    }
}
