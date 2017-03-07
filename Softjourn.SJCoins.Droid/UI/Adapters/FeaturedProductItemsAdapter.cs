using System;
using System.Collections.Generic;
using System.Linq;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Java.Lang;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Droid.utils;
using Softjourn.SJCoins.Droid.Utils;
using Square.Picasso;
using Exception = Java.Lang.Exception;
using Object = Java.Lang.Object;

namespace Softjourn.SJCoins.Droid.UI.Adapters
{
    public class FeaturedProductItemsAdapter : RecyclerView.Adapter, IFilterable
    {
        private string _recyclerViewType;
        private string _category;
        private readonly string _coins;
        private List<int> _animatedPosition;
        private Dictionary<int, AnimatorSet> _runningAnimations;

        public event EventHandler<Product> AddToFavorites;
        public event EventHandler<Product> RemoveFromFavorites;
        public event EventHandler LastFavoriteRemoved;

        public EventHandler<Product> ProductSelected;
        public EventHandler<Product> ProductDetailsSelected;
        public List<Product> ListProducts = new List<Product>();
        public List<Product> Original = new List<Product>();
        private Context _context;

        public FeaturedProductItemsAdapter(string featureCategory, string recyclerViewType, Context context)
        {

            _context = context;
            Filter = new SearchFilter(this);

            if (featureCategory != null)
            {
                _category = featureCategory;
            }
            else
            {
                _category = "";
            }

            if (recyclerViewType != null)
            {
                _recyclerViewType = recyclerViewType;
            }
            else
            {
                _recyclerViewType = Const.DefaultRecyclerView;
            }
            _coins = " " + _context.GetString(Resource.String.item_coins);
        }

        #region Standart Adapters Methods
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View v;
            //Attach needed item according to recyclerViewType
            switch (_recyclerViewType)
            {
                case "DEFAULT":
                    v = LayoutInflater.From(parent.Context)
                        .Inflate(Resource.Layout.recycler_machine_view_item, parent, false);
                    break;
                case "SEE_ALL_SNACKS_DRINKS":
                    v = LayoutInflater.From(parent.Context)
                        .Inflate(Resource.Layout.recycler_see_all_item, parent, false);
                    break;
                default:
                    v = LayoutInflater.From(parent.Context)
                        .Inflate(Resource.Layout.recycler_machine_view_item, parent, false);
                    break;
            }
            return new FeatureViewHolder(v);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var holder = viewHolder as FeatureViewHolder;
            var product = ListProducts[holder.AdapterPosition];

            var isCurrentProductInMachine = true;

            //Setting Name and Price of product
            holder.ProductName.Text = ListProducts[holder.AdapterPosition].Name;
            holder.ProductPrice.Text = Java.Lang.String.ValueOf(product.IntPrice) + _coins;

            //Setting Description of Product
            if (holder.ProductDescription != null)
            {
                holder.ProductDescription.Text = product.Description;
            }

            /**
             * Registartion OnClick and OnLongClick events
             * for Horizontal RecyclerView
             */
            if (holder.ParentView != null)
            {
                holder.Click -= OnClickClicked;
                holder.Click += OnClickClicked;
                holder.LongClick -= OnLongClick;
                holder.LongClick += OnLongClick;

            }
            /**
             * Registartion OnClick and OnLongClick events
             * for Vertical RecyclerView
             */
            if (holder.ParentViewSeeAll != null)
            {
                holder.Click -= OnClickClicked;
                holder.Click += OnClickClicked;
                holder.LongClick -= OnLongClick;
                holder.LongClick += OnLongClick;
            }

