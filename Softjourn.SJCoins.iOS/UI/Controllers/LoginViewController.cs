using System;
using Foundation;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.iOS.General.Constants;
using Softjourn.SJCoins.iOS.Services;
using Softjourn.SJCoins.iOS.UI.Services;
using UIKit;
using KeyChain.Net.XamarinIOS;

namespace Softjourn.SJCoins.iOS.UI.Controllers
{
	[Register("LoginViewController")]
	public partial class LoginViewController : BaseViewController<LoginPresenter>, ILoginView
	{
        public const string LoginKey = "sjcoins_login";
        public const string PasswordKey = "sjcoins_password";

		#region Properties
        private KeyboardScrollService scrollService;
        private TouchIDService touchIDService;
        private KeyChainHelper keychainHelper;
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
            ConfigurePage();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			//Hide error labels before view appears
			LoginErrorLabel.Hidden = true;
			PasswordErrorLabel.Hidden = true;

            UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.Default, false);

            MakeNavigationBarTransparent();
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
            scrollService = null;
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
            BackHelpButton.Clicked += BackButtonClicked;
			LoginButton.TouchUpInside += LoginButtonClicked;
            TouchIDButton.TouchUpInside += TouchIDButtonClicked;
            loginTextFieldDelegate.ShouldReturnEvent += TextFieldShouldReturn;
			passwordTextFieldDelegate.ShouldReturnEvent += TextFieldShouldReturn;
            touchIDService.AccessGranted += TouchIDAccessGranted;
            touchIDService.AccessDenied += TouchIDAccessDenied;
            scrollService.AttachToKeyboardNotifications();
		}

		public override void DetachEvents()
		{
            BackHelpButton.Clicked -= BackButtonClicked;
			LoginButton.TouchUpInside -= LoginButtonClicked;
            TouchIDButton.TouchUpInside -= TouchIDButtonClicked;
            loginTextFieldDelegate.ShouldReturnEvent -= TextFieldShouldReturn; 
            passwordTextFieldDelegate.ShouldReturnEvent -= TextFieldShouldReturn;
            touchIDService.AccessGranted -= TouchIDAccessGranted;
            touchIDService.AccessDenied -= TouchIDAccessDenied;
            scrollService.DetachToKeyboardNotifications();
			base.DetachEvents();
		}
        #endregion

        #region Private methods
        private void ConfigurePage()
        {
            LoginTextField.Delegate = loginTextFieldDelegate;
            PasswordTextField.Delegate = passwordTextFieldDelegate;

            var buttonLocation = LoginButton.Frame.Location;
            scrollService = new KeyboardScrollService(ScrollView, buttonLocation, View.Frame);

            touchIDService = new TouchIDService();
            keychainHelper = new KeyChainHelper();

            if (touchIDService.CanEvaluatePolicy())
            {
                if (FirstLogin() || NoStoredCredentials())
                {
                    TouchIDButton.TintColor = UIColor.LightGray;
                    TouchIDButton.Enabled = false;
                }

                if (!NoStoredCredentials())
                {
                    TouchIDButton.TintColor = UIColorConstants.MainGreenColor;
                    TouchIDButton.Enabled = true;

                    LoginTextField.Hidden = true;
                    PasswordTextField.Hidden = true;
                    LoginButton.Hidden = true;
                    TouchIDButton.Hidden = true;
                    touchIDService.AuthenticateUser();
                }
            }
            else
                TouchIDButton.Hidden = true;
        }

        private bool FirstLogin() => !NSUserDefaults.StandardUserDefaults.BoolForKey(Const.FIRSTLOGIN);

        private void ProposeStoreCredentials() 
        {
            Action ok = () => {
                keychainHelper.DeleteKey(LoginKey);
                keychainHelper.DeleteKey(PasswordKey);
                keychainHelper.SetKey(LoginKey, LoginTextField.Text);
                keychainHelper.SetKey(PasswordKey, PasswordTextField.Text);
                LogIn();
            };
            Action cancel = LogIn;

            new AlertService().ShowConfirmationAlert("Title", "Message", ok, cancel);
        }

        private void LogIn()
        {
            Presenter.Login(LoginTextField.Text, PasswordTextField.Text);
        }

        private bool AnotherCredentialsUsed() => !LoginTextField.Text.Equals(Retreive(LoginKey)) || !PasswordTextField.Text.Equals(Retreive(PasswordKey));

        private bool NoStoredCredentials() => string.IsNullOrEmpty(Retreive(LoginKey)) || string.IsNullOrEmpty(Retreive(PasswordKey));
                   
        private string Retreive(string key) => keychainHelper.GetKey(key);

        protected override void ShowAnimated(bool loadSuccess)
        {
            LoginTextField.Hidden = false;
            PasswordTextField.Hidden = false;
            LoginButton.Hidden = false;
            TouchIDButton.Hidden = false;
        }
        #endregion

        #region Event handlers
        private void BackButtonClicked(object sender, EventArgs e)
		{
			Presenter.ToWelcomePage();
		}

		private void LoginButtonClicked(object sender, EventArgs e)
		{
            if (touchIDService.CanEvaluatePolicy())
            {
                if (FirstLogin() || AnotherCredentialsUsed())
                    ProposeStoreCredentials();
                else
                    LogIn();
            }
            else
                LogIn();
		}

        private void TextFieldShouldReturn(object sender, UITextField textField)
		{
			if (textField == LoginTextField)
                PasswordTextField.BecomeFirstResponder();
			
            if (textField == PasswordTextField) {
			    PasswordTextField.ResignFirstResponder();
			    textField.ReturnKeyType = UIReturnKeyType.Done;
			}
		}

        // TouchID service
        private void TouchIDButtonClicked(object sender, EventArgs e)
        {
            // If credentials stored ->
            touchIDService.AuthenticateUser();
        }

        private void TouchIDAccessGranted(object sender, EventArgs e)
        {
            // Take credentials from KeyChain and perform login.
            InvokeOnMainThread(() =>
            {
                var login = Retreive(LoginKey);
                var password = Retreive(PasswordKey);
                LoginTextField.Text = login;
                PasswordTextField.Text = password;
                Presenter.Login(login, password);
            });
        }

        private void TouchIDAccessDenied(object sender, NSError error)
        {
            if (error.LocalizedDescription.Equals("Canceled by user.")) 
            {
                InvokeOnMainThread(() =>
                {
                    ShowAnimated(true);
                });
            }
        }
		#endregion 
  	}
}
