using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;
using AndroidX.ViewPager.Widget;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Droid.ui.adapters;
using Softjourn.SJCoins.Droid.ui.baseUI;

namespace Softjourn.SJCoins.Droid.UI.Activities
{
    [Activity(Label = "WelcomeActivity", Theme = "@style/NoActionBarLoginTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class WelcomeActivity : BaseActivity<WelcomePresenter>, ViewPager.IOnPageChangeListener, IWelcomeView
    {

        private ViewPager _viewPager;
        private LinearLayout _dotsLayout;
        private Button _btnSkip;
        private Button _btnNext;

        private TextView[] _dots;
        private ViewPagerAdapter _viewPagerAdapter;
        private int[] _layouts;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Making notification bar transparent
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                Window.DecorView.SystemUiVisibility =
                    (StatusBarVisibility)(SystemUiFlags.LayoutStable | SystemUiFlags.LayoutFullscreen);
            }

            SetContentView(Resource.Layout.activity_welcome);

            _viewPager = FindViewById<ViewPager>(Resource.Id.view_pager);
            _dotsLayout = FindViewById<LinearLayout>(Resource.Id.layoutDots);
            _btnSkip = FindViewById<Button>(Resource.Id.btn_skip);
            _btnNext = FindViewById<Button>(Resource.Id.btn_next);

            // _layouts of all welcome sliders
            // add few more _layouts if you want
            _layouts = new[]
            {
                Resource.Layout.welcome_slide1,
                Resource.Layout.welcome_slide2,
                Resource.Layout.welcome_slide3,
                Resource.Layout.welcome_slide4
            };

            AddBottomDots(0);

            ChangeStatusBarColor();

            _viewPagerAdapter = new ViewPagerAdapter(_layouts, this);
            _viewPager.Adapter = _viewPagerAdapter;
            _viewPager.AddOnPageChangeListener(this);

            _btnSkip.Click += (s, e) =>
            {
                ViewPresenter.DisableWelcomePageOnLaunch();
                ViewPresenter.ToLoginScreen();
            };

            _btnNext.Click += (s, e) =>
            {
                // checking for last page
                // if last page home screen will be launched
                var current = GetItem(+1);
                if (current < _layouts.Length) return;
                ViewPresenter.DisableWelcomePageOnLaunch();
                ViewPresenter.ToLoginScreen();
            };
        }

        protected override void OnDestroy()
        {
            ViewPresenter.DetachView();
            base.OnDestroy();
        }

        #region Private Methods
        /**
         * Sets Bottom Dots Colors According to which view is showing right now
         */
        private void AddBottomDots(int currentPage)
        {
            _dots = new TextView[_layouts.Length];

            var colorsActive = Resources.GetIntArray(Resource.Array.array_dot_active);
            var colorsInactive = Resources.GetIntArray(Resource.Array.array_dot_inactive);

            _dotsLayout.RemoveAllViews();
            for (int i = 0; i < _dots.Length; i++)
            {
                _dots[i] = new TextView(this);
                _dots[i].Text = Html.FromHtml("&#8226;", FromHtmlOptions.ModeCompact)?.ToString();
                _dots[i].TextSize = 35;
                _dots[i].SetTextColor(new Color(colorsInactive[currentPage]));
                _dotsLayout.AddView(_dots[i]);
            }

            if (_dots.Length > 0)
                _dots[currentPage].SetTextColor(new Color(colorsActive[currentPage]));
        }

        private int GetItem(int i)
        {
            return _viewPager.CurrentItem + i;
        }

        /**
         * Sets StatusBar Color as transparent 
         * to make allScreenActivity
         */
        private void ChangeStatusBarColor()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                var window = Window;
                window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                window.SetStatusBarColor(Color.Transparent);
            }
        }
        #endregion

        #region ViewPager.IOnPageChangeListener interface implementation

        public void OnPageScrollStateChanged(int state)
        {

        }

        public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
        {

        }

        public void OnPageSelected(int position)
        {
            AddBottomDots(position);

            // changing the next button text 'NEXT' / 'GOT IT'
            if (position == _layouts.Length - 1)
            {
                // last page. make button text to GOT IT
                _btnNext.Text = GetString(Resource.String.start);
                _btnNext.Visibility = ViewStates.Visible;
            }
            else
            {
                _btnNext.Visibility = ViewStates.Gone;
            }
        }
        #endregion
    }
}