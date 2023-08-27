namespace WebApi.Commons
{
    public static class DateTimeExtensions
    {

        public static DateTime FirstDayOfMonth(this DateTime dateTime)
        {
            return dateTime.Date.AddDays(1 - dateTime.Day);
        }

        public static DateTime LastDayOfMonth(this DateTime dateTime)
        {
            return dateTime.Date.AddDays(1 - dateTime.Day).AddMonths(1).AddDays(-1);
        }

        public static DateTime FirstDayOfYear(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, 1, 1);
        }

        public static DateTime LastDayOfYear(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, 12, 31);
        }

        public static DateTime DefaultDate(this DateTime dateTime)
        {
            return new DateTime(1900, 01, 01);
        }

        public static DateTime MaxDate(this DateTime dateTime)
        {
            return new DateTime(9999, 12, 31);
        }
    }
}
