namespace Cashier.Model
{
    public class Money
    {
        public static Money Empty
        {
            get
            {
                return new Money(0, string.Empty);
            }
        }

        public Money() { }

        public Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public decimal? Amount { get; set; }
        public string? Currency { get; set; }

        public override string ToString()
        {
            //return base.ToString();

            return $"{Amount} {Currency}";
        }

        public static Money operator +(Money a, Decimal b)
        {
            a.Amount += b;
            return a;
        }
    }
}
