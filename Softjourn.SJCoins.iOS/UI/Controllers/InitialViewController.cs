using System;

using Foundation;
using UIKit;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.iOS.Managers;

namespace Softjourn.SJCoins.iOS.UI.Controllers
{
	public partial class InitialViewController : UIViewController, ILaunchView
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

		public void ToLoginPage()
		{
			//navigate to login page
		}

		public void ToMainPage()
		{
			//navigate to main page
		}

		public void ToWelcomePage()
		{
			//navigate to welcome page
		}
	}
}
