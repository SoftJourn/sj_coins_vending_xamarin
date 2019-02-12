using System;
using Foundation;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.iOS.UI.Services;

namespace Softjourn.SJCoins.iOS.UI.Controllers
{
    [Register("InitialViewController")]
    public partial class InitialViewController : BaseViewController<LaunchPresenter>, ILaunchView
    {
        public InitialViewController(IntPtr handle) : base(handle)
        {
        }

        #region Controller Life cycle 

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            //Verify if its a first launch
            Presenter.ChooseStartPage();
        }

        #endregion

        #region ILaunchView implementation

        public void ShowNoInternetError(string msg)
        {
            //show no internet alert
            new AlertService().ShowInformationDialog(null, msg, "Ok", null);
        }

        #endregion
    }
}
