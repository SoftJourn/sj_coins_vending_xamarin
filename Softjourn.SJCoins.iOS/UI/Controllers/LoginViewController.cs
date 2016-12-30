using Foundation;
using System;
using UIKit;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.iOS.Managers;
using MBProgressHUD;

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
		public void SetUsernameError(string message)
		{
			throw new NotImplementedException();
		}

		public void SetPasswordError(string message)
		{
			throw new NotImplementedException();
		}

		public void NavigateToMain()
		{
			// navigate to home page
		}

		public void NavigateToWelcome()
		{
			// navigate to welcome page
			Console.WriteLine("Navigated to welcome");
		}

		public void ShowProgress(string message)
		{
			throw new NotImplementedException();
		}

		public void HideProgress()
		{

		}

		public void ShowMessage(string message)
		{
			throw new NotImplementedException();
		}

		public void ShowNoInternetError(string message)
		{
			//show no internet alert
			new AlertManager().PresentAlert("No internet connection.");
		}
		}
	}
}