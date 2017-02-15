
using System;
using System.Collections.Generic;
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Droid.ui.baseUI;
using Softjourn.SJCoins.Droid.UI.Adapters;
using Softjourn.SJCoins.Droid.UI.Fragments;
using Softjourn.SJCoins.Droid.Utils;

namespace Softjourn.SJCoins.Droid.UI.Activities
{
    [Activity(Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ShowAllActivity : BaseActivity<ShowAllPresenter>, IShowAllView
    {
        private FeaturedProductItemsAdapter _adapter;

        private string _category;

        private Button _sortNameButton;
        private Button _sortPriceButton;
        private TextView _textViewNoProductsInCategory;

        private const string ProductsCategory = Const.NavigationKey;
        private const string RecyclerType = "SEE_ALL_SNACKS_DRINKS";

        private static RecyclerView.LayoutManager _layoutManager;
        private List<Product> _productList;

        RecyclerView _machineItems;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_show_all);

            _category = Intent.GetStringExtra(ProductsCategory);
            Title = _category;

            SupportActionBar?.SetDisplayHomeAsUpEnabled(true);

            _productList = ViewPresenter.GetProductList(_category);

            _sortNameButton = FindViewById<Button>(Resource.Id.button_sort_name);
            _sortNameButton.Click += OnSortByNameClick;
            _sortPriceButton = FindViewById<Button>(Resource.Id.button_sort_price);
            _sortPriceButton.Click += OnSortByPriceClick;

            _machineItems = FindViewById<RecyclerView>(Resource.Id.list_items_recycler_view);
            _textViewNoProductsInCategory = FindViewById<TextView>(Resource.Id.textViewNoProductsInCategory);

            _layoutManager = new LinearLayoutManager(this, LinearLayoutManager.Vertical, false);
            _adapter = new FeaturedProductItemsAdapter(_category, RecyclerType, this);

            _adapter.ProductSelected -= ProductSelected;
            _adapter.ProductSelected += ProductSelected;

            _adapter.ProductDetailsSelected -= ProductDetailsSelected;
            _adapter.ProductDetailsSelected += ProductDetailsSelected;

            _adapter.AddToFavorites -= TrigFavorite;
            _adapter.AddToFavorites += TrigFavorite;

            _adapter.RemoveFromFavorites -= TrigFavorite;
            _adapter.RemoveFromFavorites += TrigFavorite;

            _adapter.BuyClicked -= ProductBuyClicked;
            _adapter.BuyClicked += ProductBuyClicked;

            _adapter.LastFavoriteRemoved -= ShowEmptyView;
            _adapter.LastFavoriteRemoved += ShowEmptyView;

            _machineItems.SetLayoutManager(_layoutManager);

            _machineItems.SetAdapter(_adapter);

            _adapter.SetData(_productList);

        }

        private void OnSortByPriceClick(object sender, EventArgs e)
        {
            ViewPresenter.OnSortByPriceClicked(_category);
            _sortPriceButton.SetBackgroundColor(new Color(GetColor(Resource.Color.colorScreenBackground)));
            _sortNameButton.SetBackgroundColor(new Color(GetColor(Resource.Color.transparent)));
        }

        private void OnSortByNameClick(object sender, EventArgs e)
        {
            ViewPresenter.OnSortByNameClicked(_category);
            _sortNameButton.SetBackgroundColor(new Color(GetColor(Resource.Color.colorScreenBackground)));
            _sortPriceButton.SetBackgroundColor(new Color(GetColor(Resource.Color.transparent)));
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            base.OnCreateOptionsMenu(menu);
            if (_category != Const.Favorites)
            {
                menu.FindItem(Resource.Id.action_search).SetVisible(true);
            }
            menu.FindItem(Resource.Id.profile).SetVisible(false);
            menu.FindItem(Resource.Id.menu_add_favorite).SetVisible(false);
            menu.FindItem(Resource.Id.menu_buy).SetVisible(false);

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
                _sortNameButton.Enabled = false;
                _sortPriceButton.Enabled = false;
            };


            searchView.Close += (s, e) =>

            {
                _sortNameButton.Enabled = true;
                _sortPriceButton.Enabled = true;
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

        public void FavoriteChanged(bool isFavorite)
        {
            _adapter.NotifyDataChanges();
        }

        public void ShowSortedList(List<Product> products)
        {
            _adapter.SetData(products);
        }

        public void Purchase(Product product)
        {
            ViewPresenter.OnBuyProductClick(product);
        }

        private void ProductSelected(object sender, Product product)
        {
            ViewPresenter.OnProductDetailsClick(product.Id);
        }

        private void ProductDetailsSelected(object sender, Product product)
        {
            BottomSheetDialogFragment bottomSheetDialogFragment = new ProductDetailsFragment(product);
            bottomSheetDialogFragment.Show(SupportFragmentManager, bottomSheetDialogFragment.Tag);
        }

        private void ProductBuyClicked(object sender, Product e)
        {
            ViewPresenter.OnBuyProductClick(e);
        }

        public void TrigFavorite(object sender, Product product)
        {
            ViewPresenter.OnFavoriteClick(product);
        }

        private void ShowEmptyView(object sender, EventArgs e)
        {
            _machineItems.Visibility = ViewStates.Gone;
            _textViewNoProductsInCategory.Visibility = ViewStates.Visible;

        }
    }
}