using Microsoft.Data.SqlClient;

namespace WebApi.Commons
{
    public class Util
    {
        public static string NullToBlank(string value)
        {
            // NULL、DBNullのときは空文字に変換する
            if (string.IsNullOrWhiteSpace(value) == true)
            {
                return string.Empty;
            }
            else
            {
                return Convert.ToString(value).Trim();
            }
            
        }

        public static int NullToBlank(object value)
        {
            // NULL、DBNullのときは空文字に変換する
            if (string.IsNullOrWhiteSpace(value.ToString()) == true)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(value.ToString().Trim());
            }
        }

        public static string TimeSpanToString(object value)
        {
            TimeSpan time;
            if (TimeSpan.TryParse(Convert.ToString(value), out time))
            {
                return time.ToString(@"hh\:mm");
            }
            else
            {
                return null;
            }
        }

        public static string DateTimeToString(object value)
        {
            DateTime datetime;
            if (DateTime.TryParse(Convert.ToString(value), out datetime))
            {
                return datetime.ToString("yyyy/MM/dd HH:mm:ss");
            }
            else
            {
                return null;
            }
        }

        public static decimal Decimal(object value)
        {
            // NULL、DBNullのときは空文字に変換する
            if (string.IsNullOrWhiteSpace(value.ToString()) == true)
            {
                return (decimal)0;
            }
            else
            {
                return Convert.ToDecimal(value);
            }
        }
        public static double DivideTwoNumbers(int a, int b)
        {
            // ゼロのときは空文字に変換する
            if (b == 0)
            {
                return 0;
            }
            else
            {
                return Math.Ceiling((double)a/b);
            }

        }
        public static string GetDataDB(SqlDataReader reader, string column)
        {
            if (string.IsNullOrWhiteSpace(reader[column].ToString()) == true)
            {
                return reader[column].ToString().Trim();
            }
            else
            {
                return string.Empty;
            }
        }

        static public string WeekdayName(DayOfWeek week)
        {
            string yobi = "曜日";
            switch (week)
            {
                case DayOfWeek.Sunday:
                    yobi = "日曜日";
                    break;
                case DayOfWeek.Monday:
                    yobi = "月曜日";
                    break;
                case DayOfWeek.Tuesday:
                    yobi = "火曜日";
                    break;
                case DayOfWeek.Wednesday:
                    yobi = "水曜日";
                    break;
                case DayOfWeek.Thursday:
                    yobi = "木曜日";
                    break;
                case DayOfWeek.Friday:
                    yobi = "金曜日";
                    break;
                case DayOfWeek.Saturday:
                    yobi = "土曜日";
                    break;
            }
            return yobi;
        }
    }
}
