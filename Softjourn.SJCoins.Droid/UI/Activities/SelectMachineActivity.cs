using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Softjourn.SJCoins.Core.API.Model.Machines;
using Softjourn.SJCoins.Droid.ui.baseUI;

namespace Softjourn.SJCoins.Droid.UI.Activities
{
    [Activity(Label = "Select Machine", Theme = "@style/AppThemeForCustomToolbar")]
    public class SelectMachineActivity : BaseActivity<SelectMachinePresenter>, ISelectMachineView
    {
        private List<Machines> _machinesList;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ViewPresenter.GetMachines();
        }


    }
}