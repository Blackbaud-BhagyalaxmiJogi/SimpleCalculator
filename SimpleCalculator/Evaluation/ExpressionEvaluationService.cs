

using SimpleCalculator.Calculation;
using SimpleCalculator.Logging;

namespace SimpleCalculator.Evaluation
{
    public class ExpressionEvaluationService
    {
        private readonly IExpressionCalculator calculator;
        private readonly IHistoryWriter historyWriter;

        public ExpressionEvaluationService(IExpressionCalculator calculator, IHistoryWriter historyWriter)
        {
            this.calculator = calculator;
            this.historyWriter = historyWriter;
        }

        public EvaluationOutcome TryEvaluateAndLog(string expression)
        {
            string trimmedExpression = expression.Trim();

            try
            {
                double result = calculator.Evaluate(trimmedExpression);
                string entry = $"{trimmedExpression} = {result}";
                historyWriter.Log(entry);
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
