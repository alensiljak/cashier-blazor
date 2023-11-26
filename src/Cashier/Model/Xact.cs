using Cashier.Data;

namespace Cashier.Model
{
    public class Xact(DateOnly date, string? payee = null, string? note = null, List<Posting>? postings = null)
    {
        public long? Id { get; set; }
        public DateOnly Date { get; set; } = date;
        public string? Payee { get; set; } = payee;
        public string? Note { get; set; } = note;
        public List<Posting>? Postings { get; set; } = postings;

        public override string ToString()
        {
            var postings = new StringWriter();
            if (Postings != null)
            {
                foreach (var posting in Postings)
                {
                    postings.Write(posting.ToString());
                    postings.Write(", ");
                }
            }
            return string.Format($"{Date.ToString(Constants.ISODateFormat)} {Payee}, {Note ?? "<no note>"}, {postings}");
        }
    }
}
