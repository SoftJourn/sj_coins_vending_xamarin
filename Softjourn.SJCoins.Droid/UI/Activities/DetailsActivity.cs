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
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Droid.ui.baseUI;
using Softjourn.SJCoins.Droid.UI.Adapters;
using Softjourn.SJCoins.Droid.Utils;
using Square.Picasso;

namespace Softjourn.SJCoins.Droid.UI.Activities
{
    [Activity(Label = "Details", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class DetailsActivity: BaseActivity<DetailPresenter>, IDetailView
    {
        private Product _product;
        private ViewPager _viewPager;
        private const string ProductID = Const.NavigationKey;
        private TextView _productPrice;
        private TextView _productDescription;
        private List<string> _images; 

        private DetailsPagerAdapter adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_details);

            _product = ViewPresenter.GetProduct(int.Parse(Intent.Extras.Get(ProductID).ToString()));

            _images = new List<string>();

            _images.Add(_product.ImageFullUrl);
            adapter = new DetailsPagerAdapter(this, _images);

            _viewPager = FindViewById<ViewPager>(Resource.Id.viewPager);
            _viewPager.Adapter = adapter;

            _productPrice = FindViewById<TextView>(Resource.Id.details_product_price);
            _productPrice.Text = _product.IntPrice + " coins";

            _productDescription = FindViewById<TextView>(Resource.Id.details_product_description);
            _productDescription.Text = _product.Description;

            Title = _product.Name;

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            base.OnCreateOptionsMenu(menu);
            menu.FindItem(Resource.Id.profile).SetVisible(false);
            menu.FindItem(Resource.Id.menu_favorites).SetVisible(false);
            menu.FindItem(Resource.Id.menu_buy).SetVisible(true);
            menu.FindItem(Resource.Id.menu_add_favorite).SetVisible(true);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    OnBackPressed();
                    break;
                case Resource.Id.menu_buy:
                    ViewPresenter.OnBuyProductClick(_product);
                    break;
                case Resource.Id.menu_add_favorite:
                    ViewPresenter.OnFavoriteClick(_product);
                    ChangeIcon(item);
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void ChangeIcon(IMenuItem item)
        {
            if (_product.IsProductFavorite)
            {
                item.SetIcon(Resource.Drawable.ic_favorite_white_24dp);
            }
            else
            {
                item.SetIcon(Resource.Drawable.ic_favorite_border_white);
            }
        }
    }
}