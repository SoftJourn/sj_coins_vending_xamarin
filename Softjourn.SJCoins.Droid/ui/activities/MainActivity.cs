
using System;
using System.Collections.Generic;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using BottomNavigationBar;
using Softjourn.SJCoins.Core.API.Model.AccountInfo;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Droid.Services;
using Softjourn.SJCoins.Droid.ui.baseUI;
using Softjourn.SJCoins.Droid.UI.BaseUI;
using Softjourn.SJCoins.Droid.UI.Fragments;
using Softjourn.SJCoins.Droid.Utils;

namespace Softjourn.SJCoins.Droid.UI.Activities
{
    [Activity(Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : BaseActivity<HomePresenter>, IHomeView
    {

        private SwipeRefreshLayout _swipeLayout;
        private int _viewCounter = 0;
        private View _headerView;
        private Account _account;
        private List<Categories> _listCategories;
        private TextView _balance;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            //_menuLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            //_menuView = FindViewById<NavigationView>(Resource.Id.left_side_menu);
   
            _swipeLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_container);
            _swipeLayout.SetColorSchemeResources(Resource.Color.colorAccent);
            _swipeLayout.Refresh += OnRefresh;

            //var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar_base);
            //SetSupportActionBar(toolbar);

            _balance = FindViewById<TextView>(Resource.Id.balance);

            //_bottomBar = BottomBar.AttachShy(FindViewById<CoordinatorLayout>(Resource.Id.coordinator_root_layout),
            //   FindViewById(Resource.Id.scroll_layout), savedInstanceState);

            _swipeLayout.Refreshing = true;
            ViewPresenter.OnStartLoadingPage();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    //_menuLayout.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                    return true;
                case Resource.Id.menu_favorites:
                    //ViewPresenter.OnFavoritesButtonClick();
                    return true;
                case Resource.Id.profile:
                    ViewPresenter.OnProfileButtonClicked();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }


        public void OnRefresh(object sender, EventArgs e)
        {
            RemoveContainers();
            ViewPresenter.OnRefresh();
        }

        //public override void SetBalance(View headerView)
        //{
        //    var userBalanceView = headerView.FindViewById<TextView>(Resource.Id.user_balance);
        //    userBalanceView.Text = _account != null ? _account.Amount.ToString() : "";
        //}

        //public override void SetUserName(View headerView)
        //{
        //    var userNameView = headerView.FindViewById<TextView>(Resource.Id.menu_user_name);
        //    userNameView.Text = _account != null ? _account.Name + " " + _account.Surname : "";
        //}

        //public override bool HandleNavigation(IMenuItem item)
        //{
        //    var id = item.ItemId;
        //    switch (id)
        //    {
        //        case Resource.Id.menu_all_products:
        //            //mMenuLayout.closeDrawer(GravityCompat.START);
        //            //NavigationUtils.GoToSeeAllActivity(this, Const.AllItems);
        //            break;
        //        case Resource.Id.menu_favorites:
        //            //mMenuLayout.closeDrawer(GravityCompat.START);
        //            //NavigationUtils.GoToSeeAllActivity(this, Const.Favorites);
        //            break;
        //        case Resource.Id.menu_last_added:
        //            //mMenuLayout.closeDrawer(GravityCompat.START);
        //            //NavigationUtils.GoToSeeAllActivity(this, Const.LastAdded);
        //            break;
        //        case Resource.Id.menu_best_sellers:
        //            //mMenuLayout.closeDrawer(GravityCompat.START);
        //            item.SetChecked(false);
        //            //NavigationUtils.GoToSeeAllActivity(this, Const.BestSellers);
        //            break;
        //        case Resource.Id.menu_logout_item:
        //            LogOut();
        //            //mMenuLayout.closeDrawer(GravityCompat.START);
        //            break;
        //        default:
        //            //_headerView.SetItem(item.ItemId());
        //            OnCategorySelected(item);
        //            //mMenuLayout.closeDrawer(mMenuView, true);
        //            break;
        //    }
        //    return true;
        //}

        //public override void LogOut()
        //{
        //    //ViewPresenter.LogOut();
        //}


        public void ShowToastMessage(string message)
        {
            ShowToast(message);
        }

        //public override void OnCategorySelected(IMenuItem item)
        //{
        //    //NavigationUtils.GoToSeeAllActivity(this, item.TitleFormatted.ToString());
        //}

        //public override void SetUpNavigationViewContent(NavigationView menuView)
        //{
        //    var leftSideMenuController = new LeftSideMenuController(menuView);
        //    leftSideMenuController.UnCheckAllMenuItems(menuView);
        //    if (_listCategories != null)
        //    {
        //        leftSideMenuController.AddCategoriesToMenu(GetMenu(), _listCategories);
        //    }
        //}

        public override void ShowProgress(string message)
        {
            _swipeLayout.Refreshing = true;
        }

        public override void HideProgress()
        {
            base.HideProgress();
            _swipeLayout.Refreshing = false;
        }

        private void AttachFragment(string categoryName, int headerId, int containerId, int seeAllId, List<Product> listProducts )
        {
            FragmentManager.BeginTransaction()
                .Replace(containerId, ProductListFragmentVending.NewInstance(categoryName, headerId, containerId, listProducts),
                 categoryName)
                .Commit();
        }

        public void CreateCategory(string categoryName, List<Product> listProducts)
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
            if (llContainer != null)
            {
                llContainer.Id = View.GenerateViewId();
            }

            if (tvSeeAll != null)
            {
                tvSeeAll.Click += (sender, e) =>
                {
                    //NavigationUtils.GoToSeeAllActivity(this, categoryName);
                };
            }

            if (llHeader != null && llContainer != null && tvSeeAll != null)
            {
                AttachFragment(categoryName, llHeader.Id, llContainer.Id, tvSeeAll.Id, listProducts);
            }
        }

