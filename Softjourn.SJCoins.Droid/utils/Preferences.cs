using System;

using Android.App;
using Android.Content;
using Softjourn.SJCoins.Droid.Utils;

namespace Softjourn.SJCoins.Droid.utils
{
    public class Preferences : Const
    {
        private static ISharedPreferences sharedPreferences = Application.Context.GetSharedPreferences(SjCoinsPreferences, FileCreationMode.Private);
        private static ISharedPreferencesEditor editor = sharedPreferences.Edit();

        public static string RetrieveStringObject(string keyValue)
        {
            return sharedPreferences.GetString(keyValue, null);
        }

        public static bool RetrieveBooleanObject(string key)
        {
            return sharedPreferences.GetBoolean(key, true);
        }

        public static void StoreBooleanObject(string key, bool value)
        {
            editor.PutBoolean(key, value);
            editor.Commit();
        }

        public static void StoreObject(string key, string value)
        {
            editor.PutString(key, value);
            editor.Commit();
        }

        public static void ClearStringObject(string key)
        {
            editor.PutString(key, "");
            editor.Commit();
        }
    }
}