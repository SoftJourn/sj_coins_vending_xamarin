using System;
using Autofac;
using BigTed;
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

			LoginTextField.Delegate = new TextDieldDelegate(this);
			PasswordTextField.Delegate = new TextDieldDelegate(this);

			// Add events
			BackButton.TouchUpInside += (sender, e) => { Presenter.ToWelcomePage(); };
			LoginButton.TouchUpInside += (sender, e) => { Presenter.Login(LoginTextField.Text, PasswordTextField.Text); };

			//LoginTextField.ShouldReturn += (sender, e) => { Presenter.ToWelcomePage(); };

		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			_scrollService = new KeyboardScrollService(ScrollView);
			_scrollService.AttachToKeyboardNotifications();

			//Hide error labels before view appears
			LoginErrorLabel.Hidden = true;
			PasswordErrorLabel.Hidden = true;
		}
		#endregion

		#region Native actions implementation
		partial void LoginTextFieldDidChange(UITextField sender)
		{
			LoginErrorLabel.Hidden = true;
			Presenter.IsUserNameValid(LoginTextField.Text);
		}

		partial void PasswordTextFieldDidChange(UITextField sender)
		{
			PasswordErrorLabel.Hidden = true;
			Presenter.IsPasswordValid(PasswordTextField.Text);
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
		}

		public override void AttachEvents()
		{
		}

		public override void DetachEvents()
		{
		}
		#endregion

		#region UITextFieldDelegate implementation
		private class TextDieldDelegate : UITextFieldDelegate
		{
			private LoginViewController parent;

			public TextDieldDelegate(LoginViewController parent)
			{
				this.parent = parent;
			}

			public override bool ShouldChangeCharacters(UITextField textField, NSRange range, string replacementString)
			{
				return replacementString == " " ? false : true;
			}

			public override bool ShouldReturn(UITextField textField)
			{
				if (textField == parent.LoginTextField)
				{
					parent.PasswordTextField.BecomeFirstResponder();
				}
				if (textField == parent.PasswordTextField)
				{
					parent.PasswordTextField.ResignFirstResponder();
					textField.ReturnKeyType = UIReturnKeyType.Done;
				}
				return true;
			}
		}
		#endregion
	}
}
