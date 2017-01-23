using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
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
    [Activity(Label = "Select Machine", Theme = "@style/AppThemeForCustomToolbar")]
    public class SelectMachineActivity : BaseActivity<SelectMachinePresenter>, ISelectMachineView
    {

        private SwipeRefreshLayout _swipeLayout;
        private TextView _noMachinesTextView;
        private ListView _machineListView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_select_machine);

            _noMachinesTextView = FindViewById<TextView>(Resource.Id.textViewNoMachines);
            _machineListView = FindViewById<ListView>(Resource.Id.listViewMachines);

            _swipeLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_container);
            _swipeLayout.SetColorSchemeColors(GetColor(Resource.Color.colorAccent));
            _swipeLayout.Refresh += (s, e) =>
            {
                ViewPresenter.GetMachinesList();
            };

            ViewPresenter.GetMachinesList();
        }

        public void ShowNoMachineView()
        {
            _noMachinesTextView.Visibility = ViewStates.Visible;
        }

        public void ShowMachinesList(List<Machines> list, Machines selectedMachine = null)
        {
            _machineListView.Visibility = ViewStates.Visible;
            var names = list.Select(machine => machine.Name).ToList();

            var adapter = new SelectMachineListAdapter(this,
                Android.Resource.Layout.SimpleListItem1, names, selectedMachine?.Name);
            _machineListView.Adapter = adapter;
            _machineListView.ItemClick += (sender, e) =>
            {
                foreach (var machine in list)
                {
                    if (adapter.GetItem(e.Position).ToString() == machine.Name)
                    {
                        ViewPresenter.OnMachineSelected(machine.Id);
                    }
                }
            };

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