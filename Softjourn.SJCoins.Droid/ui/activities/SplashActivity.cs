using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Text;
using Softjourn.SJCoins.Droid.utils;

namespace Softjourn.SJCoins.Droid.ui.activities
{
    [Activity(Theme = "@style/Splash", MainLauncher = true)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            if (!NetworkUtils.IsNetworkEnabled())
            {
                NavigationUtils.GoToNoInternetScreen(this);
                Finish();
            }
            else
            {
                if (TextUtils.IsEmpty(Preferences.RetrieveStringObject(Const.AccessToken))
                        && TextUtils.IsEmpty(Preferences.RetrieveStringObject(Const.RefreshToken)))
                {
                    NavigationUtils.GoToLoginActivity(this);
                    Finish();
                }
                else
                {
                    //NavigationUtils.GoToVendingActivity(this);
                    Finish();
                }
            }
        }
    }
}