

using SimpleCalculator.Evaluation;

namespace SimpleCalculator.Menu
{
    public class EvaluateManuallyCommand : IMenuCommand
    {
        private readonly IExpressionRunner runner;

        public EvaluateManuallyCommand(ConsoleExpressionRunner runner)
        {
            this.runner = runner;
        }

        public bool Execute()
        {
            runner.Run();
            return true;
        }
    }
}
