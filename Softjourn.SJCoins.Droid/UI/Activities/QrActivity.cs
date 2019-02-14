using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using Plugin.Permissions;
using Softjourn.SJCoins.Core.Common;
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
            var fragmentType = Intent.GetStringExtra(Constant.NavigationKey);

            _balance = FindViewById<TextView>(Resource.Id.qr_balance);

            //Initializing of ZXing Scanner
            MobileBarcodeScanner.Initialize(Application);

            if (fragmentType == Constant.QrScreenScanningTag)
                AttachFragment(Constant.QrScreenScanningTag, ScanningResultFragment.NewInstance());
            else
                AttachFragment(Constant.QrScreenGeneratingTag, GenerateCodeFragment.NewInstance());

            SupportActionBar?.SetDisplayHomeAsUpEnabled(true);

            SetInitialBalance();
        }

        public override bool OnCreateOptionsMenu(IMenu menu) => false;

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
                OnBackPressed();

            return base.OnOptionsItemSelected(item);
        }

        public override void OnBackPressed() => Finish();

        #endregion

        #region IQrView Implementation

        /// <summary>
        /// Updates user balance
        /// </summary>
        /// <param name="amount"></param>
        public void UpdateBalance(string amount) => _balance.Text = $"{amount}{CoinsLabel}";

        /// <summary>
        /// If amount is not valid call method on fragment
        /// to set Error to EditField
        /// </summary>
        /// <param name="message"></param>
        public void SetEditFieldError(string message)
        {
            var fragment = FragmentManager.FindFragmentById(Resource.Id.container_fragment) as GenerateCodeFragment;
            fragment?.ShowEditFieldError(message);
        }

        /// <summary>
        /// Call method on Fragment to generate QRcode
        /// </summary>
        /// <param name="cashJsonString"></param>
        public void ShowImage(string cashJsonString)
        {
            var fragment = FragmentManager.FindFragmentById(Resource.Id.container_fragment) as GenerateCodeFragment;
            fragment?.ShowImageCode(cashJsonString);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Call Scanning on Presenters side
        /// </summary>
        public void ScanCode() => ViewPresenter.ScanCode();

        /// <summary>
        /// Call Method in Presenter to make api call to get info for
        /// generating of QR code.
        /// </summary>
        /// <param name="amount"></param>
        public void GenerateCode(string amount) => ViewPresenter.GenerateCode(amount);

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            Permission[] grantResults) =>
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        #endregion

        #region Private Methods

        /// <summary>
        /// Attaches needed fragment
        /// </summary>
        /// <param name="fragmentTag"></param>
        /// <param name="fragment"></param>
        private void AttachFragment(string fragmentTag, Fragment fragment) => FragmentManager.BeginTransaction()
            .Replace(Resource.Id.container_fragment, fragment, fragmentTag).Commit();

        /// <summary>
        /// Sets balance as Text to TextView based on Response from Presenter
        /// </summary>
        private void SetInitialBalance()
        {
            _balance.Visibility = ViewStates.Visible;
            _balance.Text = ViewPresenter.GetBalance() + CoinsLabel;
        }

        #endregion
    }
}