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
        void ShowProgress(string message);

        void HideProgress();

        void ShowNoMachineView();

        void ShowMachinesList(List<Machines> list);

       
    }
}
