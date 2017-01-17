using System;

using Foundation;
using UIKit;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.iOS.Managers;

namespace Softjourn.SJCoins.iOS.UI.Controllers
{
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

			//Create Presenter
			_launchPresenter = new LaunchPresenter();
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

			_launchPresenter.DetachView();
		}

		//ILaunchView Interface
		public void ShowNoInternetError(string msg)
		{
			//show no internet alert
			new AlertManager().PresentAlert(msg);
		}
	}
}
