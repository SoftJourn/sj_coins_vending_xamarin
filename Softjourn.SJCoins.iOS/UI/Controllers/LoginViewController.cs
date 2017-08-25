using System;
using Autofac;
using Foundation;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.iOS.Services;
using Softjourn.SJCoins.iOS.UI.Services;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers
{
	[Register("LoginViewController")]
	public partial class LoginViewController : BaseViewController<LoginPresenter>, ILoginView
	{
		#region Properties
		private KeyboardScrollService _scrollService;
        private LoginPageTextFieldsDelegate loginTextFieldDelegate = new LoginPageTextFieldsDelegate();
		private LoginPageTextFieldsDelegate passwordTextFieldDelegate = new LoginPageTextFieldsDelegate();
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

            LoginTextField.Delegate = loginTextFieldDelegate;
			PasswordTextField.Delegate = passwordTextFieldDelegate;
			_scrollService = new KeyboardScrollService(ScrollView);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			//Hide error labels before view appears
			LoginErrorLabel.Hidden = true;
			PasswordErrorLabel.Hidden = true;
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
			_scrollService = null;
			loginTextFieldDelegate = null;
			passwordTextFieldDelegate = null;
		}
		#endregion

		#region Native actions implementation
		partial void LoginTextFieldDidChange(UITextField sender)
		{
			LoginErrorLabel.Hidden = true;
		}

		partial void PasswordTextFieldDidChange(UITextField sender)
		{
			PasswordErrorLabel.Hidden = true;
		}
		#endregion

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

		public void ShowNoInternetError(string message)
		{
			//show no internet alert
			new AlertService().ShowInformationDialog(null, message, "Ok", null);
		}
		#endregion

		#region BaseViewController -> IBaseView implementation
		public override void AttachEvents()
		{
			base.AttachEvents();
			BackButton.TouchUpInside += BackButtonClicked;
			LoginButton.TouchUpInside += LoginButtonClicked;
            loginTextFieldDelegate.ShouldReturnEvent += TextFieldShouldReturn;
			passwordTextFieldDelegate.ShouldReturnEvent += TextFieldShouldReturn;
			_scrollService.AttachToKeyboardNotifications();
		}

		public override void DetachEvents()
		{
			BackButton.TouchUpInside -= BackButtonClicked;
			LoginButton.TouchUpInside -= LoginButtonClicked;
            loginTextFieldDelegate.ShouldReturnEvent -= TextFieldShouldReturn; 
            passwordTextFieldDelegate.ShouldReturnEvent -= TextFieldShouldReturn;
            _scrollService.DetachToKeyboardNotifications();
			base.DetachEvents();
		}
		#endregion 

		#region Event handlers
		private void BackButtonClicked(object sender, EventArgs e)
		{
			Presenter.ToWelcomePage();
		}

		private void LoginButtonClicked(object sender, EventArgs e)
		{
			Presenter.Login(LoginTextField.Text, PasswordTextField.Text);
		}

        private void TextFieldShouldReturn(object sender, UITextField textField)
		{
			if (textField == LoginTextField)
			{
			  PasswordTextField.BecomeFirstResponder();
			}
			if (textField == PasswordTextField)
			{
			  PasswordTextField.ResignFirstResponder();
			  textField.ReturnKeyType = UIReturnKeyType.Done;
			}
		}
		#endregion 
	}
}
