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
using Softjourn.SJCoins.Droid.ui.baseUI;
using TrololoWorld.utils;
using String = System.String;

namespace Softjourn.SJCoins.Droid.ui.activities
{
    [Activity(Label = "SomeLabel", MainLauncher = true, Theme = "@style/NoActionBarLoginTheme")]
    public class LoginActivity : BaseActivity
    {

    EditText mUserName;

    EditText mPasswordText;

    Button mLoginButton;

    LinearLayout mLinearLayout;

    ImageView mArrowToWelcome;

        //private LoginContract.Presenter mPresenter;

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

            if (Preferences.RetrieveBooleanObject(Const.IsFirstTimeLaunch))
            {
                //NavigationUtils.GoToWelcomeActivity(this);
                //Finish();
            }
        }

        private void MLinkToWelcomeClick(object sender, EventArgs e)
        {
            //NavigationUtils.GoToWelcomeActivity(this);
            Finish();
        }

        private void MLoginButtonOnClick(object sender, EventArgs eventArgs)
        {
            Login();
        }

        private void Login()
        {
            var userName = mUserName.Text;
            var password = mPasswordText.Text;
            //mPresenter.login(userName, password);

            NavigateToMain();
        }

    public override void OnBackPressed()
        {
            MoveTaskToBack(true);
        }

    public override bool OnCreateOptionsMenu(IMenu menu)
        {
            return false;
        }

    public new void ShowProgress(string message)
        {
            base.ShowProgress(message);
        }

    public override void ShowSnackBar(string message)
        {

        }

    public new void HideProgress()
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


    public void ShowToastMessage(string message)
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
    }
}