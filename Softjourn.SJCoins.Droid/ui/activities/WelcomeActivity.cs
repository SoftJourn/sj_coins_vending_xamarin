using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.Presenters.IPresenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Droid.ui.adapters;
using Softjourn.SJCoins.Droid.utils;

namespace Softjourn.SJCoins.Droid.ui.activities
{
    [Activity(Label = "WelcomeActivity", Theme = "@style/NoActionBarLoginTheme")]
    public class WelcomeActivity : AppCompatActivity, ViewPager.IOnPageChangeListener, IWelcomeView
    {

        private ViewPager _viewPager;
        private LinearLayout _dotsLayout;
        private Button _btnSkip;
        private Button _btnNext;

        private TextView[] _dots;
        private ViewPagerAdapter _viewPagerAdapter;
        private int[] _layouts;

        private IWelcomePresenter _presenter;


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

            _presenter = new WelcomePresenter(this);

            // _layouts of all welcome sliders
            // add few more _layouts if you want
            _layouts = new int[]
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
                _presenter.DisableWelcomePageOnLaunch();
                _presenter.ToLoginScreen();
            };

            _btnNext.Click += (s, e) =>
            {
                // checking for last page
                // if last page home screen will be launched
                var current = GetItem(+1);
                if (current >= _layouts.Length)
                {
                    _presenter.DisableWelcomePageOnLaunch();
                    _presenter.ToLoginScreen();
                }
            };
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _presenter = null;
        }

        private void AddBottomDots(int currentPage)
        {
            _dots = new TextView[_layouts.Length];

            var colorsActive = Resources.GetIntArray(Resource.Array.array_dot_active);
            var colorsInactive = Resources.GetIntArray(Resource.Array.array_dot_inactive);

            _dotsLayout.RemoveAllViews();
            for (int i = 0; i < _dots.Length; i++)
            {
                _dots[i] = new TextView(this);
                _dots[i].Text = Html.FromHtml("&#8226;").ToString();
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

        private void ChangeStatusBarColor()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                Window window = Window;
                window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                window.SetStatusBarColor(Color.Transparent);
            }
        }


        /**
         * ViewPager.IOnPageChangeListener interface implementation
         */
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
        }

        /**
         * IWelcomeView interface implementation
         */
        public void ToLoginPage()
        {
            NavigationUtils.GoToLoginActivity(this);
            Finish();
        }
    }
}