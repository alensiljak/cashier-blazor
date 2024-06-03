﻿using BlazorDexie.Database;
using Cashier.Data;
using Cashier.Model;

namespace Cashier.Services
{
    public class AccountService : IAccountService
    {
        /// <summary>
        /// Gets the account balance in the requested currency. If none exists, the first balance is returned.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="defaultCurrency">The currency in which to retrieve the balance.</param>
        /// <returns></returns>
        public Money GetAccountBalance(Account account, string? defaultCurrency = null)
        {
            // the default value.
            var result = new Money(0, defaultCurrency);

            // Are there any balance records?
            if (account.Balances == null || account.Balances.Length == 0)
            {
                return result;
            }

            if (!string.IsNullOrEmpty(defaultCurrency))
            {
                // Do we have a balance in the default currency?
                var defaultBalance = account.Balances.FirstOrDefault(account => account.Currency == defaultCurrency);
                if (defaultBalance != null)
                {
                    result.Quantity = defaultBalance.Quantity;
                    result.Currency = defaultBalance.Currency;
                    return result;
                }
            }

            // Otherwise take the first balance/currency.
            result = account.Balances?.First();

            return result!;
        }

        public static string GetShortAccountName(string fullAccountName)
        {
            var parts = fullAccountName.Split(':');
            return parts.Last();
        }

        public async Task<List<Account>> LoadInvestmentAccounts(ISettingsService settings, IDexieDAL db)
        {
            var root = await settings.GetRootInvestmentAccount();
            if (root == null)
            {
                throw new Exception("Root investment account not set!");
            }

            var accounts = await db.Accounts
                .Where("name").StartsWithIgnoreCase(root)
                // .Filter
                .ToList();

            // Get only the accounts with a current value.
            accounts = accounts.Where(x => !string.IsNullOrEmpty(x.CurrentValue))
                .ToList();

            return accounts;
        }

        public List<AccountViewModel> ConvertToViewModel(List<Account> accounts)
        {
            return accounts.ConvertAll(acc => new AccountViewModel(acc));
        }
    }
}
