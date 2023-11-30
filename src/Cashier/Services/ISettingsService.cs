using Cashier.Model;

namespace Cashier.Services
{
    public interface ISettingsService
    {
        Task<string> BulkPut(List<Setting> items);

        Task<T?> GetSetting<T>(string key);
        Task<string> SetSetting<T>(string key, T value);

        Task<string> GetDefaultCurrency();

        Task<string[]> GetFavouriteAccountNames();

        Task<bool> GetRememberLastTransaction();
        Task<string?> GetRootInvestmentAccount();
        Task<bool> GetSyncAaValues();
        Task<bool> GetSyncAccounts();
        Task<bool> GetSyncPayees();
        Task<string?> GetSyncServerUrl();
        
        Task<string> SetDefaultCurrency(string value);
        Task<string> SetRootInvestmentAccount(string value);
        Task<string> SetSyncAaValues(bool value);
        Task<string> SetSyncAccounts(bool value);
        Task<string> SetSyncPayees(bool value);
        Task<string> SetSyncServerUrl(string value);
    }
}