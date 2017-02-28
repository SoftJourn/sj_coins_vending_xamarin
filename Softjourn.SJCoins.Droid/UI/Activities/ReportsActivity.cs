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
using Softjourn.SJCoins.Core.API.Model;
using Softjourn.SJCoins.Droid.ui.baseUI;
using Softjourn.SJCoins.Droid.UI.Adapters;

namespace Softjourn.SJCoins.Droid.UI.Activities
{
    public class ReportsActivity : BaseActivity<ReportsTransaction>, IReportsView
    {
        private RecyclerView _purchaseRecyclerView;
        private TextView _noPurchasesTextView;
        private TextView _loadingTextView;
        private ReportsAdapter _adapter;

        #region Activity standart methods
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_purchases);

            _purchaseRecyclerView = FindViewById<RecyclerView>(Resource.Id.list_items_recycler_view);
            _noPurchasesTextView = FindViewById<TextView>(Resource.Id.textViewNoPurchases);
            _loadingTextView = FindViewById<TextView>(Resource.Id.textViewLoading);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            //Setting adapter for recycler view
            RecyclerView.LayoutManager layoutManager = new LinearLayoutManager(this, LinearLayoutManager.Vertical, false);
            _adapter = new ReportsAdapter();
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
        #endregion

        #region IPurchaseView implementation
        public override void ShowProgress(string message)
        {
            //Do not showing progress as there is TextView "Loading on layout"
        }

        //Setting data to adapter in case purchase list is not empty
        public void SetData(List<Transaction> listPurchases)
        {
            _loadingTextView.Visibility = ViewStates.Gone;
            _purchaseRecyclerView.Visibility = ViewStates.Visible;
            _adapter.SetData(listPurchases);
        }

        //Showing emptyView in case purchase list is empty
        public void ShowEmptyView()
        {
            _loadingTextView.Visibility = ViewStates.Gone;
            _noPurchasesTextView.Visibility = ViewStates.Visible;
        }
        #endregion
    }
}