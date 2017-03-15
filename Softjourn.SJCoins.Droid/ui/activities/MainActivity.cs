
using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using Softjourn.SJCoins.Core.API.Model.AccountInfo;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Droid.ui.baseUI;
using Softjourn.SJCoins.Droid.UI.Fragments;
using Softjourn.SJCoins.Droid.Utils;

namespace Softjourn.SJCoins.Droid.UI.Activities
{
    [Activity(Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : BaseActivity<HomePresenter>, IHomeView
    {

        private SwipeRefreshLayout _swipeLayout;
        private int _viewCounter = 0;
        private TextView _balance;
        private TextView _favoritesShowAll;

        private bool HaveProducts { get; set; }

        //Dictionary for saving container and header IDs for created categories
        //Key - containerId
        //Value - HeaderId
        private Dictionary<int, int> _containerIds;

        #region Activity Standart Methods
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            _containerIds = new Dictionary<int, int>();
            _containerIds.Add(Resource.Id.favorites_container_ID, Resource.Id.favoriteIdLayout);

            _favoritesShowAll = FindViewById<TextView>(Resource.Id.favoriteSeeAllID);
            _favoritesShowAll.Click += (sender, e) =>
            {
                ViewPresenter.OnShowAllClick(Const.Favorites);
            };

            _swipeLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_container);
            _swipeLayout.SetColorSchemeResources(Resource.Color.colorAccent);
            _swipeLayout.Refresh += OnRefresh;

            _balance = FindViewById<TextView>(Resource.Id.balance);

            _swipeLayout.Refreshing = true;
            ViewPresenter.OnStartLoadingPage();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            base.OnCreateOptionsMenu(menu);
            menu.FindItem(Resource.Id.menu_buy).SetVisible(false);
            menu.FindItem(Resource.Id.menu_add_favorite).SetVisible(false);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    return true;
                case Resource.Id.profile:
                    ViewPresenter.OnProfileButtonClicked();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            ViewPresenter.UpdateBalanceView();

            if (HaveProducts)
            FavoriteChanged(true);
        }
        #endregion

        #region Methods from IHomeView Interface

        public override void ShowProgress(string message)
        {
            _swipeLayout.Refreshing = true;
        }

        public override void HideProgress()
        {
            base.HideProgress();
            _swipeLayout.Refreshing = false;
        }

        /**
         * Refreshes favorites fragment when OnResume is called.
         * Is using to add or remove favorite product from favorite fragment when adding ar removing
         * favorite from details or preview
         */
        public void FavoriteChanged(bool isFavorite)
        {
            ChangeFavoriteIcon(isFavorite);
            var bottomFragment = SupportFragmentManager.FindFragmentByTag(Const.BottomSheetFragmentTag) as ProductDetailsFragment;

            //if fragment exists
            if (bottomFragment != null)
            {
                bottomFragment.ChangeFavoriteIcon();
            }
        }

        public void LastUnavailableFavoriteRemoved()
        {
            ChangeFavoriteIcon(false);
            var bottomFragment = SupportFragmentManager.FindFragmentByTag(Const.BottomSheetFragmentTag) as ProductDetailsFragment;

            //if fragment exists
            if (bottomFragment != null)
            {
                bottomFragment.Dismiss();
            }
        }

        /**
         * Sets user's Account information
         */
        public void SetAccountInfo(Account account)
        {
            SetUserBalance(account.Amount.ToString());
        }

        /**
         * Sets user's balance into balance Text View
         */
        public void SetUserBalance(string balance)
        {
            _balance.Visibility = ViewStates.Visible;
            _balance.Text = string.Format(GetString(Resource.String.your_balance_is, balance));
        }

        /**
         * Sets selected machine's name as the Title of ActionBar
         */
        public void SetMachineName(string name)
        {
            SupportActionBar.Title = name;
        }

        /**
         * Creates containers for each category from input categories List
         */
        public void ShowProducts(List<Categories> listCategories)
        {
            HaveProducts = true;
            foreach (var category in listCategories)
            {
                if (category.Name == Const.Favorites)
                {
                    ShowContainer(Resource.Id.favoriteIdLayout, Resource.Id.favorites_container_ID);
                    AttachFragment(category.Name, Resource.Id.favoriteIdLayout, Resource.Id.favorites_container_ID, category.Products);
                }
                else
                {
                    CreateCategory(category.Name, category.Products);
                }
            }
        }
        #endregion

        #region Public Methods
        public void OnRefresh(object sender, EventArgs e)
        {
            HideContainer(Resource.Id.favoriteIdLayout, Resource.Id.favorites_container_ID);
            RemoveContainers();
            _balance.Visibility = ViewStates.Gone;
            ViewPresenter.OnRefresh();
        }

        /**
         * Calls Purchase functionality on Presenters side
         */
        public void Purchase(Product product)
        {
            ViewPresenter.OnBuyProductClick(product);
        }

        /**
         * Attaches BottomSheetFragment with the given product (Preview functionality)
         * Is called by OnLongClick on product item
         */
        public void ShowPreview(Product product)
        {
            BottomSheetDialogFragment bottomSheetDialogFragment = new ProductDetailsFragment(product);
            bottomSheetDialogFragment.Show(SupportFragmentManager, Const.BottomSheetFragmentTag);
        }

        /**
         * Calls navigation to Details screen on Presenter's side
         * with the given product
         * Is called by OnClick on product item
         */
        public void ShowDetails(Product product)
        {
            ViewPresenter.OnProductDetailsClick(product.Id);
        }

        /**
         * Calls Adding/Removing favorite on Presenter's Side
         * Is Called by Fragments (ProductListFragmentVending and ProductDetailsFragment)
         */
        public void TrigFavorite(Product product)
        {
            ViewPresenter.OnFavoriteClick(product);
        }


        public void ShowToastMessage(string message)
        {
            ShowToast(message);
        }

        #endregion

        #region Private Methods
        /**
         * Creates Container and Header for category from dummy layout
         * sets all needed Ids in category (ShowAllId, ContainerId etc.)
         * @param categoryName - Name of current category;
         * @param lsitProducts - list of Produts of current category  
         */
        private void CreateCategory(string categoryName, List<Product> listProducts)
        {
            _viewCounter++;

            var mainLayout = FindViewById<LinearLayout>(Resource.Id.layout_root);

            var ll = LayoutInflater.Inflate(Resource.Layout.category_header_layout, null) as LinearLayout;

            mainLayout?.AddView(ll);

            var llHeader = FindViewById<LinearLayout>(Resource.Id.dummyHeaderID);
            if (llHeader != null)
            {
                llHeader.Id = View.GenerateViewId();
            }

            var tvCategoryName = FindViewById<TextView>(Resource.Id.categoryName);
            if (tvCategoryName != null)
            {
                tvCategoryName.Id = View.GenerateViewId();
                tvCategoryName.Text = categoryName;
            }

            var tvSeeAll = FindViewById<TextView>(Resource.Id.dummySeeAllID);
            if (tvSeeAll != null)
            {
                tvSeeAll.Id = View.GenerateViewId();
            }

            var llContainer = FindViewById<LinearLayout>(Resource.Id.container_dummyID);
            if (llContainer != null && llHeader != null)
            {
                llContainer.Id = View.GenerateViewId();

            }

            if (tvSeeAll != null)
            {
                tvSeeAll.Click += (sender, e) =>
                {
                    ViewPresenter.OnShowAllClick(categoryName);
                };
            }

            if (llHeader != null && llContainer != null && tvSeeAll != null)
            {
                AttachFragment(categoryName, llHeader.Id, llContainer.Id, listProducts);
            }
        }

        /**
         * Hides conatiner and Header for category which becames empty
         * e.g. After removing last favorite from Favorites category
         */
        private void HideContainer(int headers, int fragmentContainerId)
        {
            var view = FindViewById<View>(headers);
            var fragmentContainer = FindViewById<View>(fragmentContainerId);
            if (fragmentContainer != null)
                fragmentContainer.Visibility = ViewStates.Gone;
            if (view != null)
                view.Visibility = ViewStates.Gone;
        }

        public void ShowContainer(int headers, int fragmentContainerId)
        {
            var view = FindViewById<View>(headers);
            var fragmentContainer = FindViewById<View>(fragmentContainerId);

            if (view != null)
            {
                view.Visibility = ViewStates.Visible;
            }
            if (fragmentContainer != null)
            {
                fragmentContainer.Visibility = ViewStates.Visible;
            }
        }

        /**
         * Removes all created containers
         * Is Used for refreshing layout
         */
        private void RemoveContainers()
        {
            var layout = FindViewById<LinearLayout>(Resource.Id.layout_root);
            for (var i = 0; i < _viewCounter; i++)
            {
                layout?.RemoveView(FindViewById<View>(Resource.Id.categoryLayout));
            }
        }

        /**
         * Attach fragment to created container for category.
         * @param categoryName - Name of current category;
         * @param headerID - Id of category Header (category name, ShowAllButton)
         * @param containerID - Id of container for fragment
         * @param lsitProducts - list of Produts of current category
         */
        private void AttachFragment(string categoryName, int headerId, int containerId, List<Product> listProducts)
        {
            FragmentManager.BeginTransaction()
                .Replace(containerId, ProductListFragmentVending.NewInstance(categoryName, headerId, containerId, listProducts),
                 categoryName)
                .Commit();
        }

        private void ChangeFavoriteIcon(bool isFavorite)
        {
            //Take Favorites from ProductList
            var refreshedFavorites = ViewPresenter.GetProductListForGivenCategory(Const.Favorites);

            // Taking containerId and HeaderId of favorite from Dicitionary
            var favoritesContainerId = _containerIds.ElementAt(0).Key;
            var favoriteHeaderID = _containerIds.ElementAt(0).Value;

            //Try to find fragment corresponding with containerID
            var fragment = FragmentManager.FindFragmentById(favoritesContainerId) as ProductListFragmentVending;

            //if fragment exists
            if (fragment != null)
            {
                //if Count of favorites is 0 then hide container
                if (refreshedFavorites.Count == 0)
                {
                    HideContainer(favoritesContainerId, favoriteHeaderID);
                }
                //if count of favorite > 0 then show container and trig method ChangeFavorite in corresponding fragment
                else ShowContainer(favoritesContainerId, favoriteHeaderID);
                fragment.ChangeFavorite(refreshedFavorites);
            }
            // if there is no fragment for such container then
            //show container and attach fragment to it.
            else 
            {
                if (refreshedFavorites.Count <= 0) return;
                ShowContainer(favoritesContainerId, _containerIds.ElementAt(0).Value);
                AttachFragment(Const.Favorites, _containerIds.ElementAt(0).Value, favoritesContainerId,
                    refreshedFavorites);
            }
            
        }
        #endregion
    }
}