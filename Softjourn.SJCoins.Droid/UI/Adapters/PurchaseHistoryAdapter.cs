
using System.Collections.Generic;

using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Softjourn.SJCoins.Core.API.Model;

namespace Softjourn.SJCoins.Droid.UI.Adapters
{
    public class PurchaseHistoryAdapter : RecyclerView.Adapter
    {
        private List<History> _historyList = new List<History>();

        public void SetData(List<History> history)
        {
            _historyList = history;
            NotifyDataSetChanged();
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var holder = viewHolder as HistoryViewHolder;
            var historyItem = _historyList[position];

            holder._productName.Text = historyItem.Name;
            holder._productPrice.Text = (historyItem.Price + " coins");
            holder._purchaseDate.Text = historyItem.PrettyTime;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var v = LayoutInflater.From(parent.Context)
                .Inflate(Resource.Layout.recycler_purchase_history_item, parent, false);
            return new HistoryViewHolder(v);
        }

        public override int ItemCount => _historyList?.Count ?? 0;
    }

    internal class HistoryViewHolder : RecyclerView.ViewHolder
    {
        public View _parentView;
        public TextView _productPrice;
        public TextView _productName;
        public TextView _purchaseDate;

        public HistoryViewHolder(View v) : base(v)
        {
            _parentView = v.FindViewById(Resource.Id.layout_item_parent_view);
            _productPrice = v.FindViewById<TextView>(Resource.Id.layout_item_product_price);
            _productName = v.FindViewById<TextView>(Resource.Id.layout_item_product_name);
            _purchaseDate = v.FindViewById<TextView>(Resource.Id.layout_item_purchase_date);
        }
    }
}