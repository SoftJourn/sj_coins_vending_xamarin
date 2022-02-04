
using System;
using System.Collections.Generic;
using System.Linq;
using Android.Accounts;
using Android.Content;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using Softjourn.SJCoins.Core.API.Model;

namespace Softjourn.SJCoins.Droid.UI.Adapters
{
    public class OptionsListAdapter : RecyclerView.Adapter
    {
        private List<AccountOption> _optionsList;
        private readonly Context _context;

        private class ViewHolder : RecyclerView.ViewHolder
        {
            public TextView Name { get; }
            public ImageView Icon { get; }
            public View Root { get; }
            private Action<AccountOption> _clickCallback;
            private AccountOption _accountOption;

            public ViewHolder(View itemView) : base(itemView)
            {
                Name = itemView.FindViewById<TextView>(Resource.Id.nameTextView);
                Icon = itemView.FindViewById<ImageView>(Resource.Id.imageViewOptionIcon);
                Root = itemView;
                Root.Click += RootOnClick;
            }

            public void Bind(AccountOption accountOption, Action<AccountOption> clickCallback)
            {
                _accountOption = accountOption;
                _clickCallback = clickCallback;
                Name.Text = accountOption?.OptionName;
                var iconPath = Root.Context?.Resources?.GetIdentifier(accountOption?.OptionIconName, "drawable",
                    Root.Context.PackageName);
                if (accountOption == null || !iconPath.HasValue)
                    return;
                Glide.With(Root.Context).Load(iconPath.Value).Into(Icon);
            }

            private void RootOnClick(object sender, EventArgs e) => _clickCallback?.Invoke(_accountOption);

        }

        public OptionsListAdapter(Context context, List<AccountOption> objects)
        {
            _optionsList = objects;
            _context = context;
        }


        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (!(holder is ViewHolder viewHolder)) return;
            viewHolder.Bind(_optionsList.ElementAtOrDefault(position), option => ItemClicked?.Invoke(this, option));
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            return new ViewHolder(LayoutInflater.From(_context)?
                .Inflate(Resource.Layout.recycler_options_item, parent, false));
        }

        public event EventHandler<AccountOption> ItemClicked;

        public override int ItemCount => _optionsList?.Count ?? 0;
    }
}