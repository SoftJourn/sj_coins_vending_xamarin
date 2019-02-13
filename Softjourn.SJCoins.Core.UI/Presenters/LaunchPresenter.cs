using Softjourn.SJCoins.Core.Common;
using Softjourn.SJCoins.Core.Common.Utils;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
    public class LaunchPresenter : BasePresenter<ILaunchView>, INetworkConnection
    {
        public LaunchPresenter() { }

        public void ChooseStartPage()
        {
            if (NetworkUtils.IsConnected)
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
                        if (string.IsNullOrEmpty(Settings.SelectedMachineId))
                        {
                            NavigationService.NavigateToAsRoot(NavigationPage.SelectMachineFirstTime);
                        }
                        else
                        {
                            NavigationService.NavigateToAsRoot(NavigationPage.Home);
                        }
                    }
                }
            }
            else
            {
                View.ShowNoInternetError(Resources.UiMessageResources.internet_turned_off);
                NetworkUtils.OnConnectionChanged(this);
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
