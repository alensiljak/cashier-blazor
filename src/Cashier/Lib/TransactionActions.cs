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
            var xact = new Xact(DateOnly.FromDateTime(DateTime.Today))
            {
                // Add two Postings by default.
                Postings = [new Posting(), new Posting()]
            };

            return xact;
        }
    }
}
