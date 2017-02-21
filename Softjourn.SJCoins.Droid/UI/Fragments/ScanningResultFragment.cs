
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Softjourn.SJCoins.Droid.UI.Activities;


namespace Softjourn.SJCoins.Droid.UI.Fragments
{
    public class ScanningResultFragment : Fragment
    {
        private TextView _walletWasFunded;
        private Button _buttonScanAgain;

        public static ScanningResultFragment NewInstance()
        {
            var fragment = new ScanningResultFragment();
            return fragment;
        }

        public ScanningResultFragment()
        {

        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_scan_code, container, false);
            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _walletWasFunded = view.FindViewById<TextView>(Resource.Id.money_added_textView);
            _buttonScanAgain = view.FindViewById<Button>(Resource.Id.btn_scan_again);
            _buttonScanAgain.Click += (sender, e) =>
            {
                ScanCode();
            };
            ScanCode();
        }

        private void ScanCode()
        {
            ((QrActivity) Activity).ScanCode();
        }

        public void ShowTextViewScanned()
        {
            //_walletWasFunded.Text = string.Format(GetString(Resource.String.wallet_funded, amount));
            _walletWasFunded.Visibility = ViewStates.Visible;
        }
    }
}