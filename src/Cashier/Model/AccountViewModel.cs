using Cashier.Data.Entities;

namespace Cashier.Model
{
    /// <summary>
    /// Used inside the application.
    /// The base is the Account data model
    /// </summary>
    public class AccountViewModel : Account
    {
        public AccountViewModel(Account account)
            : base(account.Name)
        {
            _account = account;

            this.Balances = account.Balances;
            this.CurrentValue = account.CurrentValue;
            this.CurrentCurrency = account.CurrentCurrency;
        }

        private Account _account;

        /// <summary>
        /// Used to sum up the balance of sub-accounts in Asset Allocation calculation.
        /// </summary>
        public Money? AccountBalance { get; set; }

        public string AccountName
        {
            get
            {
                if (this.Name == null)
                {
                    return string.Empty;
                }

                var separatorIndex = this.Name.LastIndexOf(':');
                return this.Name.Substring(separatorIndex + 1);
            }
        }

        public string ParentAccountName
        {
            get
            {
                if (this.Name == null)
                {
                    return string.Empty;
                }

                var separatorIndex = this.Name.LastIndexOf(':');
                return this.Name.Substring(0, separatorIndex);
            }
        }
    }
}
