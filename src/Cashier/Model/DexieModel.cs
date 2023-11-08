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

        public Money? AccountBalance { get; set; }
        public Money[]? Balances { get; set; }
        public string? CurrentValue {  get; set; }
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

    public class Posting(string account, Money? money)
    {
        public string? Account { get; set; } = account;
        public Money? Money { get; set; } = money;
    }

    public class Xact(string date, string? payee, string? note, Posting[]? postings)
    {
        public int? Id { get; set; }
        public string? Date { get; set; } = date;
        public string? Payee { get; set; } = payee;
        public string? Note { get; set; } = note;
        public Posting[]? Postings { get; set; } = postings;
    }
}