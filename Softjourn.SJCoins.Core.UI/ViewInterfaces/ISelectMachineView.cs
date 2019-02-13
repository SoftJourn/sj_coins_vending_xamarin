using System.Collections.Generic;
using Softjourn.SJCoins.Core.Models.Machines;

namespace Softjourn.SJCoins.Core.UI.ViewInterfaces
{
    public interface ISelectMachineView : IBaseView
    {
        void ShowNoMachineView(string message);

        void ShowMachinesList(List<Machines> list, Machines selectedMachine = null);       
    }
}
