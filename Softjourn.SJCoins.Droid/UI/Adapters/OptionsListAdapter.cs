using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using Softjourn.SJCoins.Core.Models;
using Square.Picasso;

namespace Softjourn.SJCoins.Droid.UI.Adapters
{
    public class OptionsListAdapter : ArrayAdapter<AccountOption>
    {
        private List<AccountOption> _optionsList;
        private readonly Context _context;

        private class ViewHolder
        {
            public TextView Name;
            public ImageView Icon;
        }

        public OptionsListAdapter(Context context, List<AccountOption> objects) : base(context, Resource.Layout.recycler_options_item, objects)
        {
            _optionsList = objects;
            _context = context;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var accountOption = GetItem(position);
            var iconPath = _context.Resources.GetIdentifier(accountOption.OptionIconName, "drawable",
                _context.PackageName);


            var viewHolder = new ViewHolder();
            var inflater = LayoutInflater.From(_context);

            convertView = inflater.Inflate(Resource.Layout.recycler_options_item, parent, false);

            viewHolder.Name = convertView.FindViewById<TextView>(Resource.Id.nameTextView);
            viewHolder.Icon = convertView.FindViewById<ImageView>(Resource.Id.imageViewOptionIcon);

            viewHolder.Name.Text = accountOption.OptionName;
            Picasso.With(_context).Load(iconPath).Into(viewHolder.Icon);

            return convertView;
        }
    }
}