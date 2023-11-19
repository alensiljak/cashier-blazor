using System.Text.Json;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Cashier.Lib;
using Cashier.Model;
using BlazorDexie.JsModule;
using Microsoft.JSInterop;
using Cashier.Data;

namespace Cashier.Services
{
    // Operations for application settings.
    public class SettingsService
    {
        public static SettingsService CreateInstance(IJSRuntime jsRuntime)
        {
            var db = DexieDAL.CreateInstance(jsRuntime);
            return new SettingsService(db);
        }
        private IDexieDAL _db;

        public SettingsService(IDexieDAL dal) {
            _db = dal;
        }

        public async Task<string> BulkPut(List<Setting> items)
        {
            return await _db.Settings.BulkPut(items);
        }

        public async Task<string> GetDefaultCurrency()
        {
            var record = await _db.Settings.Get(SettingsKeys.currency);

            //Console.WriteLine("loaded ", record);

            return record?.Value ?? string.Empty;
        }

        public async Task<string> GetSyncServerUrl()
        {
            var record = await _db.Settings.Get(SettingsKeys.syncServerUrl);
            return record?.Value ?? string.Empty;
        }

        public async Task<string> GetRootInvestmentAccount()
        {
            var record = await _db.Settings.Get(SettingsKeys.rootInvestmentAccount);
            return record?.Value ?? string.Empty;
        }

        public async Task<bool> GetRememberLastTransaction()
        {
            return await GetBool(SettingsKeys.rememberLastTransaction);
        }

        public async Task<bool> GetSyncAccounts()
        {
            return await GetBool(SettingsKeys.syncAccounts);
        }

        public async Task<bool> GetSyncAaValues()
        {
            return await GetBool(SettingsKeys.syncAaValues);
        }

        public async Task<bool> GetSyncPayees()
        {
            return await GetBool(SettingsKeys.syncPayees);
        }

        /// <summary>
        /// Loads favourite accounts.
        /// The account names are stored in the Settings. Then, the Accounts are loaded from the database by name.
        /// </summary>
        /// <param name="take">Take only the first N accounts.</param>
        /// <returns></returns>
        [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
        public async Task<Account?[]> GetFavouriteAccounts(int? take = null)
        {
            var setting = await _db.Settings.Get(SettingsKeys.favouriteAccounts);
            if (setting == null)
            {
                return [];
            }

            var keysJson = setting.Value;
            //Console.WriteLine("The account keys: {0}", keysJson);

            var keys = JsonSerializer.Deserialize<string[]>(keysJson);
            //Console.WriteLine("Deserialized: {0}", keys);
            if (keys == null)
            {
                return [];
            }

            // Take only top n.
            if (take != null)
            {
                keys = keys.Take(5).ToArray();
            }

            var accounts = await _db.Accounts.BulkGet(keys);
            if (accounts == null)
            {
                return [];
            }

            // Handle any accounts that have not been found in the Accounts table.
            var result = new List<Account>();
            foreach ( var account in accounts)
            {
                if (account != null)
                {
                    result.Add(account);
                }
            }
            return result.ToArray();
        }

        // Save methods

        public async Task<string> SetDefaultCurrency(string value)
        {
            return await SetString(SettingsKeys.currency, value);
        }

        public async Task<string> SetRootInvestmentAccount(string value)
        {
            return await SetString(SettingsKeys.rootInvestmentAccount, value);
        }

        public async Task<string> SetSyncAccounts(bool value)
        {
            return await SetBool(SettingsKeys.syncAccounts, value);
        }

        public async Task<string> SetSyncAaValues(bool value)
        {
            return await SetBool(SettingsKeys.syncAaValues, value);
        }

        public async Task<string> SetSyncPayees(bool value)
        {
            return await SetBool(SettingsKeys.syncPayees, value);
        }

        public async Task<string> SetSyncServerUrl(string value)
        {
            return await SetString(SettingsKeys.syncServerUrl, value);
        }

        // Private

        private async Task<bool> GetBool(string key)
        {
            var record = await _db.Settings.Get(key);
            if (record == null)
            {
                return false;
            }

            // var result = JsonSerializer.Deserialize<bool>(record.Value);
            _ = bool.TryParse(record.Value, out bool result);

            return result;
        }

        private async Task<string> SetBool(string key, bool value)
        {
            var record = await _db.Settings.Get(key);
            if (record == null)
            {
                throw new Exception("The setting not found!");
            }

            record.Value = value.ToString();

            var result = await _db.Settings.Put(record);

            return result;
        }

        private async Task<string> SetString(string key, string value)
        {
            var record = await _db.Settings.Get(key);
            if (record == null)
            {
                // throw new Exception("The setting not found!");
                record = new Setting(key, value);
            }

            record.Value = value;

            var result = await _db.Settings.Put(record);

            return result;
        }
    }
}
