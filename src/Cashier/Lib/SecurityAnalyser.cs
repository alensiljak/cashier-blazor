using Cashier.Data;
using Cashier.Services;
using NodaTime;

namespace Cashier.Lib
{
    /// <summary>
    /// Security Analysis for symbols.
    /// Calculates yield, etc.
    /// </summary>
    public class SecurityAnalyser
    {
        public SecurityAnalyser(SyncService syncService, ISettingsService settings)
        {
            Settings = settings;

            LedgerApi = syncService;
        }

        private SyncService LedgerApi { get; set; }
        private ISettingsService Settings { get; set; }

        private string? Currency { get; set; }

        public async Task<string> GetGainLoss(string symbol)
        {
            if (string.IsNullOrEmpty(Currency)) Currency = await Settings.GetDefaultCurrency();

            var command = $"b ^Assets and :{symbol}$ -G -n -X {Currency}";
            var report = await LedgerApi.ledger(command);
            if(report.Count == 0) {
                return "0";
            }

            var line = report[0];
            if (line == null)
            {
                return "0";
            }

            var number = getNumberFromCollapseResult(line);

            var result = number.ToString() + " " + Currency;

            // todo: calculate the percentage

            return result;
        }

        /// <summary>
        /// Calculate the yield in the last 12 months.
        /// This value is affected by the recent purchases, which result in seemingly lower yield!
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public async Task<string> GetYield(string symbol)
        {
            if (string.IsNullOrEmpty(Currency)) Currency = await Settings.GetDefaultCurrency();

            // Retrieve income amount.
            var incomeStr = await this.getIncomeBalance(symbol);
            var income = decimal.Parse(incomeStr);
            // turn into a positive number
            income = income * (-1);

            // Retrieve the current value of the holding.
            var valueStr = await this.getValueBalance(symbol, Currency);
            var value = decimal.Parse(valueStr);

            decimal yield;
            if (value == 0)
            {
                yield = 0;
            }
            else
            {
                yield = (income * 100) / value;
            }

            var result = yield.ToString("N2") + "%";
            return result;
        }

        // private

        /// <summary>
        /// Get the income in the last year.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        private async Task<string> getIncomeBalance(string symbol)
        {
            var currency = this.Currency;
            var yieldFrom = LocalDate.FromDateTime(DateTime.Now).Minus(Period.FromYears(1))
                .ToString(Constants.ISODateFormat, null);

            var command = $"b ^Income and :{symbol}$ -b {yieldFrom} --flat -X {currency}";

            var report = await LedgerApi.ledger(command);

            var total = this.extractTotal(report);
            return total;
        }

        /// <summary>
        /// Parses a 1-line ledger result, when --collapse is used
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private string getNumberFromCollapseResult(string line)
        {
            line = line.Trim();

            // -1,139 EUR  Assets
            var parts = line.Split(' ');
            if (parts.Length != 4)
            {
                throw new Exception("wrong number of parts!");
            }

            var totalNumeric = parts[0];
            totalNumeric = totalNumeric.Replace(",", string.Empty);
            return totalNumeric;
        }

        private async Task<string> getValueBalance(string symbol, string currency)
        {
            var command = $"b ^Assets and :{symbol}$ -X {currency}";
            var response = await this.LedgerApi.ledger(command);
            var total = extractTotal(response);

            return total;
        }

        /// <summary>
        /// Extracts the Total value from a ledger response.
        /// </summary>
        private string extractTotal(List<string> ledgerReport)
        {
            if (ledgerReport.Count == 0) return "0";

            var parser = new LedgerOutputParser();
            var totalLines = parser.getTotalLines(ledgerReport);
            var totalLine = totalLines[0];

            // Gets the numeric value of the total from the ledger total line
            totalLine = totalLine.Trim();
            var parts = totalLine.Split(' ');
            var totalNumeric = parts[0];
            // remove thousand-separators
            totalNumeric = totalNumeric.Replace(",", string.Empty);

            return totalNumeric;
        }
    }
}
