using System;
using Autofac;
using BigTed;
using Foundation;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.iOS.UI.Services;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers
{
	[Register("LoginViewController")]
	public partial class LoginViewController : BaseViewController<LoginPresenter>, ILoginView
	{
		#region Properties
		private LoginPresenter _loginPresenter;
		private KeyboardScrollService _scrollService;
		#endregion

		#region Constructor
		public LoginViewController(IntPtr handle) : base(handle)
		{
		}
		#endregion

		#region Controller Life cycle 
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			//Set this view controller when visible
			currentApplication.VisibleViewController = this;

			//Hide error labels before view appears
			LoginErrorLabel.Hidden = true;
			PasswordErrorLabel.Hidden = true;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			Presenter = null;
		}
		#endregion

		//Actions
		partial void LoginButtonPressed(UIButton sender)
		{
			Presenter.Login(LoginTextField.Text, PasswordTextField.Text);
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
			new AlertService().ShowInformationDialog(null, message, "Ok", null);
		}
		#endregion

		#region BaseViewController -> IBaseView implementation
		public override void SetUIAppearance()
		{
			base.SetUIAppearance();
			//_scrollService = new KeyboardScrollService(ScrollView);
		}

		public override void AttachEvents()
		{
			
		}

		public override void DetachEvents()
		{
			
		}
		#endregion
	}
}
