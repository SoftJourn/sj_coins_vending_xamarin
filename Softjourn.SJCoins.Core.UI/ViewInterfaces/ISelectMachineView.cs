using Softjourn.SJCoins.Core.API.Model.Machines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softjourn.SJCoins.Core.UI.ViewInterfaces
{
    public interface ISelectMachineView : IBaseView
    {
        void ShowNoMachineView(string message);

        void ShowMachinesList(List<Machines> list, Machines selectedMachine = null);       
    }
}
