using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Softjourn.SJCoins.Core.API.Model.AccountInfo;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Droid.ui.baseUI;

namespace Softjourn.SJCoins.Droid.UI.Activities
{
    [Activity(Label = "Profile", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ProfileActivity : BaseActivity<AccountPresenter>, IAccountView

    {
        private TextView _username;
        private TextView _balance;
        private ListView _options;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_profile);

            _username = FindViewById<TextView>(Resource.Id.profile_user_name);
            _balance = FindViewById<TextView>(Resource.Id.profile_amount_available);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            _options = FindViewById<ListView>(Resource.Id.profile_more_options);
            //var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, ViewPresenter.GetOptionsList());
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            return false;
        }

        public void SetAccountInfo(Account account)
        {
            _username.Text = account.Name + " " + account.Surname;
            _balance.Text = account.Amount.ToString();
        }
    }
}