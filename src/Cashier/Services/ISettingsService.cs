using Cashier.Data.Entities;

namespace Cashier.Services
{
    public interface ISettingsService
    {
        Task<string> BulkPut(List<Setting> items);

        Task<T?> GetSetting<T>(string key);
        Task<string> SetSetting<T>(string key, T value);

        Task<string> GetDefaultCurrency();

        Task<List<string>> GetFavouriteAccountNames();

        Task<bool> GetRememberLastTransaction();
        Task<string?> GetRootInvestmentAccount();
        Task<bool> GetSyncAaValues();
        Task<bool> GetSyncAccounts();
        Task<bool> GetSyncPayees();
        Task<List<string>> GetVisibleCards();
        
        Task<string> SetDefaultCurrency(string value);
        Task<string> SetRootInvestmentAccount(string value);
        Task<string> SetSyncAaValues(bool value);
        Task<string> SetSyncAccounts(bool value);
        Task<string> SetSyncPayees(bool value);
    }
}