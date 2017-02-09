using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Softjourn.SJCoins.Core.UI.Controllers.Main;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Droid.ui.baseUI;
using Softjourn.SJCoins.Droid.UI.Adapters;

namespace Softjourn.SJCoins.Droid.UI.Activities
{
    [Activity(Label = "Details", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class DetailsActivity: BaseActivity<DetailPresenter>, IDetailView
    {
        private ViewPager _viewPager;
        private int[] _images = {Resource.Drawable.get_money,Resource.Drawable.like,Resource.Drawable.noavatar,Resource.Drawable.ic_t_rex};

        private DetailsPagerAdapter adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_details);
            adapter = new DetailsPagerAdapter(this, _images);

            _viewPager = FindViewById<ViewPager>(Resource.Id.viewPager);
            _viewPager.Adapter = adapter;

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            return false;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
                this.OnBackPressed();

            return base.OnOptionsItemSelected(item);
        }
    }
}