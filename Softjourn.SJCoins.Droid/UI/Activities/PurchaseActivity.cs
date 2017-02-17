using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace Softjourn.SJCoins.Droid.UI.Activities
{
    public class PurchaseActivity : AppCompatActivity
    {
        private RecyclerView _purchaseRecyclerView;
        private TextView _noPurchasesTextView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_purchases);

            _purchaseRecyclerView = FindViewById<RecyclerView>(Resource.Id.list_items_recycler_view);
            _noPurchasesTextView = FindViewById<TextView>(Resource.Id.textViewNoPurchases);

            
        }

        public void SetData(List<Purchase> listPurchases)
        {
            mHistoryList.setVisibility(View.VISIBLE);
            mHistoryList.startAnimation(AnimationUtils.loadAnimation(this, R.anim.fade_in));
            mNoPurchasesTextView.setVisibility(GONE);
            mNoPurchasesTextView.startAnimation(AnimationUtils.loadAnimation(this, R.anim.fade_out));
            mHistoryAdapter.setData(history);
        }
    }
}