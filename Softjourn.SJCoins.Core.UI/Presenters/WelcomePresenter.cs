using Softjourn.SJCoins.Core.Common;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
    public class WelcomePresenter : BasePresenter<IWelcomeView>
    {
        public void DisableWelcomePageOnLaunch() => Settings.FirstLaunch = false;

        public void ToLoginScreen() => NavigationService.NavigateToAsRoot(NavigationPage.Login);
    }
}
