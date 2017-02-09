using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using Softjourn.SJCoins.Core.API.Model.Machines;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Droid.ui.adapters;
using Softjourn.SJCoins.Droid.ui.baseUI;

namespace Softjourn.SJCoins.Droid.UI.Activities
{
    [Activity(Label = "Select Machine", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class SelectMachineActivity : BaseActivity<SelectMachinePresenter>, ISelectMachineView
    {

        private SwipeRefreshLayout _swipeLayout;
        private TextView _noMachinesTextView;
        private ListView _machineListView;
        private List<Machines> _machinesList = new List<Machines>();
        private SelectMachineListAdapter _adapter; 

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_select_machine);
            Title = "Select Machine";

            _noMachinesTextView = FindViewById<TextView>(Resource.Id.textViewNoMachines);
            _machineListView = FindViewById<ListView>(Resource.Id.listViewMachines);

            _adapter = new SelectMachineListAdapter(this, _machinesList);
            _machineListView.Adapter = _adapter;

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            _swipeLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_container);
            _swipeLayout.SetColorSchemeColors(Resource.Color.colorAccent);
            _swipeLayout.Refresh += (s, e) =>
            {
                ViewPresenter.GetMachinesList();
            };

            ViewPresenter.GetMachinesList();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            return false;
        }

        public void ShowNoMachineView(string message)
        {
            _noMachinesTextView.Visibility = ViewStates.Visible;
            _noMachinesTextView.Text = message;
        }
        
        public void ShowMachinesList(List<Machines> list, Machines selectedMachine = null)
        {
            _machinesList = list;
            _machineListView.Visibility = ViewStates.Visible;


            _adapter.SetSelectedMachine(selectedMachine);
            _adapter.SetData(list);
            _machineListView.ItemClick += OnListItemClick;
        }

        private void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            foreach (var machine in _machinesList)
            {
                if (_adapter.GetMachine(e.Position).Name != machine.Name) continue;
                ViewPresenter.OnMachineSelected(machine);
                break;
            }
        }

        public override void ShowProgress(string message)
        {
            _swipeLayout.Refreshing = true;
        }

        public override void HideProgress()
        {
            _swipeLayout.Refreshing = false;
        }
    }
}