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
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Droid.ui.baseUI;
using Softjourn.SJCoins.Droid.UI.Fragments;
using Softjourn.SJCoins.Droid.Utils;
using ZXing.Mobile;

namespace Softjourn.SJCoins.Droid.UI.Activities
{
    [Activity(Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class QrActivity : BaseActivity<QrPresenter>, IQrView
    {
        private TextView _balance;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_qr);
            var fragmentType = Intent.GetStringExtra(Const.NavigationKey);

            _balance = FindViewById<TextView>(Resource.Id.qr_balance);

            if (fragmentType == Const.QrScreenScanningTag)
            {
                MobileBarcodeScanner.Initialize(Application);
                FragmentManager.BeginTransaction()
                .Replace(Resource.Id.container_fargment, ScanningResultFragment.NewInstance(),
                 Const.QrScreenScanningTag)
                .Commit();
            }
            else
            {
                
            }

            SupportActionBar?.SetDisplayHomeAsUpEnabled(true);

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

        public void UpdateBalance(string remain)
        {
            _balance.Text = remain + " coins";
            var fragment = FragmentManager.FindFragmentById(Resource.Id.container_fargment) as ScanningResultFragment;
            fragment?.ShowTextViewScanned();
        }

        public void SetBalance(string amount)
        {
            _balance.Text = amount + " coins";
        }

        public void ScanCode()
        {
            ViewPresenter.ScanCode();
        }
    }
}