using Cashier.Model;

namespace Cashier.Lib
{
    /// <summary>
    /// Business logic for schedules (i.e. scheduling a transaction).
    /// </summary>
    public class Scheduler
    {
        /// <summary>
        /// Project the schedule in the period start date - end date.
        /// For example, if the schedule repeats daily and the end date is in a week's time,
        /// the result should be 7 dates/occurrences.
        /// </summary>
        /// <param name="scheduledXact">Scheduled Transaction with the schedule information.</param>
        /// <param name="startDate">The start date for the projection period.</param>
        /// <param name="endDate">The end date of the projection period.</param>
        public List<DateOnly> ProjectSchedule(ScheduledXact scheduledXact, DateOnly startDate, DateOnly endDate)
        {
            var result = new List<DateOnly>();

            if (scheduledXact.NextDate > endDate) return result;

            throw new NotImplementedException();
        }
    }
}
