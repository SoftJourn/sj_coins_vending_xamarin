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
        private readonly List<string> _items;

        private readonly Context _context;

        private string _selectedMachine = "";

        public SelectMachineListAdapter(Context context, int resource, List<string> list, string selectedMachine = null)
        {
            _items = list;
            _context = context;
            if (selectedMachine != null)
            {
                _selectedMachine = selectedMachine;
            }
        }


    public override int Count => _items.Count;

        public override Object GetItem(int position)
        {
            return _items[position];
        }

    public override long GetItemId(int position)
        {
            return position;
        }


    public override View GetView(int position, View convertView, ViewGroup parent)
        {

            var view = convertView;
            if (view == null)
            {
                LayoutInflater li;
                li = LayoutInflater.From(_context);
                view = li.Inflate(Resource.Layout.select_machine_text_view, null);
            }

            var name = GetItem(position).ToString();

            var machineName = view.FindViewById<TextView>(Resource.Id.text1);
            machineName.Text = name;

        machineName.SetTextColor(name == _selectedMachine
            ? new Color(ContextCompat.GetColor(_context, Resource.Color.colorBlue))
            : new Color(ContextCompat.GetColor(_context, Resource.Color.menuBackground)));
        return view;
        }

    }
}