            /**
             * If product is Favorite setting appropriate icon
             */
            if (holder.AddFavorite != null)
            {
                {
                    if (product.IsProductFavorite)
                    {
                        Picasso.With(_context).Load(Resource.Drawable.ic_favorite_pink).Into(holder.AddFavorite);
                        if (product.IsHeartAnimationRunning)
                        {
                            FinishAnimation(holder);
                            _animatedPosition.Remove(holder.AdapterPosition);
                        }
                    }
                    else
                    {
                        Picasso.With(_context).Load(Resource.Drawable.ic_favorite_border).Into(holder.AddFavorite);
                        if (product.IsHeartAnimationRunning)
                        {
                            FinishAnimation(holder);
                            _animatedPosition.Remove(holder.AdapterPosition);
                        }
                    }
                }

                holder.AddFavoriteClick -= AddFavoriteClick;
                holder.AddFavoriteClick += AddFavoriteClick;
            }
            /**
             * Changing Alpha of image depends on is product present in chosen machine or not
             * TODO: Not implemented yet
             */
            if (TextUtils.IsEmpty(product.ImageUrl))
            {
                Picasso.With(_context).Load(Resource.Drawable.logo).Into(holder.ProductImage);
                holder.ProductImage.Alpha = 1.0f;
            }
            else
            {
                Picasso.With(_context).Load(Core.Utils.Const.BaseUrl + Core.Utils.Const.UrlVendingService + ListProducts[holder.AdapterPosition].ImageUrl).Into(holder.ProductImage);
                if (_category == Const.Favorites)
                holder.ProductImage.Alpha = !product.IsProductInCurrentMachine ? 0.3f : 1.0f;
            }
        }

        public override int ItemCount => ListProducts?.Count ?? 0;

        #endregion

        #region Public Methods

        /**
         * Sets new List of Data and resetting recyclerView
         */
        public void SetData(List<Product> data)
        {
            ListProducts = new List<Product>(data);
            Original = new List<Product>(data);
            NotifyDataSetChanged();
        }

        public void ChangeFavoriteIcon()
        {
            NotifyDataSetChanged();
        }

        #endregion

        #region Private Event Methods
        /**
         * Long Click on Item to show Preview Fragment
         */
        private void OnLongClick(object sender, EventArgs e)
        {
            var holder = sender as FeatureViewHolder;
            if (holder == null)
            {
                throw new Exception("Holder is null");
            }
            var selectedIndex = holder.AdapterPosition;
            var handler = ProductDetailsSelected;
            if (handler == null) return;
            var selectedProduct = ListProducts[selectedIndex];
            handler(this, selectedProduct);
        }

        /**
         * Long Click on Item to show Details Screen
         */
        private void OnClickClicked(object sender, EventArgs eventArgs)
        {
            var holder = sender as FeatureViewHolder;
            if (holder == null)
            {
                throw new Exception("Holder is null");
            }
            var selectedIndex = holder.AdapterPosition;
            var handler = ProductSelected;
            if (handler == null) return;
            var selectedProduct = ListProducts[selectedIndex];
            handler(this, selectedProduct);
        }

        /**
         * Calls by clicking on favorite icon
         */
        private void AddFavoriteClick(object sender, EventArgs e)
        {
            var holder = sender as FeatureViewHolder;
            if (holder == null)
            {
                throw new Exception("Holder is null");
            }
            var product = ListProducts[holder.AdapterPosition];
            if (!product.IsProductFavorite)
            {
                AnimateHeartButton(holder);
                holder.AddFavorite.Tag = true;
                AddToFavorites?.Invoke(this, product);
            }
            else
            {
                //If category is favorite we need to remove item from list
                //and not just change the icon of favorite
                if (_category == Const.Favorites)
                {
                    RemoveFromFavorites?.Invoke(this, product);
                    ListProducts.RemoveAt(holder.AdapterPosition);
                    NotifyItemRemoved(holder.AdapterPosition);
                    NotifyItemRangeChanged(holder.AdapterPosition, ItemCount + 1);
                    if (ItemCount < 1)
                    {
                        LastFavoriteRemoved?.Invoke(this, EventArgs.Empty);
                    }
                }
                else
                {
                    AnimateHeartButton(holder);
                    holder.AddFavorite.Tag = false;
                    RemoveFromFavorites?.Invoke(this, product);
                }
            }
        }

