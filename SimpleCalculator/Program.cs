

using Microsoft.Extensions.DependencyInjection;
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
            ServiceProvider serviceProvider = ConfigureServices();

            var app = serviceProvider.GetRequiredService<CalculatorApp>();
            app.Run();
        }

        private static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

        
            services.AddSingleton<Tokenizer>();
            services.AddSingleton<ShuntingYardConverter>();
            services.AddSingleton<PostfixEvaluator>();
            services.AddSingleton<IExpressionCalculator, ExpressionCalculator>();

            
            services.AddSingleton(sp => new HistoryRepository("expression_history.txt"));
            services.AddSingleton<IHistoryWriter>(sp => sp.GetRequiredService<HistoryRepository>());
            services.AddSingleton<IHistoryReader>(sp => sp.GetRequiredService<HistoryRepository>());
            services.AddSingleton<IHistoryManager>(sp => sp.GetRequiredService<HistoryRepository>());

            
            services.AddSingleton<ExpressionEvaluationService>();
            services.AddSingleton<ConsoleExpressionRunner>();
            services.AddSingleton<BatchFileProcessor>();
            services.AddSingleton<HistoryPresenter>();
            services.AddSingleton<MenuController>();
            services.AddSingleton<CalculatorApp>();

            return services.BuildServiceProvider();
        }


    }
}