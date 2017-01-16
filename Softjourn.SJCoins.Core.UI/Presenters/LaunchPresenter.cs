
using System;
using Softjourn.SJCoins.Core.Helpers;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.Presenters.IPresenters;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Core.Utils;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
    public class LaunchPresenter : BasePresenter<ILaunchView>, INetworkConnection
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
                    NavigationService.NavigateToAsRoot(NavigationPage.Welcome);
                }
                else
                {
                    if (string.IsNullOrEmpty(Settings.AccessToken))
                    {
                        NavigationService.NavigateToAsRoot(NavigationPage.Login);
                    }
                    else
                    {
                        NavigationService.NavigateToAsRoot(NavigationPage.Main);
                    }
                }
            }
            else
            {
                View.ShowNoInternetError(Resources.StringResources.internet_turned_off);
                NetworkUtils.onConnectionChanged(this);
            }
        }

        public void OnInternetAppeared()
        {
            ChooseStartPage();
        }

        public void OnInternetDismissed()
        {
            // nothing
        }
    }
}
