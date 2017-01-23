using Softjourn.SJCoins.Core.API.Model.Machines;
using Softjourn.SJCoins.Core.Exceptions;
using Softjourn.SJCoins.Core.Helpers;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
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

        public async void GetMachinesList()
        {
            View.ShowProgress(Resources.StringResources.progress_loading);
           try
            {
                List<Machines> machinesList = await RestApiServise.GetMachinesList();
                Machines selectedMachine = null;
                if (machinesList != null)
                {
                    selectedMachine = GetSelectedMachine(machinesList);
                }
                View.ShowMachinesList(machinesList, selectedMachine);
                View.HideProgress();
            } catch (ApiException ex)
            {
                View.HideProgress();
                AlertService.ShowToastMessage(ex.Message);
            } catch (Exception ex)
            {
                View.HideProgress();
                AlertService.ShowToastMessage(ex.Message);
            }
        }

        public void OnMachineSelected(Machines machine)
        {
            if (machine != null)
            {
                Settings.SelectedMachineId = machine.Id.ToString();
                Settings.SelectedMachineName = machine.Name;
                NavigationService.NavigateToAsRoot(NavigationPage.Main);
            } else
            {
                AlertService.ShowToastMessage("Invalid selected machine");
            }
        }

        private Machines GetSelectedMachine(List<Machines> machinesList)
        {
            string storedMachineId = Settings.SelectedMachineId;
            foreach (var machine in machinesList)
            {
                if (machine.Id.Equals(storedMachineId))
                {
                    return machine;
                }
            }
            return null;
        }
    }
}
