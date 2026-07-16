using SimpleCalculator.Calculation;
using SimpleCalculator.Logging;
using SimpleCalculator.Menu;
using SimpleCalculator.UI;

namespace SimpleCalculator
{
    internal class CalculatorApp
    {
        private readonly MenuController menuController;

        public CalculatorApp(MenuController menuController)
        {
            this.menuController = menuController;
        }

        public void Run()
        {
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("            Welcome to the Calculator             ");
            Console.WriteLine("--------------------------------------------------");

            menuController.RunMenuLoop();

            Console.WriteLine("Goodbye!");
        }

    }
}
