using BlazorDexie.Database;
using BlazorDexie.JsModule;
using Cashier.Components;
using Cashier.Components.Components;
using Cashier.Components.Pages;
using Cashier.Data;
using Cashier.Data.Entities;
using Cashier.Lib;
using Cashier.Model;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.Charts;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Drawing;
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

        // static methods

        public static async Task<DialogResult> ShowConfirmationDialog(IDialogService svc, string text, string title = "",
            MudBlazor.Color? confirmationButtonColor = null, MaxWidth? maxWidth = null)
        {
            var parameters = new DialogParameters<ConfirmationDialog>
            {
                { x => x.ContentText, text }
            };
            if (confirmationButtonColor != null)
            {
                parameters.Add(x => x.ConfirmationButtonColor, confirmationButtonColor.Value);
            }

            if (maxWidth is null)
            {
                maxWidth = MaxWidth.Medium;
            }

            var options = new DialogOptions { MaxWidth = maxWidth };

            var dialog = await svc.ShowAsync<ConfirmationDialog>(title, parameters, options);

            var result = await dialog.Result;
            return result;
        }

        public static async Task<DialogResult> ShowTextInputDialog(IDialogService svc, string text, string title = "",
            MudBlazor.Color? confirmationButtonColor = null, MaxWidth? maxWidth = null)
        {
            var parameters = new DialogParameters<TextInputDialog>
            {
                { x => x.ContentText, text }
            };
            if (confirmationButtonColor != null)
            {
                parameters.Add(x => x.ConfirmationButtonColor, confirmationButtonColor.Value);
            }

            if (maxWidth is null)
            {
                maxWidth = MaxWidth.Medium;
            }

            var options = new DialogOptions { MaxWidth = maxWidth };

            var dialog = await svc.ShowAsync<TextInputDialog>(title, parameters, options);

            var result = await dialog.Result;
            return result;
        }

        // instance methods

        public async Task CopyToClipboard(IJSRuntime jsRuntime, string text)
        {
            await jsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", text);
        }

        public static Xact CreateNewXact()
        {
            var xact = new Xact(DateUtils.Today)
            {
                // Add two Postings by default.
                Postings = [new Posting(), new Posting()]
            };

            return xact;
        }

        /// <summary>
        /// Creates a clone of an existing Xact, without the Id.
        /// </summary>
        /// <param name="existing"></param>
        /// <returns></returns>
        public Xact CreateNewXactFrom(Xact existing)
        {
            var newXact = new Xact(DateUtils.Today)
            {
                Date = existing.Date,
                Payee = existing.Payee,
                Note = existing.Note,
            };
            if (existing.Postings != null)
            {
                newXact.Postings = existing.Postings.ConvertAll(p => p.Clone());
            }

            return newXact;
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

        public long GetNewId()
        {
            return DateTime.UtcNow.Ticks;
        }

        /// <summary>
        /// Xacts are exported in Ledger format.
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static async Task<string?> GetXactsForExport(IDexieDAL db)
        {
            var records = await db.Xacts.OrderBy(nameof(Xact.Date)).ToList();

            //var output = Serialize(records);
            var sb = new StringBuilder();

            foreach(var record in records)
            {
                sb.AppendLine(AppService.TranslateToLedger(record));
            }

            return sb.ToString();
        }

        public static async Task<string?> GetScheduledXactsForExport(IDexieDAL db)
        {
            var records = await db.ScheduledXacts.ToList();

            var output = Serialize(records);
            return output;
        }

        public static async Task<string?> GetSettingsForExport(IDexieDAL db)
        {
            var records = await db.Settings.ToList();

            var output = Serialize(records);
            return output;
        }

        public async Task<string?> ImportBalanceSheet(IDexieDAL db, List<string> lines)
        {
            if (lines.Count == 0)
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
        public async Task<string> ImportPayees(IDexieDAL db, List<string> payeeNames)
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
                keys = keys.Take(5).ToList();
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

        public async Task<ScheduledXact> LoadScheduledXact(IDexieDAL db, long sxId, AppState state)
        {
            var sx = await db.ScheduledXacts.Get(sxId);

            if (sx is null)
            {
                throw new Exception("Scheduled Transaction not found!");
            }

            state.ScheduledXact = sx;
            state.Xact = sx.Transaction;

            return sx;
        }

        protected List<Account> ParseAccounts(List<string> lines)
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

        public async Task<long> SaveScheduledXact(IDexieDAL db, ScheduledXact stx)
        {
            if (stx == null)
            {
                throw new Exception("The record is null");
            }

            if (stx.Id is null)
            {
                stx.Id = GetNewId();
            }

            // clear any transaction ids!
            if (stx.Transaction != null && stx.Transaction.Id.HasValue)
            {
                stx.Transaction.Id = null;
            }

            var result = await db.ScheduledXacts.Put(stx);
            return result;
        }

        public async Task<long> SaveXact(IDexieDAL db, Xact xact)
        {
            if (xact.Id == null)
            {
                xact.Id = this.GetNewId();
            }

            var result = await db.Xacts.Put(xact);
            return result;
        }

        /// <summary>
        /// Translates Transaction into a ledger entry.
        /// </summary>
        /// <param name="xact"></param>
        /// <returns></returns>
        public static string TranslateToLedger(Xact xact)
        {
            var output = new StringBuilder();

            // Xact Header
            output.Append(xact.Date.ToString(Constants.ISODateFormat));
            output.Append(' ');
            output.AppendLine(xact.Payee);

            if (!string.IsNullOrWhiteSpace(xact.Note))
            {
                // Comment
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

        // Private area

        private static string Serialize(object obj)
        {
            // use lower-case property names
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            var output = JsonConvert.SerializeObject(obj, serializerSettings);
            return output;
        }
    }
}
