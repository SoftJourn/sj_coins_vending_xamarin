
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
        private View _buttonNameUnderline;
        private View _buttonPriceUnderline;
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
            _buttonNameUnderline = FindViewById<View>(Resource.Id.button_name_underline);

            _sortPriceButton = FindViewById<Button>(Resource.Id.button_sort_price);
            _sortPriceButton.Click += OnSortByPriceClick;
            _sortPriceButton.SetCompoundDrawables(null, null, null, null);
            _buttonPriceUnderline = FindViewById<View>(Resource.Id.button_price_underline);

            _machineItems = FindViewById<RecyclerView>(Resource.Id.list_items_recycler_view);
            _textViewNoProductsInCategory = FindViewById<TextView>(Resource.Id.textViewNoProductsInCategory);

            _layoutManager = new LinearLayoutManager(this, LinearLayoutManager.Vertical, false);
            _adapter = new FeaturedProductItemsAdapter(_category, RecyclerType, this);

            DetachEvents();
            AttachEvents();

            _machineItems.SetLayoutManager(_layoutManager);

            _machineItems.SetAdapter(_adapter);

            _adapter.SetData(_productList);

        }

        protected override void OnResume()
        {
            base.OnResume();
            _adapter.SetData(ViewPresenter.GetProductList(_category));
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            _adapter = null;
        }

        /**
         * Calls when Sort By Price button clicked
         * Sets colors of button to highlight chosen 
         * and calls sorting method on Presenters side
         */
        private void OnSortByPriceClick(object sender, EventArgs e)
        {
            ViewPresenter.OnSortByPriceClicked(_category);
            _buttonNameUnderline.Visibility = ViewStates.Invisible;
            _buttonPriceUnderline.Visibility = ViewStates.Visible;
        }

        /**
         * Calls when Sort By Name button clicked
         * Sets colors of button to highlight chosen 
         * and calls sorting method on Presenters side
         */
        private void OnSortByNameClick(object sender, EventArgs e)
        {
            ViewPresenter.OnSortByNameClicked(_category);
            _buttonNameUnderline.Visibility = ViewStates.Visible;
            _buttonPriceUnderline.Visibility = ViewStates.Invisible;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            base.OnCreateOptionsMenu(menu);

            //Don't show search on Favorites Category
            if (_category != Const.Favorites)
            {
                menu.FindItem(Resource.Id.action_search).SetVisible(true);
            }
            menu.FindItem(Resource.Id.profile).SetVisible(false);
            menu.FindItem(Resource.Id.menu_add_favorite).SetVisible(false);
            menu.FindItem(Resource.Id.menu_buy).SetVisible(false);

            #region SearchView
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

                //Disable sorting buttons when search is active
                _sortNameButton.Enabled = false;
                _sortPriceButton.Enabled = false;
            };


            searchView.Close += (s, e) =>

            {
                //Enabling Sort buttons
                _sortNameButton.Enabled = true;
                _sortPriceButton.Enabled = true;

                //Clear search string
                searchView.ClearFocus();
                searchView.SetQuery("", false);

                //Close keyboard
                searchView.OnActionViewCollapsed();
            };
            #endregion
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

        /**
         * Is called by Presenter when added or removed favorite
         * to make adapter redraw recyclerview
         */

        public void FavoriteChanged(bool isFavorite)
        {
            if (!_category.Equals(Const.Favorites))
            {
                _adapter.ChangeFavoriteIcon();
            }

            var fragment = SupportFragmentManager.FindFragmentByTag(Const.BottomSheetFragmentTag) as ProductDetailsFragment;
            //if fragment exists
            if (fragment != null)
            {
                fragment.ChangeFavoriteIcon();
            }
        }

        /**
         * Is called by Presenter when List is sorted
         * to make adapter redraw recyclerview
         */
        public void ShowSortedList(List<Product> products)
        {
            _adapter.SetData(products);
        }

        public void SetCompoundDrawableName(bool? isAsc)
        {
            if (isAsc == null)
            {
                _sortNameButton.SetCompoundDrawablesWithIntrinsicBounds(null, null, null, null);
                return;
            }
            if ((bool)isAsc)
            {
                _sortNameButton.SetCompoundDrawablesWithIntrinsicBounds(ContextCompat.GetDrawable(this, Resource.Drawable.ic_arrow_up), null,
                    null, null);
            }
            else
            {
                _sortNameButton.SetCompoundDrawablesWithIntrinsicBounds(ContextCompat.GetDrawable(this, Resource.Drawable.ic_arrow_down), null, 
                    null, null);
            }
        }

        public void SetCompoundDrawablePrice(bool? isAsc)
        {
            if (isAsc == null)
            {
                _sortPriceButton.SetCompoundDrawablesWithIntrinsicBounds(null, null, null, null);
                return;
            }
            if ((bool)isAsc)
            {
                _sortPriceButton.SetCompoundDrawablesWithIntrinsicBounds(null, null,
                    ContextCompat.GetDrawable(this, Resource.Drawable.ic_arrow_up), null);
            }
            else
            {
                _sortPriceButton.SetCompoundDrawablesWithIntrinsicBounds(null, null,
                    ContextCompat.GetDrawable(this, Resource.Drawable.ic_arrow_down), null);
            }
        }

        public new void ShowProgress(string message)
        {
            ProgressDialog.SetIndeterminateDrawable(ContextCompat.GetDrawable(this,Resource.Drawable.basket_animation));
            ProgressDialog.SetMessage(message);
            ProgressDialog.SetCancelable(false);
            ProgressDialog.Show();
        }

        /**
         * Calls Purchase functionality on Presenters side
         */
        public void Purchase(Product product)
        {
            ViewPresenter.OnBuyProductClick(product);
        }

        /**
         * Calls navigation to Details screen on Presenter's side
         * with the given product
         * Is called by OnClick on product item
         */
        private void ProductSelected(object sender, Product product)
        {
            ViewPresenter.OnProductDetailsClick(product.Id);
        }

        /**
         * Attaches BottomSheetFragment with the given product (Preview functionality)
         * Is called by OnLongClick on product item
         */
        private void ProductDetailsSelected(object sender, Product product)
        {
            BottomSheetDialogFragment bottomSheetDialogFragment = new ProductDetailsFragment(product);
            bottomSheetDialogFragment.Show(SupportFragmentManager, Const.BottomSheetFragmentTag);
        }

        /**
         * Calls Add/Remove Favorite on Presenter's side
         * Is Called by FeaturesAdapter.
         */
        public void TrigFavorite(object sender, Product product)
        {
            ViewPresenter.OnFavoriteClick(product);
        }

        /**
         * Calls Add/Remove Favorite on Presenter's side
         * Is Called by ProductDetailsFragment
         * TODO: Needed to make this method called by EventHandler as Previous
         */
        public void TrigFavorite(Product product)
        {
            ViewPresenter.OnFavoriteClick(product);
        }

        /**
         * Hides recyclerView and Shows TextView
         * when there is no products 
         * e.g. Last Favorite was removed
         */ 
        private void ShowEmptyView(object sender, EventArgs e)
        {
            _machineItems.Visibility = ViewStates.Gone;
            _textViewNoProductsInCategory.Visibility = ViewStates.Visible;
        }

        public override void AttachEvents()
        {
            _adapter.ProductSelected += ProductSelected;
            _adapter.ProductDetailsSelected += ProductDetailsSelected;
            _adapter.AddToFavorites += TrigFavorite;
            _adapter.RemoveFromFavorites += TrigFavorite;
            _adapter.LastFavoriteRemoved += ShowEmptyView;
        }

        public override void DetachEvents()
        {
            _adapter.ProductSelected -= ProductSelected;
            _adapter.ProductDetailsSelected -= ProductDetailsSelected;
            _adapter.AddToFavorites -= TrigFavorite;
            _adapter.RemoveFromFavorites -= TrigFavorite;
            _adapter.LastFavoriteRemoved -= ShowEmptyView;
        }
    }
}