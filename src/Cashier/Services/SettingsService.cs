using System.Text.Json;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Cashier.Lib;
using Cashier.DAL;
using Cashier.Model;
using BlazorDexie.JsModule;
using Microsoft.JSInterop;

namespace Cashier.Services
{
    // Operations for application settings.
    public class SettingsService
    {
        private DexieDAL _db;

        public SettingsService(IJSRuntime jsRuntime) {
            var moduleFactory = new EsModuleFactory(jsRuntime);
            _db = new DexieDAL(moduleFactory);
        }

        public async Task<string> Get(string key)
        {
            var record = await _db.Settings.Get(key);
            if (record == null)
            {
                await _db.Settings.Add(new Setting(key, string.Empty));
                return string.Empty;
            }
            else
            {
#pragma warning disable IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
                var result = JsonSerializer.Deserialize<string>(record.Value);
                if (result == null)
                {
                    return string.Empty;
                } else
                {
                    return result;
                }
#pragma warning restore IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
            }
        }

        public async Task<string> Set(string key, string value)
        {
            var setting = new Setting(key, value);
            // return await _db.Settings().Update(key, setting => setting.Value,  value);
            return await _db.Settings.Put(new Setting(key, value));
        }

        public async Task<string> BulkPut(List<Setting> items)
        {
            return await _db.Settings.BulkPut(items);
        }

        [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
        public async Task<bool> GetBool(string key)
        {
            var record = await _db.Settings.Get(key);
            if (record == null)
            {
                return false;
            }
                
            var result = JsonSerializer.Deserialize<bool>(record.Value);
            return result;
        }

        public async Task<string> GetDefaultCurrency()
        {
            var record = await _db.Settings.Get(SettingsKeys.currency);

            Console.WriteLine("loaded ", record);

            return record?.Value ?? string.Empty;
        }

        [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
        public async Task<Account?[]> GetFavouriteAccounts()
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
                return new Account[0];
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
    }
}
