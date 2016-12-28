using Android.App;
using Android.Content;
using Android.Net;


namespace TrololoWorld.utils
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