using BlazorDexie.Database;
using Cashier.Data;
using Cashier.Model;
using Cashier.Services;

namespace Cashier.Lib
{
    public class ForecastCalculator
    {
        private IDexieDAL _db;
        private List<AccountViewModel> _accounts;
        private string _currency;

        public ForecastCalculator(IDexieDAL db, string currency)
        {
            _db = db;
            _accounts = new List<AccountViewModel>();
            _currency = currency;
        }

        public async Task<AccountViewModel> GetAccountBalance(string accountName)
        {
            var account = await LoadAccount(accountName);

            // load the account balance.
            account.AccountBalance = new AccountService().GetAccountBalance(account, _currency);

            return account;
        }

        public async Task<double[]> Get30DayForecast(string accountName)
        {
            var forecast = new double[30];

            var account = await GetAccount(accountName);
            // await GetAccountBalance(accountName);

            var day = DateTime.Today;

            for (var i = 0; i < 30; i++)
            {
                day = day.AddDays(1);

                // initial value

                // add transactions

                // add scheduled transactions


            }

            return forecast.ToArray();
        }

        // Private

        /// <summary>
        /// Retrieves Scheduled Transactions due in the given period.
        /// todo: project the schedules for the whole period.
        /// </summary>
        /// <param name="limit"></param>
        private void LoadScheduledTxUntil(DateTime from, DateTime to)
        {
            // Get all the scheduled transactions involving this account.
            // Project the schedule (i.e. if we are looking at one week and the Xact is repeating every day,
            // create one transaction for each day).
            // Add to daily totals.
        }

        private async Task<AccountViewModel> LoadAccount(string accountName)
        {
            var entity = await _db.Accounts.Get(accountName);
            if (entity == null)
            {
                throw new Exception("Could not find account");
            }

            var account = new AccountViewModel(entity);
            return account;
        }

        private async Task<AccountViewModel> GetAccount(string accountName)
        {
            // get from the collection
            var account = _accounts.Find(a => a.AccountName == accountName);
            if (account != null)
            {
                return account;
            }
            else
            {
                account = await LoadAccount(accountName);
                _accounts.Add(account);
            }
            
            return account;
        }
    }
}
