/*
 * Domain / Data model
 */
namespace Cashier.Model
{
    public class AssetAllocation
    {
        public string? FullName { get; set; }
    }

    public class Account(string? name)
    {
        public string? Name { get; set; } = name;

        /// <summary>
        /// Used to sum up the balance of sub-accounts in Asset Allocation calculation.
        /// </summary>
        public Money? AccountBalance { get; set; }
        public Money[]? Balances { get; set; }
        /// <summary>
        /// The value in the default currency.
        /// </summary>
        public string? CurrentValue {  get; set; }
        /// <summary>
        /// The default/common currency.
        /// </summary>
        public string? CurrentCurrency { get; set;}
    }

    public class LastXact
    {
        public string? Payee { get; set; }
        public Xact? Xact { get; set; }
    }

    public class Payee
    {
        public string? Name { get; set; }
    }

    public class ScheduledXact
    {
        public int? Id { get; set; }
        public string? NextDate { get; set; }
        public Xact? Xact { get; set; }
        public string? Period { get; set; }
        public string? Count { get; set; }
        public string? EndDate { get; set; }
        public string? Remarks { get; set; }
    }

    public class Setting(string key, string value)
    {
        public string Key { get; set; } = key;
        public string Value { get; set; } = value;
    }
}