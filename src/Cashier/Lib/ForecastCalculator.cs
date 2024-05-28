using BlazorDexie.Database;
using Cashier.Data;
using Cashier.Model;
using Cashier.Services;

namespace Cashier.Lib
{
    public class ForecastCalculator
    {
        private IDexieDAL _db;

        public ForecastCalculator(IDexieDAL db)
        {
            _db = db;
        }

        public async Task<AccountViewModel> GetAccountBalance(string accountName, string currency)
        {
            // load the account balance.
            var entity = await _db.Accounts.Get(accountName);
            if (entity == null)
            {
                throw new Exception("Could not find account");
            }

            var account = new AccountViewModel(entity);
            account.AccountBalance = new AccountService().GetAccountBalance(account, currency);

            return account;
        }
    }
}
