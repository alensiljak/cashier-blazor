namespace Cashier.Domain
{
    public class Xact
    {
        public long Id { get; set; }
        public string Date { get; set; }
        public string Payee { get; set; } = string.Empty;
    }
}
