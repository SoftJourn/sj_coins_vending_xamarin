using System;

namespace Softjourn.SJCoins.Core.Common.Utils
{
    public static class TimeUtils
    {
        public static string GetPrettyTime(string dateString) => $"{GetDateFromString(dateString):MM/dd/yyyy}";

        private static DateTime GetDateFromString(string dateString) => DateTime.Parse(dateString);
    }
}
