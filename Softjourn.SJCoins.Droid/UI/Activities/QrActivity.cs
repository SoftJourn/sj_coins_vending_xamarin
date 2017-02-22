using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using Plugin.Permissions;
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
        private const string CoinsLabel = " coins";

        #region Activity methods

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_qr);
            var fragmentType = Intent.GetStringExtra(Const.NavigationKey);

            _balance = FindViewById<TextView>(Resource.Id.qr_balance);
            
            //Initializing of ZXing Scanner
            MobileBarcodeScanner.Initialize(Application);

            if (fragmentType == Const.QrScreenScanningTag)
            {
                AttachFragment(Const.QrScreenScanningTag, ScanningResultFragment.NewInstance());
            }
            else
            {
                AttachFragment(Const.QrScreenGeneratingTag, GenerateCodeFragment.NewInstance());
            }
            SupportActionBar?.SetDisplayHomeAsUpEnabled(true);

            SetInitialBalance();
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
        #endregion

        #region IQrView Implementation
        //Updates user balance
        public void UpdateBalance(string amount)
        {
            _balance.Text = amount + CoinsLabel;
        }

        //If amount is not valid call method on fragment
        //to set Error to EditField
        public void SetEditFieldError(string message)
        {
            var fragment = FragmentManager.FindFragmentById(Resource.Id.container_fragment) as GenerateCodeFragment;
            fragment?.ShowEditFieldError(message);
        }

        //Call method on Fragment to generate QRcode
        public void ShowImage(string cashJsonString)
        {
            var fragment = FragmentManager.FindFragmentById(Resource.Id.container_fragment) as GenerateCodeFragment;
            fragment?.ShowImageCode(cashJsonString);
        }
        #endregion

        #region Public Methods
        //Call Scanning on Presenters side
        public void ScanCode()
        {
            ViewPresenter.ScanCode();
        }

        //Call Method in Presenter to make api call to get info for
        //generating of QR code.
        public void GenerateCode(string amount)
        {
            ViewPresenter.GenerateCode(amount);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        #endregion

        #region Private Methods
        //Attaches needed fragment
        private void AttachFragment(string fragmentTag, Fragment fragment)
        {
            FragmentManager.BeginTransaction()
                .Replace(Resource.Id.container_fragment, fragment, fragmentTag)
                .Commit();
        }

        //Sets balance as Text to TextView based on Response from Presenter
        private void SetInitialBalance()
        {
            _balance.Visibility = ViewStates.Visible;
            _balance.Text = ViewPresenter.GetBalance() + CoinsLabel;
        }
        #endregion
    }
}