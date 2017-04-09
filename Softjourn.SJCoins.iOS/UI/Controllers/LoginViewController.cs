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
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			_scrollService = new KeyboardScrollService(ScrollView);
			_scrollService.AttachToKeyboardNotifications();

			//Hide error labels before view appears
			LoginErrorLabel.Hidden = true;
			PasswordErrorLabel.Hidden = true;

			LoginTextField.Text = "opankiv";
			PasswordTextField.Text = "azKI87987";
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
		}

		public override void DetachEvents()
		{
			BackButton.TouchUpInside -= BackButtonClicked;
			LoginButton.TouchUpInside -= LoginButtonClicked;
			base.DetachEvents();
		}
		#endregion

		#region Private methods
		// -------------------- Event handlers --------------------
		private void BackButtonClicked(object sender, EventArgs e)
		{
			Presenter.ToWelcomePage();
		}

		private void LoginButtonClicked(object sender, EventArgs e)
		{
			Presenter.Login(LoginTextField.Text, PasswordTextField.Text);
		}
		#endregion

		#region UITextFieldDelegate implementation
		private class TextDieldDelegate : UITextFieldDelegate
		{
			private WeakReference parent;

			public TextDieldDelegate(LoginViewController parent)
			{
				this.parent = new WeakReference(parent);
			}

			public override bool ShouldChangeCharacters(UITextField textField, NSRange range, string replacementString)
			{
				return replacementString == " " ? false : true;
			}

			public override bool ShouldReturn(UITextField textField)
			{
				var loginController = parent.Target as LoginViewController;
				if (textField == loginController.LoginTextField)
				{
					loginController.PasswordTextField.BecomeFirstResponder();
				}
				if (textField == loginController.PasswordTextField)
				{
					loginController.PasswordTextField.ResignFirstResponder();
					textField.ReturnKeyType = UIReturnKeyType.Done;
				}
				return true;
			}
			#endregion
		}
	}
}
