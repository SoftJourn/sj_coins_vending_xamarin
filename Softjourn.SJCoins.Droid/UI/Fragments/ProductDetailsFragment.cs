
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
        private readonly Product _product;

        public ProductDetailsFragment(Product product)
        {
            _product = product;
        }

        #region BottomSheetDialog Fragment Standart methods
        public override void OnViewCreated(View contentView, Bundle savedInstanceState)
        {
            base.OnViewCreated(contentView, savedInstanceState);
        }


        public override void SetupDialog(Dialog dialog, int style)
        {
            base.SetupDialog(dialog, style);

            var contentView = View.Inflate(Context, Resource.Layout.fragment_product_details, null);
            dialog.SetContentView(contentView);

            #region Finding Views
            var productName = contentView.FindViewById<TextView>(Resource.Id.details_product_name);
            var productLongDescription = contentView.FindViewById<TextView>(Resource.Id.details_product_description);
            var productPrice = contentView.FindViewById<TextView>(Resource.Id.details_product_price);
            var productImage = contentView.FindViewById<ImageView>(Resource.Id.details_product_image);
            _favorites = contentView.FindViewById<ImageView>(Resource.Id.details_add_to_favorite);
            _buyProduct = contentView.FindViewById<TextView>(Resource.Id.details_buy_product);
            #endregion

            productName.Text = _product.Name;
            productLongDescription.Text = _product.Description;
            productPrice.Text = _product.IntPrice + " " + Activity.GetString(Resource.String.item_coins);
            Picasso.With(Activity).Load(_product.ImageFullUrl).Into(productImage);
            LoadFavoriteIcon();

            _buyProduct.Click += (s, e) => HandleBuyButton();

            _favorites.Click += (s, e) => HandleOnFavoriteClick();
        }
        #endregion

        #region Private Methods
        /**
         * Calls by clicking on Buy button
         * Calls Purchase methode on Activity to which is attached to
         * and transfer Product object to it.
         */
        private void HandleBuyButton()
        {
            Dismiss();
            if (Activity.LocalClassName.Contains("MainActivity"))
            {
                ((MainActivity)Activity).Purchase(_product);
                return;
            }
                ((ShowAllActivity)Activity).Purchase(_product);
        }

        /**
         * Calls when creating fragment
         * Sets appropriate iamge to favorite imageView
         * and changes its tag
         */
        private void LoadFavoriteIcon()
        {
            _favorites.Tag = false;
            if (_product.IsProductFavorite)
            {
                Picasso.With(Activity).Load(Resource.Drawable.ic_favorite_pink).Into(_favorites);
                _favorites.Tag = true;

            }
            else
            {
                Picasso.With(Activity).Load(Resource.Drawable.ic_favorite_border_white).Into(_favorites);
                _favorites.Tag = false;
            }
        }

        /**
         * Calls when clicking favorite image
         * Sets appropriate iamge to favorite imageView
         * and changes its tag
         * Calls TrigFavorite methode on Activity to which is attached to
         * and transfer Product object to it.
         */
        private void HandleOnFavoriteClick()
        {
            if (!(bool)_favorites.Tag)
            {
                if (Activity.LocalClassName.Contains("MainActivity"))
                {
                    ((MainActivity)Activity).TrigFavorite(_product);
                }
                else ((ShowAllActivity)Activity).TrigFavorite(_product);
                Picasso.With(Activity).Load(Resource.Drawable.ic_favorite_pink).Into(_favorites);
                _favorites.Tag = true;
            }
            else
            {
                if (Activity.LocalClassName.Contains("MainActivity"))
                {
                    ((MainActivity)Activity).TrigFavorite(_product);
                }
                else ((ShowAllActivity)Activity).TrigFavorite(_product);
                Picasso.With(Activity).Load(Resource.Drawable.ic_favorite_border_white).Into(_favorites);
                _favorites.Tag = false;
            }
        }
        #endregion
    }
}
