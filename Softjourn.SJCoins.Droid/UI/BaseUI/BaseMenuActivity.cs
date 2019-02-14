using Android.App;
using Android.OS;
using BottomNavigationBar;
using Java.Lang;
using Softjourn.SJCoins.Core.UI.Presenters.Interfaces;
using Softjourn.SJCoins.Droid.ui.baseUI;

namespace Softjourn.SJCoins.Droid.UI.BaseUI
{
    [Activity(Label = "BaseMenuActivity")]
    public abstract class BaseMenuActivity<TPresenter> : BaseActivity<TPresenter>, BottomNavigationBar.Listeners.IOnMenuTabClickListener
        where TPresenter : class, IBasePresenter
    {
        public BottomBar _bottomBar;

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);

            if (_bottomBar == null)
                throw new IllegalStateException("Activity must have BottomBar view");

            InitBottomBar(savedInstanceState);
        }

        private void InitBottomBar(Bundle bundle)
        {
            _bottomBar = BottomBar.Attach(this, bundle);
            _bottomBar.SetItems(Resource.Menu.menu_bottom);
            _bottomBar.SetOnMenuTabClickListener(this);
        }

        public abstract void HandleMenuNavigation(int menuItemId);

        public void OnMenuTabSelected(int menuItemId) => HandleMenuNavigation(menuItemId);

        public void OnMenuTabReSelected(int menuItemId)
        {
        }
    }
}