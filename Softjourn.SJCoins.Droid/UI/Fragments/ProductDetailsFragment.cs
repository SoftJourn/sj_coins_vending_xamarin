
using System;
using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Core.Utils;
using Softjourn.SJCoins.Droid.UI.Activities;
using Square.Picasso;

namespace Softjourn.SJCoins.Droid.UI.Fragments
{
    public class ProductDetailsFragment : BottomSheetDialogFragment
    {
        private ImageView _favorites;
        private TextView _buyProduct;

        private bool _isRemovedFromFavorite = false;

        private readonly List<Product> _favoritesList = new List<Product>();

        private readonly Product _product;

        public ProductDetailsFragment(Product product, List<Categories> categories)
        {
            _product = product;
            foreach (var category in categories)
            {
                if (category.Name == "Favorites")
                {
                    _favoritesList.AddRange(category.Products);
                    break;
                }
            }
        }

        public override void OnViewCreated(View contentView, Bundle savedInstanceState)
        {
            base.OnViewCreated(contentView, savedInstanceState);
        }


        public override void SetupDialog(Dialog dialog, int style)
        {
            base.SetupDialog(dialog, style);

            var contentView = View.Inflate(Context, Resource.Layout.fragment_product_details, null);
            dialog.SetContentView(contentView);

            var productName = contentView.FindViewById<TextView>(Resource.Id.details_product_name);
            var productLongDescription = contentView.FindViewById<TextView>(Resource.Id.details_product_description);
            var productPrice = contentView.FindViewById<TextView>(Resource.Id.details_product_price);
            var productImage = contentView.FindViewById<ImageView>(Resource.Id.details_product_image);
            _favorites = contentView.FindViewById<ImageView>(Resource.Id.details_add_to_favorite);
            _buyProduct = contentView.FindViewById<TextView>(Resource.Id.details_buy_product);

            productName.Text = _product.Name;
            productLongDescription.Text = _product.Description;
            productPrice.Text = _product.IntPrice + " " + Activity.GetString(Resource.String.item_coins);
            Picasso.With(Activity).Load(Const.BaseUrl + Const.UrlVendingService + _product.ImageUrl).Into(productImage);
            LoadFavoriteIcon();

            _buyProduct.Click += (s, e) => HandleBuyButton();

            _favorites.Click += (s,e) => HandleOnFavoriteClick();
        }

        private void HandleBuyButton()
        {
            ((MainActivity)Activity).Purchase(_product);
        }

        private void LoadFavoriteIcon()
        {
            _favorites.Tag = false;
            if (_favoritesList != null && _favoritesList.Count > 0)
            {
                for (int i = 0; i < _favoritesList.Count; i++)
                {
                    if (_favoritesList[i].Id == _product.Id)
                    {
                        Picasso.With(Activity).Load(Resource.Drawable.ic_favorite_pink).Into(_favorites);
                        _favorites.Tag = true;
                        break;
                    }
                    else
                    {
                        Picasso.With(Activity).Load(Resource.Drawable.ic_favorite_border_white).Into(_favorites);
                        _favorites.Tag = false;
                    }
                }
            }
            else
            {
                Picasso.With(Activity).Load(Resource.Drawable.ic_favorite_border_white).Into(_favorites);
            }
        }

        private void HandleOnFavoriteClick()
        {
            if (!(bool)_favorites.Tag)
            {
                ((MainActivity)Activity).TrigFavorite(_product);
                Picasso.With(Activity).Load(Resource.Drawable.ic_favorite_pink).Into(_favorites);
                _favorites.Tag = true;
                _isRemovedFromFavorite = false;
            }
            else
            {
                ((MainActivity)Activity).TrigFavorite(_product);
                Picasso.With(Activity).Load(Resource.Drawable.ic_favorite_border_white).Into(_favorites);
                _favorites.Tag = false;
                _isRemovedFromFavorite = true;
            }
        }
    }
}
