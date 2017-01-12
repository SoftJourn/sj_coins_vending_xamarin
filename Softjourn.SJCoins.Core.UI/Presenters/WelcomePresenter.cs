
using Softjourn.SJCoins.Core.UI.Presenters.IPresenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
    public class WelcomePresenter : BasePresenter, IWelcomePresenter
    {

        private IWelcomeView _view;

        public WelcomePresenter(IWelcomeView view)
        {
            _view = view;
        }

        public void DisableWelcomePageOnLaunch()
        {
            Helpers.Settings.FirstLaunch = false;
        }

        public void ToLoginScreen()
        {
            _view.ToLoginPage();
        }
    }
}
