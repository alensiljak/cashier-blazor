namespace Cashier.Model
{
    public class Money
    {
        public Money() { }
        
        public Money(decimal amount, string currency) {
            Amount = amount;
            Currency = currency;
        }

        public decimal? Amount {  get; set; }
        public string? Currency { get; set; }
    }
}
