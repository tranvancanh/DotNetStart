using System.Globalization;

namespace WebApi.Commons
{
    public static class StringExtensions
    {
        public static string ToTitleCase(this string str)
        {
            // Convert the string to title case
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str);
        }
    }
}
