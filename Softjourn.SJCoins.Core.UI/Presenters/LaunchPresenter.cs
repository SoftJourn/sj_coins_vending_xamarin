
using Softjourn.SJCoins.Core.Helpers;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.Presenters.IPresenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Core.Utils;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
    public class LaunchPresenter : BasePresenter<ILaunchView>
    {
		SJCoins.iOS.UI.Controllers.InitialViewController initialViewController;

		public LaunchPresenter()
        {

        }

		public LaunchPresenter(SJCoins.iOS.UI.Controllers.InitialViewController initialViewController)
		{
			this.initialViewController = initialViewController;
		}

		public void ChooseStartPage()
        {
            if (NetworkUtils.isConnected)
            {
                if (Settings.FirstLaunch)
                {
                    View.ToWelcomePage();
                }
                else
                {
                    if (string.IsNullOrEmpty(Settings.AccessToken))
                    {
                        View.ToLoginPage();
                    }
                    else
                    {
                        View.ToMainPage();
                    }
                }
            }
            else
            {
                View.ShowNoInternetError(Resources.StringResources.internet_turned_off);
            }
        }
    }
}
