using BlazorDexie.JsInterop;
using Cashier.Data;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Cashier.Services
{
    /// <summary>
    /// Synchronizes with the Cashier Server.
    /// </summary>
    public class SyncService
    {
        private const string AccountsCommand = "b --flat --empty --no-total";
        private const string PayeesCommand = "payees";

        private HttpClient _httpClient;
        private string _serverUrl;

        public SyncService(HttpClient client, string serverUrl)
        {
            _httpClient = client;
            _serverUrl = serverUrl;
        }

        /// <summary>
        /// Demonstrates communicating with a remote server.
        /// </summary>
        /// <returns></returns>
        public async Task test()
        {
            HttpResponseMessage? response;
            var url = $"{_serverUrl}?command=b gratis";
            try
            {
                response = await _httpClient.GetAsync(url);
            }
            catch (HttpRequestException rex)
            {
                // The server is not up?
                Console.WriteLine("Error: {0}", rex.Message);
                return;
            }

            var code = response.StatusCode;
            Console.WriteLine("Code: {0}", code);

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Content: {0}", content);
        }

        [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
        public async Task<string[]?> ReadAccounts()
        {
            var response = await this.ledger(AccountsCommand);
            var lines = JsonSerializer.Deserialize<string[]>(response);
            return lines;
        }

        /// <summary>
        /// Read investment accounts' current values in the default currency, for Asset Allocation calculation.
        /// </summary>
        /// <returns></returns>
        [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
        public async Task<string> ReadCurrentValues(IJSRuntime jsRuntime)
        {
            var dal = new DexieDAL(jsRuntime);
            var settings = new SettingsService(dal);
            var rootAccount = await settings.GetRootInvestmentAccount();
            if (rootAccount == null)
            {
                throw new Exception("Root investment account not set!");
            }
            var currency = await settings.GetDefaultCurrency();
            if (currency == null)
            {
                throw new Exception("Default currency net set!");
            }

            var command = $"b ^{rootAccount} -X {currency} --flat --no-total";
            var response = await this.ledger(command);

            var result = JsonSerializer.Deserialize<string[]>(response);
            if (result == null)
            {
                throw new Exception("No response received from the sync server.");
            }

            var currentValues = ParseCurrentValues(result, rootAccount);

            await ImportCurrentValuesJson(currentValues, jsRuntime);
            return "OK";
        }

        /// <summary>
        /// Retrieve the list of Payees
        /// </summary>
        /// <returns></returns>
        [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
        public async Task<string[]?> ReadPayees()
        {
            // Limit the payees to the last 5 years, otherwise there's a high risk of crashing.
            // This command is somehow very memory hungry on Android.
            var year = DateTime.Now.Year;
            year = year - 5;

            var command = PayeesCommand + " -b " + year;
            var result = await ledger(command);
            var payees = JsonSerializer.Deserialize<string[]>(result);
            return payees;
        }

        public async Task Shutdown()
        {
            try
            {
                await _httpClient.GetAsync(_serverUrl + "/shutdown");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Private

        private string createUrl(string command)
        {
            var path = $"?command={command}";
            var url = $"{this._serverUrl}{path}";
            return url;
        }

        /// <summary>
        /// Update the current balances in the asset allocation.
        /// The current values are stored in the Account records.
        /// </summary>
        /// <param name="currentValues"></param>
        private async Task ImportCurrentValuesJson(Dictionary<string, string> currentValues, IJSRuntime jsRuntime)
        {
            var accounts = currentValues.Keys;
            foreach (var key in accounts)
            {
                var balance = currentValues[key];
                balance = balance.Replace(",", string.Empty);

                // extract the currency
                var parts = balance.Split(' ');
                var amount = parts[0];
                var currency = parts[1];

                // Update existing account.
                var db = new DexieDAL(jsRuntime);
                var account = await db.Accounts.Get(key);
                if (account == null)
                {
                    throw new Exception("Invalid account!");
                }

                // Update the values
                account.CurrentValue = amount;
                account.CurrentCurrency = currency;

                await db.Accounts.Put(account);
            }
        }

        private async Task<string> ledger(string command)
        {
            var url = createUrl(command);

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error communicating with Cashier Server");
            }

            var content = await response.Content.ReadAsStringAsync();

            return content;
        }

        private Dictionary<string, string> ParseCurrentValues(string[] lines, string rootAccount)
        {
            var result = new Dictionary<string, string>();
            foreach (var line in lines)
            {
                if (line == string.Empty) continue;

                var row = line.Trim();

                var rootIndex = row.IndexOf(rootAccount);
                var amount = row.Substring(0, rootIndex);
                amount = amount.Trim();

                var account = row.Substring(rootIndex);

                result.Add(account, amount);
            }
            return result;
        }
    }
}
