using Cashier.Model;

namespace Cashier.Lib
{
    /// <summary>
    /// Formats text.
    /// </summary>
    public class Formatter
    {
        public static string GetMoneyColor(Money amount)
        {
            return GetAmountColour(amount.Quantity ?? 0);
        }

        /// <summary>
        /// CSS Colours for amounts (<0, 0, >0).
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static string GetAmountColour(decimal amount)
        {
            switch (amount)
            {
                case var n when n < 0:
                    return "#E53935"; // red Darken1

                //case var n when n == 0:
                //    return ""; // yellow

                case var n when n > 0:
                    return "#43A047"; // green Darken1

                default:
                    return string.Empty;
            }
        }

        public static string GetXactAmountColour(Xact? xact, Money amount)
        {
            var colour = string.Empty;

            if (xact == null)
            {
                return colour;
            }

            if (xact.Postings?.Count(p => (p.Account != null) && (p.Account.StartsWith("Assets:"))) == 2)
            {
                // Transfers are yellow
                // colour = ""; // yellow
            }
            else
            {
                colour = Formatter.GetAmountColour(amount.Quantity.Value);
            }
            return colour;
        }

        /// <summary>
        /// CSS colour for Scheduled Transactions (overdue, due).
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetDateColour(DateOnly date)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);

            switch (date)
            {
                case var n when n < today:
                    return "var(--mud-palette-secondary)";

                case var n when n == today:
                    return "var(--mud-palette-tertiary)";

                case var n when n > today:
                    return "var(--mud-palette-primary)";
            }
            return string.Empty;
        }
    }
}
