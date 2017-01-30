using Android.App;
using Android.Content;
using Android.Net;


namespace Softjourn.SJCoins.Droid.Utils
{
    public class NetworkUtils
    {
        public static bool IsNetworkEnabled()
        {
            ConnectivityManager connectivityManager = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
            var info = connectivityManager.ActiveNetworkInfo;

            return info != null && info.IsConnected;
        }
    }
}