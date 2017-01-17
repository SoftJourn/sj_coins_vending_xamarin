using System;
using Autofac;
using BigTed;
using Foundation;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers
{
	[Register("LoginViewController")]
	public partial class LoginViewController : BaseViewController, ILoginView
	{
		//Properties
		private LoginPresenter _loginPresenter;
		private KeyboardScrollService _scrollService;

		//Constructor
		public LoginViewController(IntPtr handle) : base(handle)
		{
		}

		//Life cycle
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			//Resolve LoginPresenter from container and atach this view
			_loginPresenter = _scope.Resolve<LoginPresenter>();
			_loginPresenter.AttachView(this);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			//Hide error labels before view appears
			LoginErrorLabel.Hidden = true;
			PasswordErrorLabel.Hidden = true;

			//Set this view controller when visible
			currentApplication.VisibleViewController = this;
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

		#region ILoginView implementation
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

		public void ShowProgress(string message)
		{
			BTProgressHUD.Show(message);
		}

		public void HideProgress()
		{
			BTProgressHUD.Dismiss();
		}

		public void ShowNoInternetError(string message)
		{
			//show no internet alert
			//new AlertManager().PresentAlert(message);
		}
		#endregion

		public void AttachEvents()
		{
			
		}

		public void DetachEvents()
		{
			
		}
	}
}
