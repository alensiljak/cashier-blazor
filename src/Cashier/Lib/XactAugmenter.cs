﻿using Cashier.Components.Pages;
using Cashier.Data;
using Cashier.Model;
using Cashier.Services;
using System.Linq;

namespace Cashier.Lib
{
    public class XactAugmenter
    {
        /// <summary>
        /// Corrects the account balance by adding the local transactions into the calculation.
        /// The initial value is the account's balance from Ledger. Then we calculate the
        /// local transactions and adjust the balance accordingly.
        /// 
        /// The records are modified in-place.
        /// </summary>
        /// <param name="accounts">List of Accounts</param>
        public async Task AddLocalXactsToBalance(IDexieDAL db, List<AccountViewModel> accounts)
        {
            if (accounts.Count == 0)
            {
                Console.WriteLine("No accounts sent for balance adjustment.");
                return;
            }

            var accSvc = new AccountService();
            var settings = new SettingsService(db);
            var defaultCurrency = await settings.GetDefaultCurrency();

            foreach (var account in accounts)
            {
                // handle empty balances
                if (account.AccountBalance is null)
                {
                    account.AccountBalance = accSvc.GetAccountBalance(account, defaultCurrency);
                }

                var sum = account.AccountBalance.Quantity;
                // var xacts = new AppService().lo

                // TODO: complete
            }
        }

        /// <summary>
        /// Identifies the amount to display, from the user's perspective - a debit, credit, transfer for
        /// a Transaction.
        /// It runs calculateEmptyPostingAmounts() beforehand, to populate the blank Postings.
        /// </summary>
        /// <returns>An array of Money records that matches the transactions list.</returns>
        public List<Money> calculateXactAmounts(IEnumerable<Xact?> xacts)
        {
            CalculateEmptyPostingAmounts(xacts);

            var result = new List<Money>();

            foreach (var xact in xacts)
            {
                if (xact == null ) continue;

                var balance = calculateXactAmount(xact);

                // Assemble the output
                result.Add(balance);
            }

            return result;
        }

        public Money calculateXactAmount(Xact xact)
        {
            var balance = new Money();

            // Get the assets and liabilities posting(s) from the transaction.
            var postings = xact.Postings?
                .Where(p => p.Account != null &&
                (p.Account.StartsWith("Assets:") || p.Account.StartsWith("Liabilities:")));
            if (postings == null)
            {
                // continue;
                return balance;
            }

            switch (postings.Count())
            {
                case 0:
                    // No postings found in Assets or Liabilities!
                    balance.Quantity = 0;
                    break;

                case 1:
                    // a clear payment case with one source (asset/liability) account.
                    var posting = postings.First();
                    if (posting.Amount is null)
                    {
                        Console.WriteLine($"Invalid amount encountered! {posting.Account} on {xact.Date} {xact.Payee}");
                        balance.Quantity = 0;
                    }
                    else
                    {
                        balance.Quantity = posting.Amount;
                        balance.Currency = posting.Currency;
                    }
                    break;

                case 2:
                    var firstPosting = postings.First();
                    if (firstPosting.Amount is null)
                    {
                        //continue;
                        return balance;
                    }

                    // involves a transfer
                    if (firstPosting.Amount != null)
                    {
                        balance.Quantity = Math.Abs(firstPosting.Amount.Value);
                    }
                    if (firstPosting.Currency != null)
                    {
                        balance.Currency = firstPosting.Currency;
                    }

                    // Treat the liability account as an expense.
                    var assetPostings = postings.Where(p => p.Account!.StartsWith("Assets:"));
                    if (assetPostings.Count() > 0 &&
                        postings.Count(p => p.Account!.StartsWith("Liabilities:")) > 0)
                    {
                        // Take the sign from the Asset posting
                        balance.Quantity = assetPostings.First()!.Amount;
                    }
                    break;

                default:
                    Console.WriteLine("More than one posting found in Assets!");
                    break;
            }

            return balance;
        }

        /// <summary>
        /// Calculates and adds the amounts for the empty postings. This "completes" the Postings
        /// so that they have an amount and a currency.
        /// </summary>
        public static void CalculateEmptyPostingAmounts(IEnumerable<Xact?> xacts)
        {
            foreach (var xact in xacts)
            {
                if (xact == null || xact.Postings == null || xact.Postings.Count == 0) continue;

                CalculateEmptyPostingAmount(xact);
            }

            //return xacts;
        }

        /// <summary>
        /// Calculates the amounts for any postings without them. For one transaction.
        /// </summary>
        /// <param name="xact"></param>
        public static void CalculateEmptyPostingAmount(Xact xact)
        {
            var postings = xact.Postings;
            if (postings == null)
            {
                // nothing to do here.
                return;
            }

            // do we have multiple currencies? Exclude nulls.
            var currencies = postings
                .Where(p => !string.IsNullOrWhiteSpace(p.Currency))
                .Select((posting) => posting.Currency)
                .Distinct();

            if (currencies.Count() > 1)
            {
                Console.WriteLine("Multiple currencies fund in a transactions. Ignoring.");
                DebugPrinter.PrintJson(currencies);
                // continue;
                return;
            }

            // use the currency (first?)
            var currency = currencies.FirstOrDefault();

            // do we have empty postings?
            var amounts = postings.Select((posting) => posting.Amount);
            if (amounts.Count() == 0)
            {
                // continue;
                return;
            }

            var total = amounts.Sum();

            // put this value into the empty posting.
            var emptyPostings = postings.Where((posting) => posting.Amount == null);

            switch (emptyPostings.Count())
            {
                case 0:
                    // no empty postings
                    //continue;
                    return;
                case > 1:
                    var msg = $"Multiple empty postings found on {xact.Payee}";
                    Console.WriteLine(msg);
                    // continue;
                    return;
            }

            // add the values to the (only) empty posting.
            var emptyPosting = emptyPostings.First();
            emptyPosting.Amount = total * (-1);
            if (string.IsNullOrWhiteSpace(emptyPosting.Currency))
            {
                emptyPosting.Currency = currency;
            }
        }
    }
}
