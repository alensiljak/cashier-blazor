using DexieNET;
using DexieNET.Component;
using Cashier.Domain;

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
            // todo
            // fetch data
            //var settings = _db.Settings().ToArray();
            var record = await _db.Settings().Get(key);
            if (record == null)
            {
                await _db.Settings().Add(new Setting(key, string.Empty));
                return string.Empty;
            }
            else
            {
                return record.Value;
            }
        }

        public async Task<bool> Set(string key, string value)
        {
            // _db.Open()
            // return await _db.Settings().Update(key, () => new Setting(key, value), value);
            var setting = new Setting(key, value);
            return await _db.Settings().Update(key, setting => setting.Value,  value);
        }

        public async Task<IEnumerable<string>> BulkInsert(List<Setting> items)
        {
            return await _db.Settings().BulkPut(items);
        }
    }
}
