using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using Softjourn.SJCoins.Core.Models.Machines;

namespace Softjourn.SJCoins.Droid.ui.adapters
{
    public class SelectMachineListAdapter : BaseAdapter<Machines>
    {
        private List<Machines> _items;
        private readonly Context _context;
        private string _selectedMachine = string.Empty;

        public override Machines this[int position] => _items[position];

        public override int Count => _items.Count;

        public SelectMachineListAdapter(Context context, List<Machines> list)
        {
            _items = list;
            _context = context;
        }

        public Machines GetMachine(int position) => _items[position];

        public override long GetItemId(int position) => position;

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            if (view == null)
                view = LayoutInflater.From(_context).Inflate(Resource.Layout.select_machine_text_view, null, false);

            var machines = this[position];
            var machineName = view.FindViewById<TextView>(Resource.Id.text1);
            machineName.Text = machines.Name;

            //Set Color to selected machine name to highlight it
            machineName.SetTextColor(machines.Name == _selectedMachine
                ? new Color(ContextCompat.GetColor(_context, Resource.Color.colorBlue))
                : new Color(ContextCompat.GetColor(_context, Resource.Color.menuBackground)));

            return view;
        }

        /// <summary>
        /// Setting Machines List and redrawing listView
        /// </summary>
        /// <param name="list"></param>
        public void SetData(List<Machines> list)
        {
            _items = list;
            NotifyDataSetChanged();
        }

        /// <summary>
        /// If Machine is selected set this machine as selected one
        /// </summary>
        /// <param name="machine"></param>
        public void SetSelectedMachine(Machines machine)
        {
            if (machine != null)
                _selectedMachine = machine.Name;
        }
    }
}