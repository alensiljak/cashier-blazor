namespace Cashier.Model
{
    public class Posting
    {
        public string? Account { get; set; }

        //public Money? Money { get; set; }

        public decimal? Amount { get; set; }

        public string? Currency { get; set; }

        public Posting() { }

        public Posting(string? account = null, Money? amount = null)
        {
            Account = account;
            if (amount != null)
            {
                Amount = amount.Quantity;
                Currency = amount.Currency;
            }
        }

        public Posting Clone()
        {
            var newItem = new Posting
            {
                Account = Account,
                Amount = Amount,
                Currency = Currency
            };
            return newItem;
        }

        public override string ToString()
        {
            return string.Format($"{Account} {Amount} {Currency}");
        }
    }
}
