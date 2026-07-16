

using SimpleCalculator.Calculation;
using SimpleCalculator.Evaluation;
using SimpleCalculator.Logging;
using SimpleCalculator.Menu;

namespace SimpleCalculator
{
    class Program
    {

        static void Main(string[] args)
        {
            var historyRepository = new HistoryRepository("expression_history.txt");
            var evaluationService = new ExpressionEvaluationService(new ExpressionCalculator(), historyRepository);

            var menuController = new MenuController(
                new ConsoleExpressionRunner(evaluationService),
                new BatchFileProcessor(evaluationService),
                new HistoryPresenter(historyRepository),
                historyRepository);

            var app = new CalculatorApp(menuController);


            app.Run();
        }

        
    }
}