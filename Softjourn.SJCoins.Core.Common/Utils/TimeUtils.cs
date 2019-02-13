using System;

namespace Softjourn.SJCoins.Core.Common.Utils
{
    public static class TimeUtils
    {
        public static string GetPrettyTime(string dateString)
        {
            return $"{GetDateFromString(dateString):MM/dd/yyyy}";
        }

        private static DateTime GetDateFromString(string dateString)
        {
            return DateTime.Parse(dateString);
        }
    }
}
