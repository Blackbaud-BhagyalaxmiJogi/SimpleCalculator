

using SimpleCalculator.Logging;
using SimpleCalculator.UI;

namespace SimpleCalculator.Menu
{
    public class ClearHistoryCommand : IMenuCommand
    {
        private readonly IHistoryManager historyManager;

        public ClearHistoryCommand(IHistoryManager historyManager)
        {
            this.historyManager = historyManager;
        }

        public bool Execute()
        {
            historyManager.Clear();
            ConsoleHelper.WriteSuccess("History cleared successfully.");
            return true;
        }
    }
}
