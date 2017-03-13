
using System.Collections.Generic;

using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Droid.ui.baseUI;
using Softjourn.SJCoins.Droid.UI.Adapters;
using Softjourn.SJCoins.Droid.Utils;

namespace Softjourn.SJCoins.Droid.UI.Activities
{
    [Activity(Label = "Details", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class DetailsActivity : BaseActivity<DetailPresenter>, IDetailView
    {
        private Product _product;
        private ViewPager _viewPager;
        private const string ProductID = Const.NavigationKey;
        private TextView _productPrice;
        private TextView _productDescription;
        private List<string> _images;
        private IMenu _menu;

        private DetailsPagerAdapter _adapter;

        #region Public  Methods
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_details);

            _product = ViewPresenter.GetProduct(int.Parse(Intent.Extras.Get(ProductID).ToString()));

            _images = new List<string>();

            //TODO: Need to make loop for adding list of photos when it will be ready on backend
            if (_product.ImageUrls != null)
            {
                _images.AddRange(_product.ImagesFullUrls);
            }
            else
            {
                _images.Add(_product.ImageFullUrl);
            }

            _adapter = new DetailsPagerAdapter(this, _images);

            //View Pager for viewing photos by swiping 
            //and adapter for it
            _viewPager = FindViewById<ViewPager>(Resource.Id.viewPager);
            _viewPager.Adapter = _adapter;

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
            _menu = menu;
            menu.FindItem(Resource.Id.profile).SetVisible(false);
            menu.FindItem(Resource.Id.menu_buy).SetVisible(true);
            menu.FindItem(Resource.Id.menu_add_favorite).SetVisible(true);
            ChangeIcon(menu.FindItem(Resource.Id.menu_add_favorite), _product.IsProductFavorite);
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
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        public void FavoriteChanged(bool isFavorite)
        {
            ChangeIcon(_menu.FindItem(Resource.Id.menu_add_favorite), isFavorite);
        }

        #endregion

        #region Private Methods
        /**
         * Sets Icon of Favorite according to callback from Presenter
         */
        private void ChangeIcon(IMenuItem item, bool isFavorite)
        {
            item.SetIcon(isFavorite ? Resource.Drawable.ic_favorite_white_24dp : Resource.Drawable.ic_favorite_border_white);
        }
        #endregion
    }
}
