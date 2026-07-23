

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

            // these services are stateless, so one shared instance per
            // app run is both safe and avoids needless reallocation.
            // Using AddSingleton instead of AddTransient avoids constructing
            // a brand-new Tokenizer/ShuntingYardConverter/PostfixEvaluator (and so
            // on) on every single menu action, which would be wasted allocation
            // for objects that behave identical every time.
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

            services.AddSingleton<EvaluateManuallyCommand>();
            services.AddSingleton<EvaluateFromFileCommand>();
            services.AddSingleton<ShowHistoryCommand>();
            services.AddSingleton<ClearHistoryCommand>();
            services.AddSingleton<ExitCommand>();

            return services.BuildServiceProvider();
        }


    }
}

