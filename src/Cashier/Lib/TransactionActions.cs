using Cashier.Model;

namespace Cashier.Lib
{
    /// <summary>
    /// Operations on Xacts
    /// </summary>
    public class TransactionActions
    {
        public Xact CreateNew()
        {
            var xact = new Xact(DateTime.Today);

            // Add two Postings by default.
            xact.Postings = [new Posting(), new Posting()];

            return xact;
        }
    }
}
