﻿using Cashier.Lib;
using Cashier.Model;
using Cashier.Data;
using Cashier.Data.Entities;
using Newtonsoft.Json;

namespace Cashier.Services
{
    /// <summary>
    /// Operations on application settings.
    /// </summary>
    public class SettingsService : ISettingsService
    {
        private IDexieDAL _db;

        public SettingsService(IDexieDAL dal)
        {
            _db = dal;
        }

        public async Task<string> BulkPut(List<Setting> items)
        {
            return await _db.Settings.BulkPut(items);
        }

        public async Task<string> GetDefaultCurrency()
        {
            var setting = await GetSetting<string>(SettingsKeys.currency);
            if (string.IsNullOrEmpty(setting))
            {
                throw new Exception("The default currency not set! Please adjust in Settings.");
            }

            return setting;
        }

        public async Task<string?> GetRootInvestmentAccount()
        {
            return await GetSetting<string?>(SettingsKeys.rootInvestmentAccount);
        }

        public async Task<bool> GetRememberLastTransaction()
        {
            return await GetSetting<bool>(SettingsKeys.rememberLastTransaction);
        }

        public async Task<bool> GetSyncAccounts()
        {
            return await GetSetting<bool>(SettingsKeys.syncAccounts);
        }

        public async Task<bool> GetSyncAaValues()
        {
            return await GetSetting<bool>(SettingsKeys.syncAaValues);
        }

        public async Task<bool> GetSyncPayees()
        {
            return await GetSetting<bool>(SettingsKeys.syncPayees);
        }

        // Save methods

        public async Task<List<string>> GetFavouriteAccountNames()
        {
            var setting = await _db.Settings.Get(SettingsKeys.favouriteAccounts);
            if (setting == null)
            {
                return [];
            }

            var keysJson = setting.Value;

            var keys = JsonConvert.DeserializeObject<string[]>(keysJson);
            if (keys == null)
            {
                return [];
            }

            return keys.ToList();
        }

        public async Task<List<string>> GetVisibleCards()
        {
            var cardList = await GetSetting<List<string>>(SettingsKeys.visibleCards);
            if (cardList == null)
            {
                return new List<string>
                {
                    Constants.CardNamespace + "FavouritesCard",
                    Constants.CardNamespace + "JournalCard",
                    Constants.CardNamespace + "ScheduledXactCard",
                    Constants.CardNamespace + "SyncCard"
                };
            }

            return cardList;
        }

        public async Task<string> SetDefaultCurrency(string value)
        {
            return await SetSetting(SettingsKeys.currency, value);
        }

        public async Task<string> SetRootInvestmentAccount(string value)
        {
            return await SetSetting(SettingsKeys.rootInvestmentAccount, value);
        }

        public async Task<string> SetSyncAccounts(bool value)
        {
            return await SetSetting(SettingsKeys.syncAccounts, value);
        }

        public async Task<string> SetSyncAaValues(bool value)
        {
            return await SetSetting(SettingsKeys.syncAaValues, value);
        }

        public async Task<string> SetSyncPayees(bool value)
        {
            return await SetSetting(SettingsKeys.syncPayees, value);
        }

        public async Task<T?> GetSetting<T>(string key)
        {
            var record = await _db.Settings.Get(key);
            if (record == null)
            {
                var msg = string.Format("Setting with the given key {0} not found.", key);
                
                Console.WriteLine(msg);

                return default(T);
            }

            var result = JsonConvert.DeserializeObject<T>(record.Value);

            return result!;
        }

        public async Task<string> SetSetting<T>(string key, T value)
        {
            var record = await _db.Settings.Get(key);
            if (record == null)
            {
                record = new Setting(key, string.Empty);
            }

            record.Value = JsonConvert.SerializeObject(value);
            
            var result = await _db.Settings.Put(record);

            return result;
        }
    }
}