        public void HideContainer(int headers, int fragmentContainerId)
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

        private void RemoveContainers()
        {
            var layout = FindViewById<LinearLayout>(Resource.Id.layout_root);
            for (var i = 0; i < _viewCounter; i++)
            {
                layout?.RemoveView(FindViewById<View>(Resource.Id.categoryLayout));
            }
        }

        public void SetAccountInfo(Account account)
        {
            _account = account;
            _balance.Visibility = ViewStates.Visible;
            _balance.Text = string.Format(GetString(Resource.String.your_balance_is, account.Amount));
        }

        public void SetUserBalance(string balance)
        {
            _balance.Visibility = ViewStates.Visible;
            _balance.Text = string.Format(GetString(Resource.String.your_balance_is, balance));
        }

        public void SetMachineName(string name)
        {
            SupportActionBar.Title = name;
        }

        public void ShowProducts(List<Categories> listCategories)
        {
            _listCategories = listCategories;
            foreach (var category in listCategories)
            {
                CreateCategory(category.Name, category.Products);
            }
        }

        public void ShowPurchaseConfirmationDialog(Product product)
        {
            //throw new System.NotImplementedException();
        }

        public void Purchase(Product product)
        {
            ViewPresenter.OnProductClick(product);
        }

        public void ShowDetails(Product product)
        {
            BottomSheetDialogFragment bottomSheetDialogFragment = new ProductDetailsFragment(product, _listCategories);
            bottomSheetDialogFragment.Show(SupportFragmentManager, bottomSheetDialogFragment.Tag);
        }

        public void TrigFavorite(Product product)
        {
            ViewPresenter.OnFavoriteClick(product);
        }

        //public override void HandleMenuNavigation(int menuItemId)
        //{
        //    switch (menuItemId)
        //    {
        //        case Resource.Id.home:
        //            Toast.MakeText(this, "Home", ToastLength.Long).Show();
        //            break;
        //        case Resource.Id.profile:
        //            Toast.MakeText(this, "Profile", ToastLength.Long).Show();
        //            break;
        //    }
        //}
    }
}