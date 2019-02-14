using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;
using Softjourn.SJCoins.Core.Common;
using Softjourn.SJCoins.Core.Models.Products;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Droid.ui.baseUI;
using Softjourn.SJCoins.Droid.UI.Adapters;

namespace Softjourn.SJCoins.Droid.UI.Activities
{
    [Activity(Label = "Details", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class DetailsActivity : BaseActivity<DetailPresenter>, IDetailView
    {
        private Product _product;
        private ViewPager _viewPager;
        private const string ProductId = Constant.NavigationKey;
        private TextView _productPrice;
        private TextView _productDescription;
        private List<string> _images;
        private IMenu _menu;
        private TextView[] _dots;
        private LinearLayout _dotsLayout;

        private DetailsPagerAdapter _pagerAdapter;
        private NutritionFactsAdapter _nutritionFactsAdapter;

        private RecyclerView.LayoutManager _layoutManager;
        private RecyclerView _nutritionItems;
        private TextView _textViewNoNutritionFacts;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_details);

            _product = ViewPresenter.GetProduct(int.Parse(Intent.Extras.Get(ProductId).ToString()));
            _images = new List<string>();

            //TODO: Need to make loop for adding list of photos when it will be ready on backend
            if (_product.ImageUrls != null)
                _images.AddRange(_product.ImagesFullUrls);
            else
                _images.Add(_product.ImageFullUrl);

            _pagerAdapter = new DetailsPagerAdapter(this, _images);
            _dotsLayout = FindViewById<LinearLayout>(Resource.Id.layoutDots);
            AddBottomDots(0);

            //View Pager for viewing photos by swiping 
            //and adapter for it
            _viewPager = FindViewById<ViewPager>(Resource.Id.viewPager);
            _viewPager.Adapter = _pagerAdapter;
            _viewPager.PageSelected += (s, e) =>
            {
                AddBottomDots(e.Position);
            };

            _productPrice = FindViewById<TextView>(Resource.Id.details_product_price);
            _productPrice.Text = $"{_product.IntPrice} coins";

            _productDescription = FindViewById<TextView>(Resource.Id.details_product_description);
            _productDescription.Text = _product.Description;

            Title = _product.Name;

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            _textViewNoNutritionFacts = FindViewById<TextView>(Resource.Id.textViewNoNutritionFacts);

            if (!_product.NutritionFacts.Any() || _product.NutritionFacts == null)
            {
                _textViewNoNutritionFacts.Visibility = ViewStates.Visible;
            }
            else
            {
                _nutritionItems = FindViewById<RecyclerView>(Resource.Id.nutrition_facts_recycler_view);
                _nutritionItems.Visibility = ViewStates.Visible;
                _nutritionItems.NestedScrollingEnabled = false;
                _layoutManager = new LinearLayoutManager(this, LinearLayoutManager.Vertical, false);

                _nutritionFactsAdapter = new NutritionFactsAdapter();

                _nutritionItems.SetLayoutManager(_layoutManager);
                _nutritionItems.SetAdapter(_nutritionFactsAdapter);

                _nutritionFactsAdapter.SetData(_product.NutritionFacts);
            }
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
                    item.SetEnabled(false);
                    ViewPresenter.OnFavoriteClick(_product);
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            GC.Collect(GC.MaxGeneration);
        }

        public void FavoriteChanged(Product product)
        {
            ChangeIcon(_menu.FindItem(Resource.Id.menu_add_favorite), product.IsProductFavorite);
            _product.IsProductFavorite = product.IsProductFavorite;
        }

        public void LastUnavailableFavoriteRemoved(Product product) => Finish();

        #region Private Methods

        /// <summary>
        /// Sets Icon of Favorite according to callback from Presenter
        /// </summary>
        /// <param name="item"></param>
        /// <param name="isFavorite"></param>
        private static void ChangeIcon(IMenuItem item, bool isFavorite)
        {
            item.SetIcon(isFavorite ? Resource.Drawable.ic_favorite_white_24dp : Resource.Drawable.ic_favorite_border_white);
            item.SetEnabled(true);
        }

        private void AddBottomDots(int currentPage)
        {
            _dots = new TextView[_images.Count];

            var colorsActive = ContextCompat.GetColor(this, Resource.Color.dot_detail_light_screen);
            var colorsInactive = ContextCompat.GetColor(this, Resource.Color.dot_detail_dark_screen);

            _dotsLayout.RemoveAllViews();
            for (var i = 0; i < _dots.Length; i++)
            {
                _dots[i] = new TextView(this) { Text = Html.FromHtml("&#8226;").ToString(), TextSize = 35 };
                _dots[i].SetTextColor(new Color(colorsInactive));
                _dotsLayout.AddView(_dots[i]);
            }

            if (_dots.Any())
                _dots[currentPage].SetTextColor(new Color(colorsActive));
        }

        #endregion
    }
}
