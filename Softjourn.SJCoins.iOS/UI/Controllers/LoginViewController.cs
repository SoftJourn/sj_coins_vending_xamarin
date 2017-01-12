using System;
using BigTed;
using Foundation;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.iOS.Managers;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers
{
	[Register("LoginViewController")]
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

			//Hide error labels before view appears
			LoginErrorLabel.Hidden = true;
			PasswordErrorLabel.Hidden = true;
		}
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			_loginPresenter = null;
		}

		//Actions
		partial void LoginButtonPressed(UIButton sender)
		{
			_loginPresenter.Login(LoginTextField.Text, PasswordTextField.Text);
		}

		partial void LoginTextFieldDidChange(UITextField sender)
		{
			//...
		}

		//ILoginView Interface
		public void SetUsernameError(string message)
		{
			LoginErrorLabel.Text = message;
			LoginErrorLabel.Hidden = false;
		}

		public void SetPasswordError(string message)
		{
			// show passwordLabel error
			PasswordErrorLabel.Text = message;
			PasswordErrorLabel.Hidden = false;
		}

		public void ToMainPage()
		{
			// navigate to home page
		}

		public void ToWelcomePage()
		{
			// navigate to welcome page
			Console.WriteLine("Navigated to welcome");
		}

		public void ShowProgress(string message)
		{
			BTProgressHUD.Show(message);
		}

		public void HideProgress()
		{
			BTProgressHUD.Dismiss();
		}

		public void ShowMessage(string message)
		{
			// show login faliled alert
			new AlertManager().PresentAlert(message);
		}

		public void ShowNoInternetError(string message)
		{
			//show no internet alert
			new AlertManager().PresentAlert(message);
		}
	}
}
