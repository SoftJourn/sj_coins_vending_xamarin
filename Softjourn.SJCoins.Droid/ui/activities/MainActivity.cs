using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Droid.ui.baseUI;

namespace Softjourn.SJCoins.Droid.ui.activities
{
    public class MainActivity : BaseActivity<LoginPresenter>, ILoginView
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
        }

        public override void ShowSnackBar(string message)
        {
            throw new NotImplementedException();
        }

        public override void LogOut(IMenuItem item)
        {
            throw new NotImplementedException();
        }

        public void SetUsernameError(string message)
        {
            throw new NotImplementedException();
        }

        public void SetPasswordError(string message)
        {
            throw new NotImplementedException();
        }

        public void ToMainPage()
        {
            throw new NotImplementedException();
        }

        public void ToWelcomePage()
        {
            throw new NotImplementedException();
        }

        public void ShowMessage(string message)
        {
            throw new NotImplementedException();
        }

        public void ShowNoInternetError(string message)
        {
            throw new NotImplementedException();
        }
    }
}