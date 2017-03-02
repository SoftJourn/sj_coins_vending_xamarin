using System;

namespace Softjourn.SJCoins.Core.Utils
{
    public class TimeUtils
    {
        private static DateTime GetDateFromString(string dateString)
        {
            return DateTime.Parse(dateString);
        }

        public static string GetPrettyTime(string dateString)
        {
            return $"{GetDateFromString(dateString):dd/MM/yyyy}";
        }
    }
}
