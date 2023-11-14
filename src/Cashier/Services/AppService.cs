using BlazorDexie.Database;
using BlazorDexie.JsModule;
using Cashier.Components.Pages;
using Cashier.DAL;
using Cashier.Model;
using Microsoft.JSInterop;
using MudBlazor.Charts;

namespace Cashier.Services
{
    /// <summary>
    /// Most of the business logic is currently here.
    /// </summary>
    public class AppService
    {
        private IJSRuntime _jsRuntime;
        private DexieDAL _db;

        public AppService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;

            var moduleFactory = new EsModuleFactory(jsRuntime);
            _db = new DexieDAL(moduleFactory);
        }

        public async Task deleteAccounts()
        {
            await _db.Accounts.Clear();
        }

        public async Task<string?> importBalanceSheet(string[] accountNames)
        {
            if (accountNames.Length == 0)
            {
                throw new ArgumentException("No accounts sent for import!");
            }

            var settings = new SettingsService(_jsRuntime);
            var mainCurrency = await settings.GetDefaultCurrency();
            if (mainCurrency == null)
            {
                throw new ArgumentException("Main currency not set!");
            }

            var accounts = new List<Account>();
            var accountBalances = new List<Money>();

            foreach (var line in accountNames)
            {
                if (string.IsNullOrEmpty(line)) continue;

                Account? account = null;
                // name
                string namePart = string.Empty;
                if (line.Length > 21)
                {
                    namePart = line.Substring(21).Trim();
                    account = new Account(namePart);
                }

                var balancePart = line.Substring(0, 20).Trim();
                // separate the currency
                var balanceParts = balancePart.Split(" ");

                // currency
                string currencyPart = string.Empty;
                if (balanceParts.Length > 1)
                {
                    currencyPart = balanceParts[1];
                }

                var amountPart = balanceParts[0];
                // clean-up the thousand-separators
                amountPart = amountPart.Replace(",", string.Empty);

                var accountBalance = new Money(decimal.Parse(amountPart), currencyPart);
                accountBalances.Add(accountBalance);

                // If we do not have a name, it's an amount of a multicurrency account.
                // Keep the balance until we get the line with the account name.
                if (string.IsNullOrEmpty(namePart)) continue;

                // Once we have the name, assign the balances dictionary and keep for update.
                account!.Balances = accountBalances.ToArray();

                accounts.Add(account);

                // clean-up.
                accountBalances = [];
            }

            var saveResult = await _db.Accounts.BulkPut(accounts);
            return saveResult;
        }
    }
}
