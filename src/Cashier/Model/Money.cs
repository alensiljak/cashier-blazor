using Cashier.Data;

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

        // public static Money Create(decimal amount, st)

        public Money() { }

        public Money(decimal amount, string? currency)
        {
            Quantity = amount;
            Currency = currency;
        }

        public Money(int amount, string? currency)
        {
            Quantity = (decimal)amount;
            Currency = currency;
        }

        public Money(float amount, string? currency)
        {
            Quantity = (decimal)amount;
            Currency = currency;
        }

        public decimal? Quantity { get; set; }
        public string? Currency { get; set; }

        public override string ToString()
        {
            return $"{Quantity?.ToString(Constants.NUMBER_FORMAT)} {Currency}";
        }

        public static Money operator +(Money a, Decimal b)
        {
            a.Quantity += b;
            return a;
        }
    }
}
