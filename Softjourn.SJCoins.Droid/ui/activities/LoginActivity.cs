using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.Presenters.IPresenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Droid.ui.baseUI;
using Softjourn.SJCoins.Droid.utils;


namespace Softjourn.SJCoins.Droid.ui.activities
{
    [Activity(Label = "SomeLabel", Theme = "@style/NoActionBarLoginTheme")]
    public class LoginActivity : BaseActivity, ILoginView
    {

    EditText _userName;

    EditText _passwordText;

    Button _loginButton;

    LinearLayout _linearLayout;

    ImageView _arrowToWelcome;

        private ILoginPresenter _presenter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_login);

            _userName = FindViewById<EditText>(Resource.Id.input_email);
            _passwordText = FindViewById<EditText>(Resource.Id.input_password);
            _loginButton = FindViewById<Button>(Resource.Id.btn_login);
            _linearLayout = FindViewById<LinearLayout>(Resource.Id.login_root);
            _arrowToWelcome = FindViewById<ImageView>(Resource.Id.link_to_welcome_activity);

            _loginButton.Click += LoginButtonOnClick;
            _arrowToWelcome.Click += LinkToWelcomeClick;

            _presenter = new LoginPresenter(this);
        }

        private void LinkToWelcomeClick(object sender, EventArgs e)
        {
            ToWelcomePage();
        }

        private void LoginButtonOnClick(object sender, EventArgs eventArgs)
        {
            Login();
        }

        private void Login()
        {
            var userName = _userName.Text;
            var password = _passwordText.Text;
            _presenter.Login(userName, password);

            //NavigateToMain();
        }

    public override void OnBackPressed()
        {
            MoveTaskToBack(true);
        }

    public override bool OnCreateOptionsMenu(IMenu menu)
        {
            return false;
        }

    public override void ShowSnackBar(string message)
        {

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

    public void ToMainPage()
        {
            _presenter = null;
            //NavigationUtils.GoToVendingActivity(this);
            Finish();
        }

    public void ShowMessage(string message)
        {
            Utils.ShowSnackBar(FindViewById(Resource.Id.login_root), message);
            _userName.StartAnimation(AnimationUtils.LoadAnimation(this, Resource.Animation.shake));
            _passwordText.StartAnimation(AnimationUtils.LoadAnimation(this, Resource.Animation.shake));
        }

    public void ShowNoInternetError(string message)
        {
            OnNoInternetAvailable(message);
        }

        public override void LogOut(IMenuItem item)
        {
            throw new NotImplementedException();
        }

        public void ToWelcomePage()
        {
            NavigationUtils.GoToWelcomeActivity(this);
            Finish();
        }
    }
}