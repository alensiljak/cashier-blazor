using System.ComponentModel;
using System.Runtime.Serialization;

namespace Cashier.Lib
{
    public enum Periods
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

        public static string ToString(Periods value)
        {
            switch (value)
            {
                case Periods.Days:
                    return PeriodNames.Days;

                case Periods.Weeks:
                    return PeriodNames.Weeks;

                case Periods.Months:
                    return PeriodNames.Months;

                case Periods.StartOfMonth:
                    return PeriodNames.StartOfMonth;

                case Periods.EndOfMonth:
                    return PeriodNames.EndOfMonth;

                case Periods.Years:
                    return PeriodNames.Years;

                default:
                    return string.Empty;
            }
        }

        public static Periods ToEnum(string value)
        {
            switch (value)
            {
                case PeriodNames.Days:
                    return Periods.Days;

                case PeriodNames.Weeks:
                    return Periods.Weeks;

                case PeriodNames.Months:
                    return Periods.Months;

                case PeriodNames.StartOfMonth:
                    return Periods.StartOfMonth;

                case PeriodNames.EndOfMonth:
                    return Periods.EndOfMonth;

                case PeriodNames.Years:
                    return Periods.Years;

                default:
                    throw new InvalidEnumArgumentException(nameof(value));
            }
        }
    }
}
