using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using Java.Lang;
using Plugin.Permissions;
using Softjourn.SJCoins.Core.API.Model.AccountInfo;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Droid.ui.baseUI;
using Console = System.Console;

namespace Softjourn.SJCoins.Droid.UI.Activities
{
    [Activity(Label = "Profile", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ProfileActivity : BaseActivity<AccountPresenter>, IAccountView

    {
        private TextView _username;
        private TextView _balance;
        private ListView _options;
        private ImageView _avatar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_profile);

            _username = FindViewById<TextView>(Resource.Id.profile_user_name);
            _balance = FindViewById<TextView>(Resource.Id.profile_amount_available);
            _avatar = FindViewById<ImageView>(Resource.Id.profile_avatar);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            _avatar.Click += ChangePhoto;

            _options = FindViewById<ListView>(Resource.Id.profile_more_options);
            var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, ViewPresenter.GetOptionsList());

            ViewPresenter.OnStartLoadingPage();
        }

        private async void ChangePhoto(object sender, EventArgs e)
        {
            await ViewPresenter.OnPhotoClicked();
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

        public void ImageAcquired(byte[] data)
        {
            SetAvatarImage(data);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void SetAvatarImage(byte[] data)
        {
            var bmp = BitmapFactory.DecodeByteArray(data, 0, data.Length);
            _avatar.SetImageBitmap(bmp);
        }
    }
}