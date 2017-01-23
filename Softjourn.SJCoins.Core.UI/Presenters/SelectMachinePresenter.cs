using Softjourn.SJCoins.Core.API.Model.Machines;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
    public class SelectMachinePresenter : BasePresenter<ISelectMachineView>
    {

        public SelectMachinePresenter()
        {

        }

        public List<Machines> GetMachinesList()
        {
            return null;
        }

        public void OnMachineSelected(int machineId)
        {

        }
    }
}
