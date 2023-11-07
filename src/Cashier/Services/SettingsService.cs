using DexieNET;
using DexieNET.Component;
using Cashier.Domain;
using System.Text.Json;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Cashier.Lib;

namespace Cashier.Services
{
    // Operations for application settings.
    public class SettingsService
    {
        private CashierDB _db;

        public SettingsService(CashierDB db) {
            _db = db;
        }

        public async Task<string> Get(string key)
        {
            var record = await _db.Settings().Get(key);
            if (record == null)
            {
                await _db.Settings().Add(new Setting(key, string.Empty));
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
            return await _db.Settings().Put(new Setting(key, value));
        }

        public async Task<IEnumerable<string>> BulkPut(List<Setting> items)
        {
            return await _db.Settings().BulkPut(items);
        }

        [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
        public async Task<bool> GetBool(string key)
        {
            var record = await _db.Settings().Get(key);
            if (record == null)
            {
                return false;
            }
                
            var result = JsonSerializer.Deserialize<bool>(record.Value);
            return result;
        }

        public async Task<string> GetDefaultCurrency()
        {
            var record = await _db.Settings().Get(SettingsKeys.currency);

            Console.WriteLine("loaded ", record);

            return record?.Value ?? string.Empty;
        }
    }
}
