using System.ComponentModel;
using System.Runtime.Serialization;

namespace Cashier.Lib
{
    public enum RecurrencePeriods
    {
        Days,

        Weeks,

        Months,

        [EnumMember(Value = "start of month")]
        [Description("Start of Month")]
        StartOfMonth,

        [EnumMember(Value = "end of month")]
        [Description("End of Month")]
        EndOfMonth,

        Years
    }

    public static class PeriodNames
    {
        public const string Days = "days";
        public const string Weeks = "weeks";
        public const string Months = "months";
        public const string StartOfMonth = "start of month";
        public const string EndOfMonth = "end of month";
        public const string Years = "years";

        public static string ToString(RecurrencePeriods value)
        {
            switch (value)
            {
                case RecurrencePeriods.Days:
                    return PeriodNames.Days;

                case RecurrencePeriods.Weeks:
                    return PeriodNames.Weeks;

                case RecurrencePeriods.Months:
                    return PeriodNames.Months;

                case RecurrencePeriods.StartOfMonth:
                    return PeriodNames.StartOfMonth;

                case RecurrencePeriods.EndOfMonth:
                    return PeriodNames.EndOfMonth;

                case RecurrencePeriods.Years:
                    return PeriodNames.Years;

                default:
                    return string.Empty;
            }
        }

        public static RecurrencePeriods ToEnum(string value)
        {
            switch (value)
            {
                case PeriodNames.Days:
                    return RecurrencePeriods.Days;

                case PeriodNames.Weeks:
                    return RecurrencePeriods.Weeks;

                case PeriodNames.Months:
                    return RecurrencePeriods.Months;

                case PeriodNames.StartOfMonth:
                    return RecurrencePeriods.StartOfMonth;

                case PeriodNames.EndOfMonth:
                    return RecurrencePeriods.EndOfMonth;

                case PeriodNames.Years:
                    return RecurrencePeriods.Years;

                default:
                    throw new InvalidEnumArgumentException(nameof(value));
            }
        }
    }
}