        private void AnimateHeartButton(FeatureViewHolder holder)
        {
            var animatorSet = new AnimatorSet();

            var rotationAnim = ObjectAnimator.OfFloat(holder.AddFavorite, "rotation", 0f, 360f);
            rotationAnim.SetDuration(600);
            rotationAnim.SetInterpolator(new AccelerateInterpolator());
            rotationAnim.RepeatCount = Animation.Infinite;

            animatorSet.Play(rotationAnim);
            animatorSet.Start();

            if (_animatedPosition == null)
            {
                _animatedPosition = new List<int>();
            }
            _animatedPosition.Add(holder.AdapterPosition);

            if (_runningAnimations == null)
            {
                _runningAnimations = new Dictionary<int, AnimatorSet>();
            }
            _runningAnimations.Add(holder.AdapterPosition, animatorSet);
            ListProducts[holder.AdapterPosition].IsHeartAnimationRunning = true;
        }

        private void FinishAnimation(FeatureViewHolder holder)
        {
            if (_runningAnimations.ContainsKey(holder.AdapterPosition))
            {
                _runningAnimations[holder.AdapterPosition].End();
                _runningAnimations.Remove(holder.AdapterPosition);
                ListProducts[holder.AdapterPosition].IsHeartAnimationRunning = false;
            }

            var animatorSet = new AnimatorSet();

            var bounceAnimX = ObjectAnimator.OfFloat(holder.AddFavorite, "scaleX", 0.2f, 1f);
            bounceAnimX.SetDuration(400);
            bounceAnimX.SetInterpolator(new OvershootInterpolator());

            var bounceAnimY = ObjectAnimator.OfFloat(holder.AddFavorite, "scaleY", 0.2f, 1f);
            bounceAnimY.SetDuration(400);
            bounceAnimY.SetInterpolator(new OvershootInterpolator());
            bounceAnimY.AnimationStart += (sender, e) =>
            {
                if (ListProducts[holder.AdapterPosition].IsProductFavorite)
                {
                    holder.AddFavorite.SetImageResource(
                        Resource.Drawable.ic_favorite_pink);
                }
                else
                {
                    holder.AddFavorite.SetImageResource(
                        Resource.Drawable.ic_favorite_border);
                }
            };

            animatorSet.Play(bounceAnimX).With(bounceAnimY);
            animatorSet.Start();
        }

        #endregion

        #region Filter for SearchView

        public Filter Filter { get; private set; }

        private class SearchFilter : Filter
        {
            private readonly FeaturedProductItemsAdapter _adapter;

            public SearchFilter(FeaturedProductItemsAdapter adapter)
            {
                _adapter = adapter;
            }

            protected override FilterResults PerformFiltering(ICharSequence constraint)
            {
                var oReturn = new FilterResults();
                var results = new List<Product>();
                if (_adapter.Original == null || _adapter.Original.Count <= 0)
                    _adapter.Original = _adapter.ListProducts;
                if (constraint != null)
                {
                    if (_adapter.Original != null && _adapter.Original.Count > 0)
                    {
                        foreach (var g in _adapter.Original)
                        {
                            if (g.Name.ToLower().Contains(constraint.ToString()))
                            {
                                results.Add(g);
                            }
                        }
                        oReturn.Values = FromArray(results.Select(r => r.ToJavaObject()).ToArray());
                        oReturn.Count = results.Count;
                    }
                    constraint.Dispose();
                }
                return oReturn;
            }

            protected override void PublishResults(ICharSequence constraint, FilterResults results)
            {
                if (_adapter.ListProducts == null || _adapter.ListProducts.Count <= 0) return;
                using (var values = results.Values)
                    _adapter.ListProducts = values.ToArray<Object>()
                        .Select(r => r.ToNetObject<Product>()).ToList();

                _adapter.NotifyDataSetChanged();
            }
        }
        #endregion
    }
}