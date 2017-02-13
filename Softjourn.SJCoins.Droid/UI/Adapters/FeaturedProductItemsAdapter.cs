using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Droid.utils;
using Softjourn.SJCoins.Droid.Utils;
using Square.Picasso;
using Exception = System.Exception;
using Object = Java.Lang.Object;

namespace Softjourn.SJCoins.Droid.UI.Adapters
{
    public class FeaturedProductItemsAdapter : RecyclerView.Adapter, IFilterable
    {
        private string _recyclerViewType;
        private string _category;
        private string _coins;

        public event EventHandler<Product> AddToFavorites;
        public event EventHandler<Product> RemoveFromFavorites;
        public event EventHandler<Product> BuyClicked;
        public event EventHandler LastFavoriteRemoved;

        public EventHandler<Product> ProductSelected;
        public EventHandler<Product> ProductDetailsSelected;
        public List<Product> ListProducts = new List<Product>();
        public List<Product> _original = new List<Product>();
        private List<Product> _favoritesList; // = mDataManager.loadFavorites();
        private Context _context;

        private const int TagKey = 100001;

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

        public void NotifyDataChanges()
        {
            if (_category != "Favorites")
            {
                NotifyDataSetChanged();
            }
            else
            {
                
            }
        }

        public void SetData(List<Product> data)
        {
            ListProducts = new List<Product>(data);
            _original = new List<Product>(data);
            NotifyDataSetChanged();
        }

        public void RemoveItem(int id)
        {
            for (int i = 0; i < ListProducts.Count; i++)
            {
                if (ListProducts[i].Id == id)
                {
                    ListProducts.Remove(ListProducts[i]);
                    NotifyItemRemoved(i);
                    NotifyItemRangeChanged(0, ItemCount + 1);
                }
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View v;
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

            bool isCurrentProductInMachine = true; //mDataManager.isSingleProductPresent(product.Id);

            holder.ProductName.Text = ListProducts[holder.AdapterPosition].Name;
            holder.ProductPrice.Text = Java.Lang.String.ValueOf(product.IntPrice) + _coins;

            if (holder.ProductDescription != null)
            {
                holder.ProductDescription.Text = product.Description;
            }

            if (holder.ParentView != null)
            {
                holder.Click -= OnClickClicked;
                holder.Click += OnClickClicked;
                holder.LongClick -= OnLongClick;
                holder.LongClick += OnLongClick;

            }
            if (holder.ParentViewSeeAll != null)
            {
                holder.Click -= OnClickClicked;
                holder.Click += OnClickClicked;
                holder.LongClick -= OnLongClick;
                holder.LongClick += OnLongClick;
            }


            if (holder.BuyProduct != null)
            {
                holder.BuyProduct.Click += (s, e) =>
                {
                    BuyClicked(this, product);
                };
            }
            /**
             * Here We compare ID of product from general products list
             * and ID of favorites list
             * When IDs are the same we change image of Favorites accordingly.
             * Also we differentiate callbacks on Click on Image depends on
             * if this product is already in Favorites to remove or add product
             * to favorites list
             */
            if (holder.AddFavorite != null)
            {
                if (product.IsProductFavorite)
                {
                    Picasso.With(_context).Load(Resource.Drawable.ic_favorite_pink).Into(holder.AddFavorite);
                }
                else
                {
                    Picasso.With(_context).Load(Resource.Drawable.ic_favorite_border).Into(holder.AddFavorite);
                }

                holder.AddFavoriteClick -= AddFavoriteClick;
                holder.AddFavoriteClick += AddFavoriteClick;
                //holder.AddFavorite.Click += (s, e) =>
                //{
                //    if (!product.IsProductFavorite)
                //    {
                //        AddToFavorites(this, product);
                //    }
                //    else
                //    {
                //        if (_category == Const.Favorites)
                //        {
                //            if ((holder.AdapterPosition) >= 0)
                //            {
                //                RemoveFromFavorites(this, product);
                //                ListProducts.Remove(ListProducts[holder.AdapterPosition]);
                //                NotifyItemRemoved(holder.AdapterPosition);
                //                NotifyItemRangeChanged(0, ItemCount);
                //                if (ItemCount < 1)
                //                {
                //                    LastFavoriteRemoved(this, EventArgs.Empty);
                //                }
                //            }
                //        }
                //        else
                //        {
                //           RemoveFromFavorites(this, product);
                //        }
                //    }
                //};
            }
            /**
             * Changing Alpha of image depends on is product present in chosen machine or not
             */
            if (TextUtils.IsEmpty(product.ImageUrl))
            {
                Picasso.With(_context).Load(Resource.Drawable.logo).Into(holder.ProductImage);
                holder.ProductImage.Alpha = 1.0f;
            }
            else
            {
                Picasso.With(_context).Load(Core.Utils.Const.BaseUrl + Core.Utils.Const.UrlVendingService + ListProducts[holder.AdapterPosition].ImageUrl).Into(holder.ProductImage);
                holder.ProductImage.Alpha = !isCurrentProductInMachine ? 0.3f : 1.0f;
            }
        }

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
                AddToFavorites?.Invoke(this, product);
            }
            else
            {
                if (_category == Const.Favorites)
                {
                    if ((holder.AdapterPosition) >= 0)
                    {
                        RemoveFromFavorites?.Invoke(this, product);
                        ListProducts.Remove(ListProducts[holder.AdapterPosition]);
                        NotifyItemRemoved(holder.AdapterPosition);
                        NotifyItemRangeChanged(0, ItemCount);
                        if (ItemCount < 1)
                        {
                            LastFavoriteRemoved?.Invoke(this, EventArgs.Empty);
                        }
                    }
                }
                else
                {
                    RemoveFromFavorites?.Invoke(this, product);
                }
            }
        }

        public override int ItemCount => ListProducts?.Count ?? 0;

        public void OnLongClick(object sender, EventArgs e)
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

        public void OnClickClicked(object sender, EventArgs eventArgs)
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
                if (_adapter._original == null || _adapter._original.Count <= 0)
                    _adapter._original = _adapter.ListProducts;
                if (constraint != null)
                {
                    if (_adapter._original != null && _adapter._original.Count > 0)
                    {
                        foreach (var g in _adapter._original)
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


    }
}