using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Text;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.Presenters.IPresenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Droid.utils;

namespace Softjourn.SJCoins.Droid.ui.activities
{
    [Activity(Theme = "@style/Splash", MainLauncher = true)]
    public class SplashActivity : AppCompatActivity, ILaunchView
    {

        private ILaunchPresenter _presenter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _presenter = new LaunchPresenter(this);
            
            _presenter.ChooseStartPage();
        }

        public void ToWelcomePage()
        {
            NavigationUtils.GoToWelcomeActivity(this);
            Finish();
        }

        public void ToMainPage()
        {
            NavigationUtils.GoToMainActivity(this);
            Finish();
        }

        public void ToLoginPage()
        {
            NavigationUtils.GoToLoginActivity(this);
            Finish();
        }

        public void ShowNoInternetError(string message)
        {
            NavigationUtils.GoToNoInternetScreen(this);
            Finish();
        }
    }
}