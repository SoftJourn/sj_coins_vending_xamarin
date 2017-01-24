using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Droid.ui.baseUI;


namespace Softjourn.SJCoins.Droid.UI.Activities
{
    [Activity(Label = "SomeLabel", Theme = "@style/NoActionBarLoginTheme")]
    public class LoginActivity : BaseActivity<LoginPresenter>, ILoginView
    {

        EditText _userName;

        EditText _passwordText;

        Button _loginButton;

        ImageView _arrowToWelcome;

        //private ILoginPresenter _presenter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_login);

            _userName = FindViewById<EditText>(Resource.Id.input_email);
            _passwordText = FindViewById<EditText>(Resource.Id.input_password);
            _loginButton = FindViewById<Button>(Resource.Id.btn_login);
            _arrowToWelcome = FindViewById<ImageView>(Resource.Id.link_to_welcome_activity);

            _userName.TextChanged += (s, e) =>
            {
                if (!_userName.Text.EndsWith("-") || _userName.Text.EndsWith("_"))
                {
                    ViewPresenter.IsUserNameValid(_userName.Text);
                }
            };

            _loginButton.Click += LoginButtonOnClick;
            _arrowToWelcome.Click += LinkToWelcomeClick;

            //_presenter = new LoginPresenter(this);
        }

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

        public override void OnBackPressed()
        {
            MoveTaskToBack(true);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            return false;
        }

        public void SetUsernameError(string message)
        {
            _userName.RequestFocus();
            _userName.SetError(message, null);
        }

        public void SetPasswordError(string message)
        {
            _passwordText.RequestFocus();
            _passwordText.SetError(message, null);
        }

        public void ShowNoInternetError(string message)
        {
            //OnNoInternetAvailable(message);
        }

        public void AttachEvents()
        {
            throw new NotImplementedException();
        }

        public void DetachEvents()
        {
            throw new NotImplementedException();
        }
    }
}