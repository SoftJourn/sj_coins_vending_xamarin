
using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Droid.ui.baseUI;
using Softjourn.SJCoins.Droid.utils;
using Softjourn.SJCoins.Droid.UI.BaseUI;
using Softjourn.SJCoins.Droid.UI.Fragments;

namespace Softjourn.SJCoins.Droid.UI.Activities
{
    [Activity(Label = "Vending Machine", Theme = "@style/AppThemeForCustomToolbar")]
    public class MainActivity : BaseMenuActivity<MainPresenter>, IMainView
    {

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

            _swipeLayout.Refreshing = true;

            Title = "Vending Machine";
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    _menuLayout.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
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

        public override void SetBalance(View headerView)
        {
            var userBalanceView = headerView.FindViewById<TextView>(Resource.Id.user_balance);
            userBalanceView.Text = ViewPresenter.GetUserBalance() ?? (string) "";
        }

        public override void SetUserName(View headerView)
        {
            var userNameView = headerView.FindViewById<TextView>(Resource.Id.menu_user_name);
            userNameView.Text = ViewPresenter.GetUserName() ?? (string)"";
        }

        public override bool HandleNavigation(IMenuItem item)
        {
            var id = item.ItemId;
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
                    LogOut();
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

        public override void LogOut()
        {
            ViewPresenter.LogOut();
        }


        public void ShowToastMessage(string message)
        {
            ShowToast(message);
        }

        public override void OnCategorySelected(IMenuItem item)
        {
            //NavigationUtils.GoToSeeAllActivity(this, item.TitleFormatted.ToString());
        }

        public override void SetUpNavigationViewContent()
        {
            //LeftSideMenuController leftSideMenuController = new LeftSideMenuController(mMenuView);
            //leftSideMenuController.unCheckAllMenuItems(mMenuView);
            //leftSideMenuController.addCategoriesToMenu(getMenu(), mVendingPresenter.getCategories());
        }

        public new void HideProgress()
        {
            base.HideProgress();
            _swipeLayout.Refreshing = false;
        }

        private void AttachFragment(string categoryName, int headerId, int containerId, int seeAllId)
        {
            FragmentManager.BeginTransaction()
                .Replace(containerId, ProductListFragment.NewInstance(categoryName, headerId, containerId),
                 Preferences.RetrieveStringObject(categoryName.ToUpper()))
                .Commit();
        }

        public void CreateCategory(string categoryName)
        {
            _viewCounter++;

            var mainLayout = FindViewById<LinearLayout>(Resource.Id.layout_root);

            var inflater = Application.Context.GetSystemService(LayoutInflaterService) as LayoutInflater;

            var ll = (LinearLayout)inflater.Inflate(Resource.Layout.category_header_layout, null);

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
            for (var i = 0; i < _viewCounter; i++)
            {
                layout?.RemoveView(FindViewById<View>(Resource.Id.categoryLayout));
            }
        }

        public void OnCreateErrorDialog(string message)
        {
            //base.OnCreateErrorDialog(message);
            _swipeLayout.Refreshing = false;
        }
    }
}