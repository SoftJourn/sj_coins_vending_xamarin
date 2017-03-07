
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Softjourn.SJCoins.Core.API.Model.TransactionReports;

namespace Softjourn.SJCoins.Droid.UI.Adapters
{
    class ReportsAdapter : RecyclerView.Adapter
    {
        private List<Transaction> _transactionsList = new List<Transaction>();

        public void SetData(List<Transaction> transactions)
        {
            var transactionList = new List<Transaction>(transactions);

            _transactionsList.Clear();
            _transactionsList = transactionList;
            NotifyDataSetChanged();
        }

        public void AddData(List<Transaction> transaction)
        {
            var transactionList = new List<Transaction>(transaction);

            _transactionsList.AddRange(transactionList);
            NotifyDataSetChanged();
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var holder = viewHolder as TransactionViewHolder;
            var transactionItem = _transactionsList[position];

            holder._sender.Text = transactionItem.Account;
            holder._receiver.Text = transactionItem.Destination;
            holder._amount.Text = transactionItem.Amount + " coins";
            holder._date.Text = transactionItem.PrettyTime;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var v = LayoutInflater.From(parent.Context)
                .Inflate(Resource.Layout.recycler_report_transactions_item, parent, false);
            return new TransactionViewHolder(v);
        }

        public override int ItemCount => _transactionsList?.Count ?? 0;
    }

    internal class TransactionViewHolder : RecyclerView.ViewHolder
    {
        public View _parentView;
        public TextView _sender;
        public TextView _receiver;
        public TextView _amount;
        public TextView _date;

        public TransactionViewHolder(View v) : base(v)
        {
            _parentView = v.FindViewById(Resource.Id.layout_item_parent_view);
            _sender = v.FindViewById<TextView>(Resource.Id.layout_item_sender);
            _receiver = v.FindViewById<TextView>(Resource.Id.layout_item_receiver);
            _amount = v.FindViewById<TextView>(Resource.Id.layout_item_amount);
            _date = v.FindViewById<TextView>(Resource.Id.layout_item_date);
        }
    }
}