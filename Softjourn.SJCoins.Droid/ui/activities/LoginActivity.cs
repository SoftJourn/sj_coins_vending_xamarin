using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Droid.ui.baseUI;


namespace Softjourn.SJCoins.Droid.UI.Activities
{
    [Activity(Label = "SomeLabel", Theme = "@style/NoActionBarLoginTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class LoginActivity : BaseActivity<LoginPresenter>, ILoginView
    {

        EditText _userName;

        EditText _passwordText;

        Button _loginButton;

        ImageView _arrowToWelcome;

        #region Standart Activity Methods
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_login);

            _userName = FindViewById<EditText>(Resource.Id.input_email);
            _passwordText = FindViewById<EditText>(Resource.Id.input_password);
            _loginButton = FindViewById<Button>(Resource.Id.btn_login);
            _arrowToWelcome = FindViewById<ImageView>(Resource.Id.link_to_welcome_activity);

            _loginButton.Click += LoginButtonOnClick;
            _arrowToWelcome.Click += LinkToWelcomeClick;
        }

        public override void OnBackPressed()
        {
            MoveTaskToBack(true);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            return false;
        }
        #endregion

        #region ILoginView Methods
        /**
         * Sets Error to username field with the given message
         */
        public void SetUsernameError(string message)
        {
            _userName.RequestFocus();
            _userName.SetError(message, null);
        }

        /**
         * Sets Error to password field with the given message
         */
        public void SetPasswordError(string message)
        {
            _passwordText.RequestFocus();
            _passwordText.SetError(message, null);
        }
        #endregion

        #region Private Methods
        private void LinkToWelcomeClick(object sender, EventArgs e)
        {
            ViewPresenter.ToWelcomePage();
        }

        private void LoginButtonOnClick(object sender, EventArgs eventArgs)
        {
            Login();
        }

        private void Login()
        {
            var userName = _userName.Text;
            var password = _passwordText.Text;
            ViewPresenter.Login(userName, password);
        }
        #endregion
    }
}