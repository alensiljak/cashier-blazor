namespace Cashier.Model
{
    public class Posting
    {
        public string? Account { get; set; }
        public Money? Amount { get; set; }

        public Posting() { }

        public Posting(string? account = null, Money? amount = null)
        {
            Account = account;
            Amount = amount;
        }

        public override string ToString()
        {
            return string.Format($"{Account} {Amount}");
        }
    }
}
