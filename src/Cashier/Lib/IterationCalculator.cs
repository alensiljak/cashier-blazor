using Cashier.Data;
using NodaTime;
using System.Globalization;

namespace Cashier.Lib
{
    /// <summary>
    /// Calculates iterations for Scheduled Transactions.
    /// </summary>
    public class IterationCalculator
    {
        /// <summary>
        /// Calculate the schedule based on the given parameters.
        /// </summary>
        /// <returns>
        /// The next date as string.
        /// </returns>
        public DateOnly? calculateNextIteration(DateOnly startDate, int? count, Periods? period, DateOnly? endDate)
        {
            if (count == null || period == null)
            {
                // There is no next occurrence.
                return null;
            }

            // Get the start point.
            var start = LocalDate.FromDateOnly(startDate);
            LocalDate next;

            // add the given period
            switch (period)
            {
                case Periods.StartOfMonth:
                    next = start.PlusMonths(count.Value)
                        .With(DateAdjusters.StartOfMonth);
                    break;
                case Periods.EndOfMonth:
                    next = start.PlusMonths(count.Value)
                        .With(DateAdjusters.EndOfMonth);
                    break;
                case Periods.Days:
                    next = start.PlusDays(count.Value);
                    break;

                case Periods.Weeks:
                    next = start.PlusWeeks(count.Value);
                    break;

                case Periods.Months:
                    next = start.PlusMonths(count.Value);
                    break;

                case Periods.Years:
                    next = start.PlusYears(count.Value);
                    break;

                default:
                    throw new Exception("Invalid period");
            }

            // handle end date, if any.
            if (endDate != null)
            {
                var end = LocalDate.FromDateOnly(endDate.Value);
                if (next > end)
                {
                    // no more iterations, end date passed
                    return null;
                }
            }

            return next.ToDateOnly();
        }
    }
}
