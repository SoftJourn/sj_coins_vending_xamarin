using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Softjourn.SJCoins.Core.UI.Presenters.IPresenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
    public class LaunchPresenter : ILaunchPresenter
    {

        private ILaunchView _view;

        public LaunchPresenter(ILaunchView view)
        {
            _view = view;
        }

        public void ChooseStartPage()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (Helpers.Settings.FirstLaunch)
                {
                    _view.ToWelcomePage();
                }
                else
                {
                    if (string.IsNullOrEmpty(Helpers.Settings.AccessToken))
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
                _view.NoInternet();
            }
        }
    }
}
