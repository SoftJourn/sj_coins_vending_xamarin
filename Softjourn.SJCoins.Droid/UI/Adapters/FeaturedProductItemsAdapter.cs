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
using Square.Picasso;

namespace Softjourn.SJCoins.Droid.UI.Adapters
{
    public class FeaturedProductItemsAdapter : RecyclerView.Adapter, IFilterable
    {
        private string _recyclerViewType;
        private string _category;
        private string _coins;

        public List<Product> _listProducts = new List<Product>();
        //private DataManager mDataManager = new DataManager();
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
            //_favoritesList = mDataManager.loadFavorites();
            NotifyDataSetChanged();
        }

        public void SetData(List<Product> data)
        {
            _listProducts = new List<Product>(data);
            _original = new List<Product>(data);
            NotifyDataSetChanged();
        }

        public void RemoveItem(int id)
        {
            for (int i = 0; i < _listProducts.Count; i++)
            {
                if (_listProducts[i].Id == id)
                {
                    _listProducts.Remove(_listProducts[i]);
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
            var product = _listProducts[holder.AdapterPosition];

            bool isCurrentProductInMachine = true; //mDataManager.isSingleProductPresent(product.Id);

            holder.ProductName.Text = _listProducts[holder.AdapterPosition].Name;
            holder.ProductPrice.Text = Java.Lang.String.ValueOf(product.Price) + _coins;

            if (holder.ProductDescription != null)
            {
                holder.ProductDescription.Text = product.Description;
            }

            if (holder.ParentView != null)
            {
                holder.ParentView.Click += (s, e) =>
                {
                    //EventBus.getDefault().post(new OnProductItemClickEvent(mListProducts.get(holder.getAdapterPosition())));
                };
            }
            if (holder.ParentViewSeeAll != null)
            {
                holder.ParentViewSeeAll.Click += (s, e) =>

                {
                       //EventBus.getDefault().post(new OnProductDetailsClick(mListProducts.get(holder.getAdapterPosition())));
                   };
            }

            /**
             * Changing color of Buy TextView depends on is product in chosen machine or not
             * Also if product is not available in current machine there is no listener for click on TextView.
             */
            if (holder.BuyProduct != null)
            {
                if (isCurrentProductInMachine)
                {
                    holder.BuyProduct.SetTextColor(new Color(ContextCompat.GetColor(_context, Resource.Color.colorBlue)));
                }
                else
                {
                    holder.BuyProduct.SetTextColor(new Color(ContextCompat.GetColor(_context, Resource.Color.colorScreenBackground)));
                }
                holder.BuyProduct.Click += (s, e) =>
                {
                           //EventBus.getDefault().post(new OnProductBuyClickEvent(mListProducts.get(holder.getAdapterPosition())));
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
                holder.AddFavorite.SetTag(TagKey, false);
                if (_favoritesList != null && _favoritesList.Count > 0)
                {
                    for (int i = 0; i < _favoritesList.Count; i++)
                    {
                        if (_favoritesList[i].Id == product.Id)
                        {
                            Picasso.With(_context).Load(Resource.Drawable.ic_favorite_pink).Into(holder.AddFavorite);
                            holder.AddFavorite.SetTag(TagKey, true);
                            break;
                        }
                        Picasso.With(_context).Load(Resource.Drawable.ic_favorite_border).Into(holder.AddFavorite);
                        holder.AddFavorite.SetTag(TagKey, false);
                    }
                }
                else
                {
                    Picasso.With(_context).Load(Resource.Drawable.ic_favorite_border).Into(holder.AddFavorite);
                }
                holder.AddFavorite.Click += (s, e) =>
                {
                    if ((bool)holder.AddFavorite.GetTag(TagKey) != true)
                    {
                        //EventBus.getDefault().post(new OnAddFavoriteEvent(mListProducts.get(holder.getAdapterPosition())));
                    }
                    else
                    {
                        //EventBus.getDefault().post(new OnRemoveFavoriteEvent(mListProducts.get(holder.getAdapterPosition())));
                        if (NetworkUtils.IsNetworkEnabled())
                        {
                            if (_category == Const.Favorites)
                            {
                                _listProducts.Remove(_listProducts[holder.AdapterPosition]);
                                NotifyItemRemoved(holder.AdapterPosition);
                                NotifyItemRangeChanged(0, ItemCount + 1);
                                if (ItemCount < 1)
                                {
                                    //EventBus.getDefault().post(new OnRemovedLastFavoriteEvent(mListProducts));
                                }
                            }
                        }
                    }
                };
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
                Picasso.With(_context).Load(Const.UrlVendingService + _listProducts[holder.AdapterPosition].ImageUrl).Into(holder.ProductImage);
                holder.ProductImage.Alpha = !isCurrentProductInMachine ? 0.3f : 1.0f;
            }
        }

        public override int ItemCount => _listProducts?.Count ?? 0;

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
                    _adapter._original = _adapter._listProducts;
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
                if (_adapter._listProducts == null || _adapter._listProducts.Count<=0) return;
                using (var values = results.Values)
                    _adapter._listProducts = values.ToArray<Object>()
                        .Select(r => r.ToNetObject<Product>()).ToList();

                _adapter.NotifyDataSetChanged();
            }
        }


    }
}