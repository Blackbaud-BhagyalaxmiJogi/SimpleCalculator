

using SimpleCalculator.Evaluation;

namespace SimpleCalculator.Menu
{
    public class EvaluateFromFileCommand : IMenuCommand
    {
        private readonly IExpressionRunner runner;

        public EvaluateFromFileCommand(BatchFileProcessor runner)
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
