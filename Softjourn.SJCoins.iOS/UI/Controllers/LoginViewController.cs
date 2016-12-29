using Foundation;
using System;
using UIKit;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Core.UI.Presenters;

namespace Softjourn.SJCoins.iOS.UI.Controllers
{
	public partial class LoginViewController : UIViewController, ILoginView
	{
		private LoginPresenter _loginPresenter;

		//Constructor
		public LoginViewController(IntPtr handle) : base(handle)
		{
		}
	
		//Life cyclee
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			_loginPresenter = new LoginPresenter(this);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			loginErrorLabel.Hidden = true;
			passwordErrorLabel.Hidden = true;
		}

		//Actions
		partial void LoginButton_TouchUpInside(UIButton sender)
		{
			Console.WriteLine("sjdhfjkshgjksdhgjksdhgjkhdsjkghdsjkghdjksghjk sdgsdhgjkdhsgjk");
		}

		//ILoginView Interface
		public void HideProgress()
		{
			throw new NotImplementedException();
		}

		public void NavigateToMain()
		{
			throw new NotImplementedException();
		}

		public void NavigateToWelcome()
		{
			throw new NotImplementedException();
		}

		public void SetPasswordError()
		{
			throw new NotImplementedException();
		}

		public void SetUsernameError()
		{
			throw new NotImplementedException();
		}

		public void ShowMessage(string message)
		{
			throw new NotImplementedException();
		}

		public void ShowNoInternetError()
		{
			UIAlertController alert = UIAlertController.Create("Warning", "No internet connecton.", UIAlertControllerStyle.Alert);
			alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Cancel, null));
			this.PresentViewController(alert, animated: true, completionHandler: null);
		}

		public void ShowProgress(string message)
		{
			throw new NotImplementedException();
		}
	}
}