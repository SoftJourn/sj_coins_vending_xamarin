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
using Softjourn.SJCoins.Core.API.Model.Machines;


namespace Softjourn.SJCoins.Droid.ui.adapters
{
    public class SelectMachineListAdapter : BaseAdapter<Machines>
    {
        private List<Machines> _items;

        private Context _context;

        private string _selectedMachine = "";

        public SelectMachineListAdapter(Context context, List<Machines> list) : base()
        {
            _items = list;
            _context = context;
        }


        public Machines GetMachine(int position)
        {
            return _items[position];
        }

        public override long GetItemId(int position)
        {
            return position;
        }
        public override Machines this[int position]
        {
            get { return _items[position]; }
        }
        public override int Count
        {
            get { return _items.Count; }
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            var view = convertView;
            if (view == null)
            {
                view = LayoutInflater.From(_context).Inflate(Resource.Layout.select_machine_text_view, null, false);
            }

            var machines = this[position];

            var machineName = view.FindViewById<TextView>(Resource.Id.text1);
            machineName.Text = machines.Name;

        machineName.SetTextColor(machines.Name == _selectedMachine
            ? new Color(ContextCompat.GetColor(_context, Resource.Color.colorBlue))
            : new Color(ContextCompat.GetColor(_context, Resource.Color.menuBackground)));
        return view;
        }

        public void SetData(List<Machines> list)
        {
            _items = list;
            NotifyDataSetChanged();
        }

        public void SetSelectedMachine(Machines machine)
        {
            if (machine != null)
            {
                _selectedMachine = machine.Name;
            }
        }

    }
}