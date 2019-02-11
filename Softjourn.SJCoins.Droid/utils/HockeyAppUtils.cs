using Android.Content;
using HockeyApp.Android;

namespace Softjourn.SJCoins.Droid.Utils
{
    public class HockeyAppUtils
    {
        public static void CheckForCrashes(Context ctx)
        {
            CrashManager.Register(ctx, "2b3c1a56da2647a798a84f6ff34ac180");
        }

        public static void UnregisterManagers()
        {
            UpdateManager.Unregister();
        }
    }
}