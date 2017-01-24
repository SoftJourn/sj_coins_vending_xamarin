using Android.App;
using Android.OS;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Droid.ui.baseUI;

namespace Softjourn.SJCoins.Droid.UI.Activities
{
    [Activity(Theme = "@style/Splash", MainLauncher = true)]
    public class SplashActivity : BaseActivity<LaunchPresenter>, ILaunchView
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ViewPresenter.ChooseStartPage();
        }

        public void ShowNoInternetError(string message)
        {
            SetContentView(Resource.Layout.activity_no_internet);
        }
    }
}
