using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace Softjourn.SJCoins.Droid.UI.Adapters
{
    public class FeatureViewHolder : RecyclerView.ViewHolder, View.IOnClickListener, View.IOnLongClickListener
    {

        public event EventHandler Click;
        public event EventHandler LongClick;
            
        public View ParentView { get; set; }
        public View ParentViewSeeAll { get; set; }
        public TextView ProductPrice { get; set; }
        public TextView ProductName { get; set; }
        public TextView BuyProduct { get; set; }
        public TextView ProductDescription { get; set; }
        public ImageView ProductImage { get; set; }
        public ImageView AddFavorite { get; set; }

        public FeatureViewHolder(View v) : base(v)
            {
            ParentViewSeeAll = v.FindViewById<View>(Resource.Id.layout_item_parent_view);
            ParentView = v.FindViewById<View>(Resource.Id.layout_item_product_parent_view);
            ProductImage = v.FindViewById<ImageView>(Resource.Id.layout_item_product_img);
            ProductPrice = v.FindViewById<TextView>(Resource.Id.layout_item_product_price);
            ProductName = v.FindViewById<TextView>(Resource.Id.layout_item_product_name);
            BuyProduct = v.FindViewById<TextView>(Resource.Id.layout_item_product_buy);
            ProductDescription = v.FindViewById<TextView>(Resource.Id.layout_item_product_description);
            AddFavorite = v.FindViewById<ImageView>(Resource.Id.imageViewFavorite);

            v.SetOnClickListener(this);
            v.SetOnLongClickListener(this);
        }

        public void OnClick(View v)
        {
            Click?.Invoke(this, EventArgs.Empty);
        }

        public bool OnLongClick(View v)
        {
            if (v == null) return false;
            LongClick?.Invoke(this, EventArgs.Empty);
            return true;
        }
    }
}