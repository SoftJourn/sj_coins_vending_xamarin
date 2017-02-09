
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Droid.ui.baseUI;
using Softjourn.SJCoins.Droid.utils;
using Softjourn.SJCoins.Droid.UI.Adapters;
using Softjourn.SJCoins.Droid.Utils;

namespace Softjourn.SJCoins.Droid.UI.Activities
{
    public class ShowAllActivity : BaseActivity<ShowAllPresenter>
    {
        private FeaturedProductItemsAdapter _adapter;

        private string _category;

        private Button _fragmentsSortNameButton;
        private Button _fragmentsSortPriceButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_show_all);

            RequestedOrientation = ScreenOrientation.Portrait;

            //mVendingPresenter = new SeeAllPresenter(this);
            //mPurchasePresenter = new PurchasePresenter(this);

            _category = Intent.GetStringExtra(Const.ExtrasCategory);
            Title = _category;

            SupportActionBar?.SetDisplayHomeAsUpEnabled(true);
            AttachFragment(_category);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            base.OnCreateOptionsMenu(menu);
            if (_category != Const.Favorites)
            {
                menu.FindItem(Resource.Id.action_search).SetVisible(true);
            }
            menu.FindItem(Resource.Id.menu_favorites).SetVisible(false);
            menu.FindItem(Resource.Id.profile).SetVisible(false);

            var manager = (SearchManager)GetSystemService(SearchService);

            var search = menu.FindItem(Resource.Id.action_search).ActionView;
            var searchView = search.JavaCast<Android.Support.V7.Widget.SearchView>();

            searchView.SetSearchableInfo(manager.GetSearchableInfo(ComponentName));

            searchView.QueryHint = GetString(Resource.String.search_hint);

            var searchEditText = searchView.FindViewById<EditText>(Resource.Id.search_src_text);

            searchEditText.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.white)));

            searchEditText.SetHintTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.white)));

            searchView.QueryTextSubmit += (s, e) =>
            {
                var imm = (InputMethodManager)GetSystemService(InputMethodService);
                imm.HideSoftInputFromWindow(CurrentFocus.WindowToken, 0);
                searchView.ClearFocus();
                e.Handled = true;
            };
            searchView.QueryTextChange += (s, e) =>
            {
                if (TextUtils.IsEmpty(e.NewText))
                {
                    _adapter.Filter.InvokeFilter("");
                }
                else
                {
                    _adapter.Filter.InvokeFilter(e.NewText);
                }
                _fragmentsSortNameButton.Enabled = false;
                _fragmentsSortPriceButton.Enabled = false;
            };


            searchView.Close += (s, e) =>

            {
                _fragmentsSortNameButton.Enabled = true;
                _fragmentsSortPriceButton.Enabled = true;
                searchView.ClearFocus();
                searchView.SetQuery("", false);
                searchView.OnActionViewCollapsed();
            };

            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }


        /*@Override
            public void navigateToBuyProduct(Product product)
        {
            onCreateConfirmDialog(product, mPurchasePresenter);
        }

        @Override
            public void logOut()
        {
            Utils.clearUsersData();
            Navigation.goToLoginActivity(this);
            finish();
        }

        @Override
            public void showToastMessage(String message)
        {
            super.showToast(message);
        }

        @Override
            public void showNoInternetError()
        {
            onNoInternetAvailable();
        }*/

        protected override void OnDestroy()
        {
            base.OnDestroy();
            //mVendingPresenter.onDestroy();
            //mPurchasePresenter.onDestroy();
            //mVendingPresenter = null;
            //mPurchasePresenter = null;
        }

        private void AttachFragment(string stringExtra)
        {
            //NavigationUtils.NavigationOnCategoriesSeeAll(-1, this, _category);
        }

        /**
         * Method to disable sorting buttons in case Search is active as Search is handling in Activity class
         * and Buttons are on the Fragment side.
         */
        public void SetButtons(Button nameButton, Button priceButton)
        {
            this._fragmentsSortNameButton = nameButton;
            this._fragmentsSortPriceButton = priceButton;
            _fragmentsSortNameButton.SetBackgroundColor(new Color(GetColor(Resource.Color.colorScreenBackground)));
            _fragmentsSortPriceButton.SetBackgroundColor(new Color(GetColor(Resource.Color.transparent)));
        }

        /**
         * Method to set Category and adapter for correct appearing title and correct
         * checking of Item in NavBar.
         *
         * @param adapter  is object of FeaturedProductItemsAdapter for correct behaviour of filtering
         *                 in Search functionality
         * @param category String with the name of the category
         */
        public void ProductsList(FeaturedProductItemsAdapter adapter, string category)
        {
            _category = category;
            _adapter = adapter;
            InvalidateOptionsMenu();
        }
    }
}