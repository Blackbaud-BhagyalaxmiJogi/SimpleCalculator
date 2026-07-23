

using SimpleCalculator.Logging;

namespace SimpleCalculator.Menu
{
    public class ShowHistoryCommand : IMenuCommand
    {
        private readonly HistoryPresenter historyPresenter;
        public ShowHistoryCommand(HistoryPresenter historyPresenter)
        {
            this.historyPresenter = historyPresenter;
        }
        public bool Execute()
        {
            historyPresenter.ShowHistory();
            return true;
        }
    }
}
