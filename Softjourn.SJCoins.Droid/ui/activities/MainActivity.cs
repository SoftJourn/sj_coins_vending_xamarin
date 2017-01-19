using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using Softjourn.SJCoins.Core.API.Model.Machines;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Droid.ui.adapters;
using Softjourn.SJCoins.Droid.ui.baseUI;
using Softjourn.SJCoins.Droid.utils;

namespace Softjourn.SJCoins.Droid.ui.activities
{
    [Activity(Label = "Vending Machine", Theme = "@style/AppThemeForCustomToolbar")]
    public class MainActivity : BaseActivity<MainPresenter>, IMainView
    {

        private DrawerLayout _drawerLayout;
        private NavigationView _navigationView;
        private ActionBarDrawerToggle _toggle;
        private SwipeRefreshLayout _swipeLayout;
        private int _viewCounter = 0;
        private View _headerView;

        private TextView _seeAllFavoriteTextView;
        private TextView _seeAllLastAddedTextView;
        private TextView _seeAllBestSellerTextView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            _swipeLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_container);
            _swipeLayout.SetColorSchemeColors(GetColor(Resource.Color.colorAccent));

            _drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            _navigationView = FindViewById<NavigationView>(Resource.Id.left_side_menu);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar_base);
            SetSupportActionBar(toolbar);

            _seeAllFavoriteTextView = FindViewById<TextView>(Resource.Id.textViewFavoritesSeeAll);
            _seeAllLastAddedTextView = FindViewById<TextView>(Resource.Id.textViewLastAddedSeeAll);
            _seeAllBestSellerTextView = FindViewById<TextView>(Resource.Id.textViewBestSellersSeeAll);

            _seeAllFavoriteTextView.Click += (sender, e) =>
            {
                //ViewPresenter.GoToSeeAllActivity(this, Const.Favorites);
            };

            _seeAllBestSellerTextView.Click += (sender, e) =>
            {
                //ViewPresenter.GoToSeeAllActivity(this, Const.BestSellers);
            };

            _seeAllLastAddedTextView.Click += (sender, e) =>
            {
                //ViewPresenter.GoToSeeAllActivity(this, Const.LastAdded);
            };

            InitDrawerToggle();

            _headerView = _navigationView.GetHeaderView(0);
            _navigationView.NavigationItemSelected += (sender, e) =>
            {
                e.MenuItem.SetChecked(true);
                HandleNavigation(e.MenuItem);
                _drawerLayout.CloseDrawers();
            };

            _swipeLayout.Refreshing = true;

            Title = "Vending Machine";
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    _drawerLayout.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                    return true;
                case Resource.Id.select_machine:
                    //ViewPresenter.getMachinesList();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }


        public void OnRefresh()
        {
            if (TextUtils.IsEmpty(Preferences.RetrieveStringObject(Const.SelectedMachineId)))
            {
                ShowToastMessage(GetString(Resource.String.machine_not_selected_toast));
                _swipeLayout.Refreshing = false;
            }
            else
            {
                RemoveContainers();
                //LoadProductList();
            }
        }

        private bool HandleNavigation(IMenuItem item)
        {

            int id = item.ItemId;
            switch (id)
            {
                case Resource.Id.menu_all_products:
                    //mMenuLayout.closeDrawer(GravityCompat.START);
                    //NavigationUtils.GoToSeeAllActivity(this, Const.AllItems);
                    break;
                case Resource.Id.menu_favorites:
                    //mMenuLayout.closeDrawer(GravityCompat.START);
                    //NavigationUtils.GoToSeeAllActivity(this, Const.Favorites);
                    break;
                case Resource.Id.menu_last_added:
                    //mMenuLayout.closeDrawer(GravityCompat.START);
                    //NavigationUtils.GoToSeeAllActivity(this, Const.LastAdded);
                    break;
                case Resource.Id.menu_best_sellers:
                    //mMenuLayout.closeDrawer(GravityCompat.START);
                    item.SetChecked(false);
                    //NavigationUtils.GoToSeeAllActivity(this, Const.BestSellers);
                    break;
                case Resource.Id.menu_logout_item:
                    //ViewPresenter.LogOut();
                    //mMenuLayout.closeDrawer(GravityCompat.START);
                    break;
                default:
                    //_headerView.SetItem(item.ItemId());
                    OnCategorySelected(item);
                    //mMenuLayout.closeDrawer(mMenuView, true);
                    break;
            }
            return true;
        }


        public void ShowToastMessage(string message)
        {
            ShowToast(message);
        }

        private void InitDrawerToggle()
        {
            _toggle = new ActionBarDrawerToggle(
                this, _drawerLayout, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);

            if (SupportActionBar != null)
            {
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);
            }

            _drawerLayout.AddDrawerListener(_toggle);

            _toggle.SyncState();
        }

        public void OnCategorySelected(IMenuItem item)
        {
            //NavigationUtils.GoToSeeAllActivity(this, item.TitleFormatted.ToString());
        }

        public new void HideProgress()
        {
            base.HideProgress();
            _swipeLayout.Refreshing = false;
        }

        private void AttachFragment(string categoryName, int headerId, int containerId, int seeAllId)
        {
            //FragmentManager.BeginTransaction()
            //    .Replace(containerId, ProductListFragment.NewInstance(categoryName, headerId, containerId),
            //        Preferences.RetrieveStringObject(categoryName.ToUpper()))
            //    .Commit();
        }

        public void CreateContainer(string categoryName)
        {
            _viewCounter++;

            var mainLayout = FindViewById<LinearLayout>(Resource.Id.layout_root);

            LinearLayout ll;

            var inflater = Application.Context.GetSystemService(LayoutInflaterService) as LayoutInflater;

            ll = (LinearLayout)inflater.Inflate(Resource.Layout.category_header_layout, null);

            if (mainLayout != null)
            {
                mainLayout.AddView(ll);
            }

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
                AttachFragment(categoryName, llHeader.Id, llContainer.Id, tvSeeAll.Id);
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
            for (int i = 0; i < _viewCounter; i++)
            {
                if (layout != null)
                {
                    layout.RemoveView(FindViewById<View>(Resource.Id.categoryLayout));
                }
            }
        }

        public void OnCreateErrorDialog(string message)
        {
            //base.OnCreateErrorDialog(message);
            _swipeLayout.Refreshing = false;
        }
    }
}