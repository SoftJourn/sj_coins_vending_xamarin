using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Softjourn.SJCoins.Droid.UI.Activities;

namespace Softjourn.SJCoins.Droid.UI.Fragments
{
    public class ScanningResultFragment : Fragment
    {
        private Button _buttonScanAgain;

        public static ScanningResultFragment NewInstance() => new ScanningResultFragment();

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) =>
            inflater.Inflate(Resource.Layout.fragment_scan_code, container, false);

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _buttonScanAgain = view.FindViewById<Button>(Resource.Id.btn_scan_again);
            _buttonScanAgain.Click += (sender, e) =>
            {
                ScanCode();
            };
            ScanCode();
        }

        private void ScanCode() => ((QrActivity)Activity).ScanCode();
    }
}