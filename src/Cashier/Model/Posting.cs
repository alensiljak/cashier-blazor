using Newtonsoft.Json;

namespace Cashier.Model
{
    public class Posting
    {
        public string? Account { get; set; }
        
        [JsonIgnore]
        public Money? Money { get; set; }

        public decimal? Amount
        {
            get
            {
                return Money?.Quantity;
            }
            set
            {
                if(Money is null)
                {
                    Money = new Money();
                }

                Money.Quantity = value;
            }
        }

        public string? Currency
        {
            get
            {
                return Money?.Currency;
            }
            set
            {
                if (Money is null)
                {
                    Money = new Money();
                }
                Money.Currency = value;
            }
        }

        public Posting() { }

        public Posting(string? account = null, Money? amount = null)
        {
            Account = account;
            Money = amount;
        }

        public override string ToString()
        {
            return string.Format($"{Account} {Money}");
        }
    }
}
