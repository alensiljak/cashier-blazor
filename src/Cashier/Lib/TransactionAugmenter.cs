using Cashier.Components.Pages;
using Cashier.Model;
using System.Linq;

namespace Cashier.Lib
{
    public class TransactionAugmenter
    {
        /// <summary>
        /// Identifies the amount to display, from the user's perspective - a debit, credit, transfer for
        /// a Transaction.
        /// Appends {amount, currency} to the Transaction record.
        /// It is normally useful to run calculateEmptyPostingAmounts() to populate the blank Postings.
        /// </summary>
        /// <returns>An array of balance records that matches the transactions.</returns>
        public List<Money> calculateTxAmounts(List<Xact> xacts)
        {
            calculateEmptyPostingAmounts(xacts);

            var result = new List<Money>();

            foreach (var xact in xacts)
            {
                var balance = new Money();

                // Get the assets and liabilities posting(s) from the transaction.
                var postings = xact.Postings?
                    .Where(p => p.Account != null &&
                    (p.Account.StartsWith("Assets:") || p.Account.StartsWith("Liabilities:") ));
                
                switch(postings.Count())
                {
                    case 0:
                        Console.WriteLine("No postings found in Assets or Liabilities!");
                        break;

                    case 1:
                        // a clear payment case with one source (asset/liability) account.
                        Console.WriteLine("1");
                        break;
                    
                    case 2:
                        Console.WriteLine("2");
                        break;

                    default:
                        Console.WriteLine("default");
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Calculates and adds the amounts for the empty postings. This "completes" the Postings
        /// so that they have an amount and a currency.
        /// </summary>
        public void calculateEmptyPostingAmounts(List<Xact> xacts)
        {
            foreach (var xact in xacts) {
                if (xact.Postings == null || xact.Postings.Count == 0) continue;

                var postings = xact.Postings;


                // do we have multiple currencies? Exclude nulls.
                var currencies = postings
                    .Where(p => p.Money != null)
                    .Select((posting) => posting.Money?.Currency).Distinct();
                if (currencies.Count() > 1) {
                    Console.WriteLine("Multiple currencies fund in a transactions. Ignoring.");
                    DebugPrinter.PrintJson(currencies);
                    continue;
                }

                // use the currency (first?)
                var currency = currencies.First();

                // do we have empty postings?
                var amounts = postings.Select((posting) => posting.Money?.Amount);
                if (amounts.Count() == 0) continue;

                var total = amounts.Sum();

                // put this value into the empty posting.
                var emptyPostings = postings.Where((posting) => posting.Money == null || posting.Money?.Amount == null);
                Console.WriteLine("empty postings: {0}", emptyPostings.Count());

                switch(emptyPostings.Count())
                {
                    case 0:
                        // no empty postings
                        continue;
                    case > 1:
                        var msg = $"Multiple empty postings found on {xact.Payee}";
                        Console.WriteLine(msg);
                        continue;
                }

                // add the values to the (only) empty posting.
                var emptyPosting = emptyPostings.First();
                if (emptyPosting.Money == null)
                {
                    emptyPosting.Money = new Money();
                }
                emptyPosting.Money.Amount = total * (-1);
                if (emptyPosting.Money.Currency == null)
                {
                    emptyPosting.Money.Currency = currency;
                }
            }

            //return xacts;
        }
    }
}
