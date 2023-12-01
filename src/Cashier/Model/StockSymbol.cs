using Cashier.Data;

namespace Cashier.Model
{
    public class StockSymbol
    {
        public StockSymbol() {
            Accounts = [];
        }

        public string? Name { get; set; }
        
        public List<AccountViewModel> Accounts { get; set; }
        
        public StockAnalysis? Analysis { get; set; }
    }
}
