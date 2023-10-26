namespace Cashier.Domain
{
    public class Xact
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Payee { get; set; } = string.Empty;
    }
}
