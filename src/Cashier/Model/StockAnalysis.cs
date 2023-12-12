namespace Cashier.Model
{
    /// <summary>
    /// A DTO for showing the stock analysis results.
    /// </summary>
    public class StockAnalysis
    {
        public StockAnalysis() { }

        public string? Yield { get; set; }
        public string? GainLoss { get; set; }

        public override string ToString()
        {
            // return base.ToString();
            return $"Yield: {Yield}, Gain/Loss: {GainLoss}";
        }
    }
}
