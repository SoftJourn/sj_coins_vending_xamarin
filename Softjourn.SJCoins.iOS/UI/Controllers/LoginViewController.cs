using Foundation;
using System;
using UIKit;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Core.UI.Presenters;

namespace Softjourn.SJCoins.iOS.UI.Controllers
{
	public partial class LoginViewController : UIViewController, ILoginView
	{
		//Properties
		private LoginPresenter _loginPresenter;

		//Constructor
		public LoginViewController(IntPtr handle) : base(handle)
		{
		}
	
		//Life cycle
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			//Create Presenter
			_loginPresenter = new LoginPresenter(this);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			//Verify if its a first launch
			_loginPresenter.CheckFirstLaunch();

			//Hide error labels before view appears
			loginErrorLabel.Hidden = true;
			passwordErrorLabel.Hidden = true;
		}

		//Actions
		partial void LoginButton_TouchUpInside(UIButton sender)
		{
			_loginPresenter.Login(loginTextField.Text, passwordTexField.Text);
		}

		//ILoginView Interface
		public void HideProgress()
		{
			
		}

		public void NavigateToMain()
		{
			throw new NotImplementedException();
		}

		public void NavigateToWelcome()
		{
			Console.WriteLine("Navigated to welcome");
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