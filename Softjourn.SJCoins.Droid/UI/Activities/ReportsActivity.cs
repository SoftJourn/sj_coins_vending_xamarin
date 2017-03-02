
using System;
using System.Collections.Generic;

using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Softjourn.SJCoins.Core.API.Model.TransactionReports;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Droid.ui.baseUI;
using Softjourn.SJCoins.Droid.UI.Adapters;
using Softjourn.SJCoins.Droid.UI.Listeners;

namespace Softjourn.SJCoins.Droid.UI.Activities
{
    [Activity(Label = "Transaction Reports", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ReportsActivity : BaseActivity<TransactionReportPresenter>, ITransactionReportView
    {
        private RecyclerView _transactionsRecyclerView;
        private TextView _noTransactionsTextView;
        private ReportsAdapter _adapter;
        private SwipeRefreshLayout _refreshLayout;

        private LinearLayout _buttonBar;
        private Button _buttonInput;
        private View _buttonInputUnderline;
        private Button _buttonOutput;
        private View _buttonOutputUnderline;

        #region Activity standart methods

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_transaction);

            _transactionsRecyclerView = FindViewById<RecyclerView>(Resource.Id.list_items_recycler_view);
            _noTransactionsTextView = FindViewById<TextView>(Resource.Id.textViewNoPurchases);

            _buttonBar = FindViewById<LinearLayout>(Resource.Id.reports_button_bar);
            _buttonInput = FindViewById<Button>(Resource.Id.button_input);
            _buttonInput.Click += ButtonInputOnClick;

            _buttonInputUnderline = FindViewById<View>(Resource.Id.button_input_underline);

            _buttonOutput = FindViewById<Button>(Resource.Id.button_output);
            _buttonOutput.Click += ButtonOutputOnClick;

            _buttonOutputUnderline = FindViewById<View>(Resource.Id.button_output_underline);

            _refreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_container);
            _refreshLayout.SetColorSchemeResources(Resource.Color.colorAccent);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            //Setting adapter for recycler view
            var layoutManager = new LinearLayoutManager(this, LinearLayoutManager.Vertical, false);

            //Creating OnScrollListener to handle if the end of list reached 
            //so we can load next page of data
            var onScrollListener = new XamarinRecyclerViewOnScrollListener(layoutManager);
            onScrollListener.LoadMoreEvent += (sender, e) =>
            {
                ViewPresenter.GetNextPage();
            };

            _transactionsRecyclerView.AddOnScrollListener(onScrollListener);

            _adapter = new ReportsAdapter();
            _transactionsRecyclerView.SetLayoutManager(layoutManager);
            _transactionsRecyclerView.SetAdapter(_adapter);

            ViewPresenter.OnStartLoadingPage();

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var inflater = MenuInflater;
            inflater.Inflate(Resource.Menu.menu_transactions, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                OnBackPressed();
            }
            else
            {
                switch (item.ItemId)
                {
                    case Resource.Id.transactions_menu_date:
                        //ViewPresenter.OnSortDateClicked();
                        return true;
                    case Resource.Id.transactions_menu_amount:
                        //ViewPresenter.OnSortAmountClicked();
                        return true;
                }
            }
            return base.OnOptionsItemSelected(item);
        }

        #endregion

        #region ITransactionView implementation


        //Setting data to adapter in case purchase list is not empty
        public void SetData(List<Transaction> transactionsList)
        {
            _transactionsRecyclerView.Visibility = ViewStates.Visible;
            _buttonBar.Visibility = ViewStates.Visible;
            _adapter.SetData(transactionsList);
        }

        //Showing emptyView in case purchase list is empty
        public void ShowEmptyView()
        {
            _noTransactionsTextView.Visibility = ViewStates.Visible;
        }

        public void AddItemsToExistedList(List<Transaction> transactionsList)
        {
            _adapter.AddData(transactionsList);
        }

        #endregion

        #region Private Methods

        private void ButtonOutputOnClick(object sender, EventArgs eventArgs)
        {
            _buttonInputUnderline.Visibility = ViewStates.Invisible;
            _buttonOutputUnderline.Visibility = ViewStates.Visible;
            ViewPresenter.OnOutputClicked();
        }

        private void ButtonInputOnClick(object sender, EventArgs eventArgs)
        {
            _buttonInputUnderline.Visibility = ViewStates.Visible;
            _buttonOutputUnderline.Visibility = ViewStates.Invisible;
            ViewPresenter.OnInputClicked();
        }

        #endregion

        public override void HideProgress()
        {
            _refreshLayout.Refreshing = false;
        }

        public override void ShowProgress(string message)
        {
            _refreshLayout.Refreshing = true;
        }
    }
}