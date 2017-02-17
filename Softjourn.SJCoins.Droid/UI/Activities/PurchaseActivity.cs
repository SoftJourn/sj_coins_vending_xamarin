using System.Collections.Generic;
using Android.App;
using Android.Content.PM;
using Android.OS;
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
    [Activity(Label = "Purchase History", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
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

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            RecyclerView.LayoutManager layoutManager = new LinearLayoutManager(this, LinearLayoutManager.Vertical, false);
            _adapter = new PurchaseHistoryAdapter();
            _purchaseRecyclerView.SetLayoutManager(layoutManager);
            _purchaseRecyclerView.SetAdapter(_adapter);

            ViewPresenter.OnStartLoadingPage();

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

        public void SetData(List<History> listPurchases)
        {
            _purchaseRecyclerView.Visibility  = ViewStates.Visible;
            _noPurchasesTextView.Visibility  = ViewStates.Gone;
            _adapter.SetData(listPurchases);
        }
    }
}