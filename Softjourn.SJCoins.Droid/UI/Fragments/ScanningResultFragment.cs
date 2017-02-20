using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Softjourn.SJCoins.Droid.UI.Activities;
using Softjourn.SJCoins.Droid.UI.Adapters;

namespace Softjourn.SJCoins.Droid.UI.Fragments
{
    public class ScanningResultFragment : Fragment
    {
        private TextView _walletWasFunded;

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
            ScanCode();
        }

        private void ScanCode()
        {
            ((QrActivity) Activity).ScanCode();
        }

        public void ShowTextViewScanned()
        {
            _walletWasFunded.Visibility = ViewStates.Visible;
        }
    }
}