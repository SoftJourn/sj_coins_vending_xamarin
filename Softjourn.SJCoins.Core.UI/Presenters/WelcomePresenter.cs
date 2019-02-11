using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
    public class WelcomePresenter : BasePresenter<IWelcomeView>
    {
        public WelcomePresenter() { }

        public void DisableWelcomePageOnLaunch()
        {
            Helpers.Settings.FirstLaunch = false;
        }

        public void ToLoginScreen()
        {
            NavigationService.NavigateToAsRoot(NavigationPage.Login);
        }
    }
}
