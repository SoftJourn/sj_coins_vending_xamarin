using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Java.Lang;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.Presenters.IPresenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Droid.ui.baseUI;
using Softjourn.SJCoins.Droid.utils;
using String = System.String;


namespace Softjourn.SJCoins.Droid.ui.activities
{
    [Activity(Label = "SomeLabel", MainLauncher = true, Theme = "@style/NoActionBarLoginTheme")]
    public class LoginActivity : BaseActivity, ILoginView
    {

    EditText mUserName;

    EditText mPasswordText;

    Button mLoginButton;

    LinearLayout mLinearLayout;

    ImageView mArrowToWelcome;

        private ILoginPresenter _presenter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_login);

            mUserName = FindViewById<EditText>(Resource.Id.input_email);
            mPasswordText = FindViewById<EditText>(Resource.Id.input_password);
            mLoginButton = FindViewById<Button>(Resource.Id.btn_login);
            mLinearLayout = FindViewById<LinearLayout>(Resource.Id.login_root);
            mArrowToWelcome = FindViewById<ImageView>(Resource.Id.link_to_welcome_activity);

            mLoginButton.Click += MLoginButtonOnClick;
            mArrowToWelcome.Click += MLinkToWelcomeClick;

            _presenter = new LoginPresenter(this);
        }

        private void MLinkToWelcomeClick(object sender, EventArgs e)
        {
            NavigateToWelcome();
        }

        private void MLoginButtonOnClick(object sender, EventArgs eventArgs)
        {
            Login();
        }

        private void Login()
        {
            var userName = mUserName.Text;
            var password = mPasswordText.Text;
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

    public override void ShowProgress(string message)
        {
            base.ShowProgress(message);
        }

    public override void ShowSnackBar(string message)
        {

        }

    public override void HideProgress()
        {
            base.HideProgress();
        }

    public void SetUsernameError()
        {
            mUserName.RequestFocus();
            mUserName.SetError(GetString(Resource.String.activity_login_invalid_email), null);
        }

    public void SetPasswordError()
        {
            mPasswordText.RequestFocus();
            mPasswordText.SetError(GetString(Resource.String.activity_login_invalid_password),null);
        }

    public void NavigateToMain()
        {
            //mPresenter.onDestroy();
            //mPresenter = null;
            //NavigationUtils.GoToVendingActivity(this);
            Finish();
        }

    public void ShowMessage(string message)
        {
            Utils.ShowSnackBar(FindViewById(Resource.Id.login_root), message);
            mUserName.StartAnimation(AnimationUtils.LoadAnimation(this, Resource.Animation.shake));
            mPasswordText.StartAnimation(AnimationUtils.LoadAnimation(this, Resource.Animation.shake));
        }

    public void ShowNoInternetError()
        {
            OnNoInternetAvailable();
        }

        public override void LogOut(IMenuItem item)
        {
            throw new NotImplementedException();
        }

        public void NavigateToWelcome()
        {
            NavigationUtils.GoToWelcomeActivity(this);
            Finish();
        }
    }
}