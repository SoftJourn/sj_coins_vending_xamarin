using System;

namespace Softjourn.SJCoins.Core.UI.Utils
{
    public class TimeUtils
    {
        private static DateTime GetDateFromString(string dateString)
        {
            return DateTime.Parse(dateString);
        }

        public static string GetPrettyTime(string dateString)
        {
            return $"{GetDateFromString(dateString):MM/dd/yyyy}";
        }
    }
}
