using Cashier.Model;

namespace Cashier.Data.Entities
{
    public class Account()
    {
        //public Account() { }

        public Account(string? name) : this()
        {
            Name = name;
        }

        public string? Name { get; set; }

        public Money[]? Balances { get; set; }

        /// <summary>
        /// The value in the default currency.
        /// </summary>
        public string? CurrentValue { get; set; }

        /// <summary>
        /// The default/common currency.
        /// </summary>
        public string? CurrentCurrency { get; set; }
    }
}
