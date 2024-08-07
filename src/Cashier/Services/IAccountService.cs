﻿using Cashier.Data;
using Cashier.Data.Entities;
using Cashier.Model;

namespace Cashier.Services
{
    public interface IAccountService
    {
        Money GetAccountBalance(Account account, string defaultCurrency);
        Task<List<Account>> LoadInvestmentAccounts(ISettingsService settings, IDexieDAL dal);

        List<AccountViewModel> ConvertToViewModel(List<Account> accounts);
    }
}