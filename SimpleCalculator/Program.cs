

using SimpleCalculator.Calculation;
using SimpleCalculator.Logging;
using SimpleCalculator.UI;
using SimpleCalculator.Menu;

namespace SimpleCalculator
{
    class Program
    {

        static void Main(string[] args)
        {
            var app = new CalculatorApp(
                new ExpressionCalculator(),
                new HistoryLogger("expression_history.txt"));

            app.Run();
        }

        
    }
}