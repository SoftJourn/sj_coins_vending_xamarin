
using Softjourn.SJCoins.Core.Helpers;
using Softjourn.SJCoins.Core.UI.Presenters.IPresenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Core.Utils;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
    public class LaunchPresenter : BasePresenter, ILaunchPresenter
    {

        private ILaunchView _view;

        public LaunchPresenter(ILaunchView view)
        {
            _view = view;
        }

        public void ChooseStartPage()
        {
            if (NetworkUtils.isConnected)
            {
                if (Settings.FirstLaunch)
                {
                    _view.ToWelcomePage();
                }
                else
                {
                    if (string.IsNullOrEmpty(Settings.AccessToken))
                    {
                        _view.ToLoginPage();
                    }
                    else
                    {
                        _view.ToMainPage();
                    }
                }
            }
            else
            {
                _view.ShowNoInternetError(Resources.StringResources.internet_turned_off);
            }
        }
    }
}
