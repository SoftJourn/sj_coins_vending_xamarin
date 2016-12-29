using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Softjourn.SJCoins.Droid;
using Softjourn.SJCoins.Droid.Receivers;

namespace Softjourn.SJCoins.Droid.ui.activities
{
    [Activity(Label = "NoInternet", Theme = "@style/NoActionBarLoginTheme")]
    public class NoInternetActivity : AppCompatActivity
    {
        private InternetBroadcastListener mBroadcastListener;

        //private AnimatedSvgView svgView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_no_internet);
            mBroadcastListener = new InternetBroadcastListener(this);
            
            RegisterReceiver(mBroadcastListener, new IntentFilter(ConnectivityManager.ConnectivityAction));
        }

        protected override void OnDestroy()
        {
            UnregisterReceiver(mBroadcastListener);
            base.OnDestroy();
        }

    }
}