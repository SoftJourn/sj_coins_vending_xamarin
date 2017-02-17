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
using Softjourn.SJCoins.Core.API.Model;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Droid.ui.baseUI;
using Softjourn.SJCoins.Droid.UI.Adapters;

namespace Softjourn.SJCoins.Droid.UI.Activities
{
    public class PurchaseActivity : BaseActivity<PurchasePresenter>, IPurchaseView
    {
        private RecyclerView _purchaseRecyclerView;
        private TextView _noPurchasesTextView;
        private PurchaseHistoryAdapter _adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_purchases);

            _purchaseRecyclerView = FindViewById<RecyclerView>(Resource.Id.list_items_recycler_view);
            _noPurchasesTextView = FindViewById<TextView>(Resource.Id.textViewNoPurchases);

            RecyclerView.LayoutManager layoutManager = new LinearLayoutManager(this, LinearLayoutManager.Vertical, false);
            _adapter = new PurchaseHistoryAdapter();
            _purchaseRecyclerView.SetLayoutManager(layoutManager);
            _purchaseRecyclerView.SetAdapter(_adapter);

        }

        public void SetData(List<History> listPurchases)
        {
            _purchaseRecyclerView.Visibility  = ViewStates.Visible;
            _noPurchasesTextView.Visibility  = ViewStates.Gone;
            _adapter.SetData(listPurchases);
        }
    }
}