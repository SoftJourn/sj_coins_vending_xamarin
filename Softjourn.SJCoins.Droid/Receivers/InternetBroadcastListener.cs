
using Android.App;
using Android.Content;
using Android.Net;
using Softjourn.SJCoins.Droid.ui.activities;


namespace Softjourn.SJCoins.Droid.Receivers
{
    public class InternetBroadcastListener : BroadcastReceiver
    {
        private Activity _activity;

        public InternetBroadcastListener(Activity starter)
        {
            _activity = starter;
        }

        public override void OnReceive(Context context, Intent intent)
        {

            if (intent.Extras != null)
            {
                ConnectivityManager connectivityManager = (ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
                NetworkInfo ni = connectivityManager.ActiveNetworkInfo;

                if (ni != null && ni.IsConnectedOrConnecting)
                {
                    var intentToStart = new Intent(_activity, typeof(SplashActivity));
                    _activity.StartActivity(intentToStart);
                    _activity.Finish();
                }
            }
        }
    }
}
