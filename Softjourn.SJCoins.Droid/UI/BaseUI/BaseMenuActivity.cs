using Android.App;
using Android.OS;
using BottomNavigationBar;
using Java.Lang;
using Softjourn.SJCoins.Core.UI.Presenters.IPresenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Droid.ui.baseUI;

namespace Softjourn.SJCoins.Droid.UI.BaseUI
{
    [Activity(Label = "BaseMenuActivity")]
    public abstract class BaseMenuActivity<TPresenter> : BaseActivity<TPresenter>, IBaseView, BottomNavigationBar.Listeners.IOnMenuTabClickListener
        where TPresenter : class, IBasePresenter
    {
        public BottomBar _bottomBar;
        //public DrawerLayout _menuLayout;
        //public NavigationView _menuView;
        //private ActionBarDrawerToggle _menuToggle;

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);

            if (_bottomBar == null)
            {
                throw new IllegalStateException("Activity must have BottomBar view");
            }

            //if (_menuView == null)
            //{
            //    throw new IllegalStateException("Activity must have view with left_side_menu id");
            //}

            //SetUpNavigationViewContent(_menuView);
            InitBottomBar(savedInstanceState);
            //InitNavigationDrawer();
        }

        private void InitBottomBar(Bundle bundle)
        {
            _bottomBar = BottomBar.Attach(this, bundle);
            _bottomBar.SetItems(Resource.Menu.menu_bottom);
            _bottomBar.SetOnMenuTabClickListener(this);
        }

        public abstract void HandleMenuNavigation(int menuItemId);

        //protected override void OnPostCreate(Bundle savedInstanceState)
        //{
        //    base.OnPostCreate(savedInstanceState);

        //    if (_menuLayout == null)
        //    {
        //        throw new IllegalStateException("Activity must have DrawerLayout view");
        //    }

        //    if (_menuView == null)
        //    {
        //        throw new IllegalStateException("Activity must have view with left_side_menu id");
        //    }

        //    SetUpNavigationViewContent(_menuView);
        //    InitActionBarToggle();
        //    InitNavigationDrawer();
        //}

        //private void InitActionBarToggle()
        //{
        //    var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar_base);
        //    SetSupportActionBar(toolbar);

        //    _menuToggle = new ActionBarDrawerToggle(
        //            this, _menuLayout, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);

        //    _menuLayout.AddDrawerListener(_menuToggle);
        //    if (SupportActionBar != null)
        //    {
        //        SupportActionBar.SetDisplayHomeAsUpEnabled(true);
        //        SupportActionBar.SetHomeButtonEnabled(true);
        //    }

        //    _menuLayout.AddDrawerListener(_menuToggle);
        //    _menuToggle.SyncState();

        //}

        //private void InitNavigationDrawer()
        //{
        //    var headerView = _menuView.GetHeaderView(0);
        //    _menuView.NavigationItemSelected += (sender, e) =>
        //    {
        //        e.MenuItem.SetChecked(true);
        //        HandleNavigation(e.MenuItem);
        //        _menuLayout.CloseDrawers();
        //    };


        //    _menuLayout.DrawerOpened += (s, e) =>
        //    {
        //        SetBalance(headerView);
        //        SetUserName(headerView);
        //        SetUpNavigationViewContent(_menuView);
        //    };

        //    headerView.SetBackgroundColor(new Color(ContextCompat.GetColor(this, Resource.Color.colorBlue)));
        //    headerView.Click += (s, e) =>
        //    {
        //        _menuLayout.CloseDrawer(GravityCompat.Start);
        //        //Navigation.goToProfileActivity(BaseMenuActivity.this);
        //    };

        //    var iconView = headerView.FindViewById<ImageView>(Resource.Id.menu_user_icon);
        //    iconView.SetImageResource(Resource.Drawable.logo);

        //    SetBalance(headerView);
        //    SetUserName(headerView);
        //}

        //public abstract void SetBalance(View headerView);

        //public abstract void SetUserName(View headerView);

        //public abstract bool HandleNavigation(IMenuItem item);

        //public abstract void LogOut();

        //public abstract void OnCategorySelected(IMenuItem item);

        //public abstract void SetUpNavigationViewContent(NavigationView menuView);


        //public override void OnConfigurationChanged(Configuration newConfig)
        //{
        //    base.OnConfigurationChanged(newConfig);
        //    _menuToggle?.OnConfigurationChanged(newConfig);
        //}

        //public override bool OnOptionsItemSelected(IMenuItem item)
        //{
        //    return _menuToggle != null && _menuToggle.OnOptionsItemSelected(item) || base.OnOptionsItemSelected(item);
        //}

        //protected IMenu GetMenu()
        //{
        //    return _menuView.Menu;
        //}

        //public override void OnBackPressed()
        //{
        //    if (_menuLayout.IsDrawerOpen(GravityCompat.Start))
        //    {
        //        _menuLayout.CloseDrawer(GravityCompat.Start);
        //    }
        //    else
        //    {
        //        base.OnBackPressed();
        //    }
        //}

        public void OnMenuTabSelected(int menuItemId)
        {
            HandleMenuNavigation(menuItemId);
        }

        public void OnMenuTabReSelected(int menuItemId)
        { 
        }
    }
}