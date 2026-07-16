

using SimpleCalculator.Calculation;
using SimpleCalculator.Logging;

namespace SimpleCalculator
{
    class Program
    {

        static void Main(string[] args)
        {
            var historyRepository = new HistoryRepository("expression_history.txt");
            var app = new CalculatorApp(
                new ExpressionCalculator(), historyRepository, historyRepository,
                new HistoryPresenter(historyRepository));

            app.Run();
        }

        
    }
}