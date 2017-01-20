using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Softjourn.SJCoins.Droid.utils;
using String = System.String;

namespace Softjourn.SJCoins.Droid.ui.adapters
{
    public class SelectMachineListAdapter : BaseAdapter
    {
        private List<String> mItems;

        private Context mContext;

        public SelectMachineListAdapter(Context context, int resource, List<String> list)
        {
            mItems = list;
            mContext = context;
        }


    public override int Count => mItems.Count;

        public override Object GetItem(int position)
        {
            return mItems[position];
        }

    public override long GetItemId(int position)
        {
            return position;
        }


    public override View GetView(int position, View convertView, ViewGroup parent)
        {

            View view = convertView;
            if (view == null)
            {
                LayoutInflater li;
                li = LayoutInflater.From(mContext);
                view = li.Inflate(Resource.Layout.select_machine_text_view, null);
            }

            String name = GetItem(position).ToString();

            TextView machineName = view.FindViewById<TextView>(Resource.Id.text1);
            machineName.Text = name;

            if (name == Preferences.RetrieveStringObject(Const.SelectedMachineName))
            {
                machineName.SetTextColor(new Color(ContextCompat.GetColor(mContext, Resource.Color.colorBlue)));
            }
            else
            {
                machineName.SetTextColor(new Color(ContextCompat.GetColor(mContext, Resource.Color.menuBackground)));
            }
            return view;
        }

    }
}