namespace Cashier.Model
{
    public class ScheduledXact
    {
        public long? Id { get; set; }
        public string? NextDate { get; set; }
        public Xact? Transaction { get; set; }
        public string? Period { get; set; }
        public string? Count { get; set; }
        public string? EndDate { get; set; }
        public string? Remarks { get; set; }
    }
}
