
using Android.Animation;
using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Droid.UI.Activities;
using Square.Picasso;

namespace Softjourn.SJCoins.Droid.UI.Fragments
{
    public class ProductDetailsFragment : BottomSheetDialogFragment
    {
        private const string ArgProduct = "PRODUCT";

        private ImageView _favorites;
        private TextView _buyProduct;
        private Product _product;

        private bool _isAnimationRunning;
        private AnimatorSet _runningAnimation;

        public static ProductDetailsFragment GetInstance(Product product)
        {
            var bundle = new Bundle();
            string serializedObj = Newtonsoft.Json.JsonConvert.SerializeObject(product);
            bundle.PutString(ArgProduct, serializedObj);
            var fragment = new ProductDetailsFragment();
            fragment.Arguments = bundle;
            return fragment;
        }

        #region BottomSheetDialog Fragment Standart methods

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _product = Newtonsoft.Json.JsonConvert.DeserializeObject<Product>(Arguments.GetString(ArgProduct));
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

        #region Public Methods

        public void ChangeFavoriteIcon(bool isFavorite)
        {
            FinishAnimation();
            _product.IsProductFavorite = isFavorite;
            _favorites.Tag = isFavorite;
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
                    AnimateHeartButton();
                    ((MainActivity)Activity).TrigFavorite(_product);
                }
                else ((ShowAllActivity)Activity).TrigFavorite(_product);
                //AnimateHeartButton();
                //_favorites.Tag = true;
            }
            else
            {
                if (Activity.LocalClassName.Contains("MainActivity"))
                {
                    AnimateHeartButton();
                    ((MainActivity)Activity).TrigFavorite(_product);
                }
                else ((ShowAllActivity)Activity).TrigFavorite(_product);
                //AnimateHeartButton();
                //_favorites.Tag = false;
            }
        }

        private void AnimateHeartButton()
        {
            var animatorSet = new AnimatorSet();
            _favorites.Enabled = false;

            var rotationAnim = ObjectAnimator.OfFloat(_favorites, "rotation", 0f, 360f);
            rotationAnim.SetDuration(600);
            rotationAnim.SetInterpolator(new AccelerateInterpolator());
            rotationAnim.RepeatCount = Animation.Infinite;

            animatorSet.Play(rotationAnim);
            animatorSet.Start();

            _isAnimationRunning = true;
            _runningAnimation = animatorSet;
        }

        private void FinishAnimation()
        {
            if (_isAnimationRunning)
            {
                _runningAnimation.End();
                _isAnimationRunning = false;
                _runningAnimation = null;
            }

            var animatorSet = new AnimatorSet();

            var bounceAnimX = ObjectAnimator.OfFloat(_favorites, "scaleX", 0.2f, 1f);
            bounceAnimX.SetDuration(400);
            bounceAnimX.SetInterpolator(new OvershootInterpolator());

            var bounceAnimY = ObjectAnimator.OfFloat(_favorites, "scaleY", 0.2f, 1f);
            bounceAnimY.SetDuration(400);
            bounceAnimY.SetInterpolator(new OvershootInterpolator());
            bounceAnimY.AnimationStart += (sender, e) =>
            {
                if (!(bool)_favorites.Tag)
                {
                    _favorites.SetImageResource(
                        Resource.Drawable.ic_favorite_pink);
                //    _product.IsProductFavorite = true;
                }
                else
                {
                    _favorites.SetImageResource(
                        Resource.Drawable.ic_favorite_border_white);
                //    _product.IsProductFavorite = false;
                }
            };

            animatorSet.Play(bounceAnimX).With(bounceAnimY);
            animatorSet.Start();
            _favorites.Enabled = true;
        }
        #endregion
    }
}
