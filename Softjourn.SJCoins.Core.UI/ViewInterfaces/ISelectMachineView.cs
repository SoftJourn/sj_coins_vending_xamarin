using Softjourn.SJCoins.Core.API.Model.Machines;
using System.Collections.Generic;

namespace Softjourn.SJCoins.Core.UI.ViewInterfaces
{
    public interface ISelectMachineView : IBaseView
    {
        void ShowNoMachineView(string message);

        void ShowMachinesList(List<Machines> list, Machines selectedMachine = null);       
    }
}
