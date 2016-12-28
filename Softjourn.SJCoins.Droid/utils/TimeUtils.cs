using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Text;
using Java.Util;


namespace TrololoWorld.utils
{
    public class TimeUtils
    {
        private static Date GetDateFromString(string dateString)
        {
            DateFormat format = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss", Locale.Us);
            Date date;
            try
            {
                date = format.Parse(dateString);
            }
            catch (ParseException e)
            {
                e.PrintStackTrace();
                return null;
            }
            return date;
        }

        public static string GetPrettyTime(string dateString)
        {
            DateFormat format = new SimpleDateFormat("dd.MM.yyyy", Locale.Us);
            Date date = GetDateFromString(dateString);

            return date == null ? dateString : format.Format(date);
        }
    }
}