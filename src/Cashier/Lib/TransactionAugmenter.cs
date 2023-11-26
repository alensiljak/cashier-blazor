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
                if (postings == null) continue;
                
                switch(postings.Count())
                {
                    case 0:
                        Console.WriteLine("No postings found in Assets or Liabilities!");
                        break;

                    case 1:
                        // a clear payment case with one source (asset/liability) account.
                        var posting = postings.First();
                        if (posting.Amount is null)
                        {
                            Console.WriteLine("Invalid amount encountered!");
                            continue;
                        }

                        balance.Quantity = posting.Amount.Quantity;
                        balance.Currency = posting.Amount.Currency;
                        break;
                    
                    case 2:
                        var firstPosting = postings.First();

                        // involves a transfer
                        balance.Quantity = Math.Abs(firstPosting.Amount!.Quantity!.Value);
                        balance.Currency = firstPosting.Amount.Currency;

                        // Treat the liability account as an expense.
                        var assetPostings = postings.Where(p => p.Account!.StartsWith("Assets:"));
                        if (assetPostings.Count() > 0 &&
                            postings.Count(p => p.Account!.StartsWith("Liabilities:")) > 0)
                        {
                            // Take the sign from the Asset posting
                            balance.Quantity = assetPostings.First()!.Amount!.Quantity;
                        }
                        break;

                    default:
                        Console.WriteLine("More than one posting found in Assets!");
                        break;
                }
                // Assemble the output
                result.Add(balance);
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
                    .Where(p => p.Amount != null)
                    .Select((posting) => posting.Amount?.Currency).Distinct();
                if (currencies.Count() > 1) {
                    Console.WriteLine("Multiple currencies fund in a transactions. Ignoring.");
                    DebugPrinter.PrintJson(currencies);
                    continue;
                }

                // use the currency (first?)
                var currency = currencies.First();

                // do we have empty postings?
                var amounts = postings.Select((posting) => posting.Amount?.Quantity);
                if (amounts.Count() == 0) continue;

                var total = amounts.Sum();

                // put this value into the empty posting.
                var emptyPostings = postings.Where((posting) => posting.Amount == null || posting.Amount?.Quantity == null);
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
                if (emptyPosting.Amount == null)
                {
                    emptyPosting.Amount = new Money();
                }
                emptyPosting.Amount.Quantity = total * (-1);
                if (emptyPosting.Amount.Currency == null)
                {
                    emptyPosting.Amount.Currency = currency;
                }
            }

            //return xacts;
        }
    }
}
