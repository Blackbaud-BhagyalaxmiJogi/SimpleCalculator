

using SimpleCalculator.Logging;

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
            return true;
        }
    }
}
