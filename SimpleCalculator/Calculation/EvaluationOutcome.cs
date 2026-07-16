

namespace SimpleCalculator.Calculation
{
    public class EvaluationOutcome
    {
        public bool Success { get; }
        public double Result { get; }
        public string Entry { get; }
        public string ErrorMessage { get; }

        private EvaluationOutcome(bool success, double result, string entry, string errorMessage)
        {
            Success = success;
            Result = result;
            Entry = entry;
            ErrorMessage = errorMessage;
        }

        public static EvaluationOutcome Succeeded(double result, string entry) =>
            new EvaluationOutcome(true, result, entry, null);

        public static EvaluationOutcome Failed(string errorMessage) =>
            new EvaluationOutcome(false, 0, null, errorMessage);
    }
}
