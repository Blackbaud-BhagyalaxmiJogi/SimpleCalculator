
namespace SimpleCalculator.Logging
{
    public class HistoryPresenter
    {
        private readonly IHistoryReader historyReader;

        public HistoryPresenter(IHistoryReader historyReader)
        {
            this.historyReader = historyReader;
        }

        public void ShowHistory()
        {
            Console.Clear();
            if (!historyReader.HasHistory())
            {
                Console.WriteLine("History is empty.");
                return;

            }
            foreach(String entry in historyReader.ReadAll())
            {
                Console.WriteLine(entry);
            }
        }
    }
}
