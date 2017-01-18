using Android.App;
using Android.OS;
using Android.Views;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Droid.ui.baseUI;
using Softjourn.SJCoins.Droid.utils;

namespace Softjourn.SJCoins.Droid.ui.activities
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

        public override void ShowSnackBar(string message)
        {
            throw new System.NotImplementedException();
        }

        public override void LogOut(IMenuItem item)
        {
            throw new System.NotImplementedException();
        }
    }
}
