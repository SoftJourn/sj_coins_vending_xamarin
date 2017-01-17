using System;

using Foundation;
using UIKit;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Core.UI.Presenters;
using Autofac;

namespace Softjourn.SJCoins.iOS.UI.Controllers
{
	[Register("InitialViewController")]
	public partial class InitialViewController : BaseViewController, ILaunchView
	{
		//Properties
		private LaunchPresenter _launchPresenter;

		//Constructor
		public InitialViewController (IntPtr handle) : base (handle)
		{
		}

		//Life cycle
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			//Resolve LaunchPresenter from container and atach this view
			_launchPresenter = _scope.Resolve<LaunchPresenter>();
			_launchPresenter.AttachView(this);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			//Set this view controller when visible
			currentApplication.VisibleViewController = this;

			//Verify if its a first launch
			_launchPresenter.ChooseStartPage();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			//_launchPresenter.DetachView();
		}

		#region ILaunchView implementation
		public void ShowNoInternetError(string msg)
		{
			//show no internet alert
			//new AlertManager().PresentAlert(msg);
		}
		#endregion
	}
}
