using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Plugin.Permissions;
using Softjourn.SJCoins.Core.API.Model.AccountInfo;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Droid.ui.baseUI;
using Softjourn.SJCoins.Droid.Utils;

namespace Softjourn.SJCoins.Droid.UI.Activities
{
    [Activity(Label = "Profile", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ProfileActivity : BaseActivity<AccountPresenter>, IAccountView

    {
        private TextView _username;
        private TextView _balance;
        private ListView _options;
        private ImageView _avatar;
        private Bitmap _bmp;

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
            _options.Adapter = adapter;
            _options.Visibility = ViewStates.Visible;
            _options.VerticalScrollBarEnabled = false;
            SetListViewHeightBasedOnChildren(_options);
            _options.ItemClick += (sender, e) =>
            {
                ViewPresenter.OnItemClick(_options.GetItemAtPosition(e.Position).ToString());
            };
            //To make Actvity Opened
            _avatar.RequestFocus();
            ViewPresenter.OnStartLoadingPage();
        }

        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            base.OnRestoreInstanceState(savedInstanceState);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            return false;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
                this.OnBackPressed();

            return base.OnOptionsItemSelected(item);
        }

        protected override void OnResume()
        {
            base.OnResume();
            ViewPresenter.OnStartLoadingPage();
        }

        public void SetAccountInfo(Account account)
        {
            _username.Text = account.Name + " " + account.Surname;
            _balance.Text = account.Amount.ToString();
        }

        public void ImageAcquired(byte[] data)
        {
            
        }

        public void ImageAcquired(string data)
        {
            SetAvatarImage(data);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        #region Private Methods
        private void SetAvatarImage(string data)
        {
            var imageSize = 360;
            _bmp?.Recycle();
            var options = new BitmapFactory.Options();
            options.InJustDecodeBounds = true;
            _bmp = BitmapFactory.DecodeFile(data, options);
            options.InSampleSize = BitmapUtils.CalculateInSampleSize(options, imageSize, imageSize);
            options.InJustDecodeBounds = false;
            options.InPreferredConfig = Bitmap.Config.Rgb565;
            _bmp = BitmapFactory.DecodeFile(data, options);
            _bmp = BitmapUtils.RotateIfNeeded(_bmp, data);
            _avatar.SetImageBitmap(_bmp);
        }

        private async void ChangePhoto(object sender, EventArgs e)
        {
            await ViewPresenter.OnPhotoClicked();
        }

        private static void SetListViewHeightBasedOnChildren(ListView listView)
        {
            var listAdapter = listView.Adapter;
            if (listAdapter == null)
                return;

            int desiredWidth = View.MeasureSpec.MakeMeasureSpec(listView.Width, MeasureSpecMode.Unspecified);
            int totalHeight = 0;
            View view = null;

            for (int i = 0; i < listAdapter.Count; i++)
            {
                view = listAdapter.GetView(i, view, listView);

                if (i == 0)
                    view.LayoutParameters = new ViewGroup.LayoutParams(desiredWidth, ViewGroup.LayoutParams.MatchParent);

                view.Measure(desiredWidth, 0);
                totalHeight += view.MeasuredHeight;
            }

            var lParams = listView.LayoutParameters;
            lParams.Height = totalHeight + ((listView.DividerHeight) * (listAdapter.Count));

            listView.LayoutParameters = lParams;
            listView.RequestLayout();
        }
        #endregion
    }
}