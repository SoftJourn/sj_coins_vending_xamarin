using Android.App;
using Android.Content.PM;
using Android.OS;
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
        private string _coinsLabel = " coins";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_qr);
            var fragmentType = Intent.GetStringExtra(Const.NavigationKey);

            _balance = FindViewById<TextView>(Resource.Id.qr_balance);
            MobileBarcodeScanner.Initialize(Application);

            if (fragmentType == Const.QrScreenScanningTag)
            {
                FragmentManager.BeginTransaction()
                .Replace(Resource.Id.container_fargment, ScanningResultFragment.NewInstance(),
                 Const.QrScreenScanningTag)
                .Commit();
            }
            else
            {
                FragmentManager.BeginTransaction()
                .Replace(Resource.Id.container_fargment, GenerateCodeFragment.NewInstance(),
                 Const.QrScreenScanningTag)
                .Commit();
            }
            SupportActionBar?.SetDisplayHomeAsUpEnabled(true);
            _balance.Visibility = ViewStates.Visible;
            _balance.Text = ViewPresenter.GetBalance() + _coinsLabel;
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

        public override void OnBackPressed()
        {
            Finish();
        }

        public void UpdateBalance(string remain)
        {
            _balance.Text = remain + _coinsLabel;
        }

        public void ShowSuccessFunding()
        {
            var fragment = FragmentManager.FindFragmentById(Resource.Id.container_fargment) as ScanningResultFragment;
            fragment?.ShowTextViewScanned();
        }

        public void SetEditFieldError(string message)
        {
            var fragment = FragmentManager.FindFragmentById(Resource.Id.container_fargment) as GenerateCodeFragment;
            fragment?.ShowEditFieldError(message);
        }

        public void ShowImage(string image)
        {
            var fragment = FragmentManager.FindFragmentById(Resource.Id.container_fargment) as GenerateCodeFragment;
            fragment?.ShowImageCode(image);
        }

        public void SetBalance(string amount)
        {
            _balance.Text = amount + _coinsLabel;
        }

        public void ScanCode()
        {
            ViewPresenter.ScanCode();
        }

        public void GenerateCode(string amount)
        {
            ViewPresenter.GenerateCode(amount);
        }
    }
}