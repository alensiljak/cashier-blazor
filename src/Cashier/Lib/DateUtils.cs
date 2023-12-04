namespace Cashier.Lib
{
    public static class DateUtils
    {
        public static DateOnly Today
        {
            get
            {
                return DateOnly.FromDateTime(DateTime.Today);
            }
        }
    }
}
