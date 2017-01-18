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
		//Properties

		#region Constructor
		public InitialViewController (IntPtr handle) : base (handle)
		{
		}
		#endregion

		#region Controller Life cycle 
		public void ViewDidLoad()
		{
			base.ViewDidLoad();
		}

		public void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			//Set this view controller when visible
			currentApplication.VisibleViewController = this;

			//Verify if its a first launch
			Presenter.ChooseStartPage();
		}

		protected void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			//Detach this view to presentera
			Presenter.DetachView();
		}
		#endregion

		#region ILaunchView implementation
		public void ShowNoInternetError(string msg)
		{
			//show no internet alert
			new AlertService().ShowInformationDialog(null, msg, "Ok", null);
		}
		#endregion

		#region Private methods

		#endregion
	}
}
