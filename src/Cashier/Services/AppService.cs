﻿using BlazorDexie.Database;
using BlazorDexie.JsModule;
using Cashier.Components.Components;
using Cashier.Components.Pages;
using Cashier.Data;
using Cashier.Lib;
using Cashier.Model;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.Charts;
using Newtonsoft.Json;
using System.Dynamic;
using System.Runtime;
using System.Text;

namespace Cashier.Services
{
    /// <summary>
    /// Most of the business logic is currently here.
    /// </summary>
    public class AppService
    {
        public AppService() { }

        public async Task CopyToClipboard(IJSRuntime jsRuntime, string text)
        {
            await jsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", text);
        }

        public async Task deleteAccounts(IDexieDAL db)
        {
            await db.Accounts.Clear();
        }

        /// <summary>
        /// Returns all the register transactions as text,
        /// ready to be exported as a file or copied as a string.
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetExportTransactions(IDexieDAL db)
        {
            var xacts = await db.Xacts.OrderBy("date").ToList();
            var output = new StringBuilder();

            foreach (var xact in xacts)
            {
                output.AppendLine(TranslateToLedger(xact));
            }

            return output.ToString();
        }

        public string GetDateColour(DateOnly date)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);

            switch (date)
            {
                case var n when n < today:
                    return "var(--mud-palette-secondary)";

                case var n when n == today:
                    return "var(--mud-palette-tertiary)";

                case var n when n > today:
                    return "var(--mud-palette-primary)";
            }
            return string.Empty;
        }

        public async Task<string?> ImportBalanceSheet(IDexieDAL db, string[] lines)
        {
            if (lines.Length == 0)
            {
                throw new ArgumentException("No accounts sent for import!");
            }

            var accounts = ParseAccounts(lines);

            var saveResult = await db.Accounts.BulkPut(accounts);
            return saveResult;
        }

        /// <summary>
        /// Imports the payees into storage.
        /// </summary>
        /// <returns></returns>
        public async Task<string> ImportPayees(IDexieDAL db, string[] payeeNames)
        {
            var payees = payeeNames.Select(payee => new Payee { Name = payee });
            var result = await db.Payees.BulkAdd(payees);
            return result;
        }

        /// <summary>
        /// Restores Scheduled Transactions from a JSON string.
        /// </summary>
        /// <param name="serializedList"></param>
        /// <returns></returns>
        public async Task ImportScheduledTransactions(IDexieDAL db, string json)
        {
            List<ScheduledXact>? scheduledXacts;
            scheduledXacts = JsonConvert.DeserializeObject<List<ScheduledXact>>(json);

            if (scheduledXacts is null)
            {
                throw new Exception("No settings found in the file.");
            }

            // Clear existing records
            await db.ScheduledXacts.Clear();

            // save to database
            var lastId = await db.ScheduledXacts.BulkPut(scheduledXacts);
        }

        /// <summary>
        /// Loads favourite accounts.
        /// The account names are stored in the Settings. Then, the Accounts are loaded from the database by name.
        /// </summary>
        /// <param name="take">Take only the first N accounts.</param>
        /// <returns></returns>
        public async Task<List<Account>> LoadFavouriteAccounts(IDexieDAL db, int? take = null)
        {
            var settings = new SettingsService(db);
            var keys = await settings.GetFavouriteAccountNames();

            // Take only top n.
            if (take != null)
            {
                keys = keys.Take(5).ToArray();
            }

            var accounts = await db.Accounts.BulkGet(keys);
            if (accounts == null)
            {
                return [];
            }

            // Handle any accounts that have not been found in the Accounts table.
            var result = new List<Account>();
            foreach (var account in accounts)
            {
                if (account != null)
                {
                    result.Add(account);
                }
            }
            return result.ToList();
        }

        protected List<Account> ParseAccounts(string[] lines)
        {
            var accounts = new List<Account>();
            var accountBalances = new List<Money>();

            foreach (var line in lines)
            {
                //Console.WriteLine("Processing {0}", line);

                // Prepare for parsing
                var source = line.Trim();
                if (string.IsNullOrEmpty(source)) continue;

                var parts = source.Split("  ");

                Account? account = null;
                if (parts.Length > 1)
                {
                    // name
                    var namePart = parts[1];
                    account = new Account(namePart);
                }

                // Balance
                var balancePart = parts[0];
                // separate the currency and the quantity
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
                if (account == null) continue;

                // Once we have the name, assign the balances dictionary and keep for update.
                account!.Balances = accountBalances.ToArray();

                accounts.Add(account);

                // clean-up.
                accountBalances = [];
            }

            return accounts;
        }

        /// <summary>
        /// Returns the next available Xact Id number.
        /// The insert/put is complicated even though the Id is an autoincrement field.
        /// </summary>
        /// <returns></returns>
        public int GetNextXactId()
        {
            //DAL.Xacts.OrderBy("id").;
            // DAL.Xacts.PrimaryKeys()
            return 0;
        }

        /// <summary>
        /// Saves the given transaction as the Last Transaction for the Payee.
        /// This is retrieved when the Payee is selected on a new transaction, or when editing.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> saveLastTransaction(IDexieDAL db, Xact xact)
        {
            var record = new LastXact
            {
                Payee = xact.Payee,
                Xact = xact,
            };

            // Delete unneeded properties - the ids, date, etc.
            record.Xact.Id = null;
            // record.Xact.Date = null;

            await db.LastTransactions.Put(record);
            return true;
        }

        /// <summary>
        /// Translates Transaction into a ledger entry.
        /// </summary>
        /// <param name="xact"></param>
        /// <returns></returns>
        public string TranslateToLedger(Xact xact)
        {
            var output = new StringBuilder();

            output.Append(xact.Date.ToString(Constants.ISODateFormat));
            output.Append(' ');
            output.AppendLine(xact.Payee);

            if (xact.Note != null)
            {
                output.Append("    ; ");
                output.AppendLine(xact.Note);
            }

            // Postings.
            if (xact.Postings != null && xact.Postings.Count > 0)
            {
                foreach (var posting in xact.Postings)
                {
                    output.Append("    ");
                    output.Append(posting.Account);

                    if (posting.Amount != null)
                    {
                        output.Append("  ");
                        output.Append(posting.Amount);
                        output.Append(' ');
                        output.Append(posting.Currency);
                    }
                    output.AppendLine();
                }
            }

            return output.ToString();
        }
    }
}
