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

        public bool IsMachineSet()
        {
            return Settings.SelectedMachineId != "";
        }

        public async void GetMachinesList()
        {
            View.ShowProgress(Resources.StringResources.progress_loading);
           try
            {
                List<Machines> machinesList = await RestApiServise.GetMachinesListAsync();
                View.HideProgress();                
                if (machinesList != null && machinesList.Count != 0)
                {
                    if (machinesList.Count == 1)
                    {
						Settings.OnlyOneVendingMachine = true;
                        OnMachineSelected(machinesList.First<Machines>());
                    } else {
						Settings.OnlyOneVendingMachine = false;
						Machines selectedMachine = GetSelectedMachine(machinesList);
                        View.ShowMachinesList(machinesList, selectedMachine);
                    }                   
                } else
                {
                    View.ShowNoMachineView(Resources.StringResources.error_msg_empty_machines_list);
                }

            }
            catch (ApiNotAuthorizedException ex)
            {
                View.HideProgress();
                AlertService.ShowToastMessage(ex.Message);
                DataManager.Profile = null;
                Settings.ClearUserData();
                NavigationService.NavigateToAsRoot(NavigationPage.Login);
            }
            catch (Exception ex)
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
                NavigationService.NavigateToAsRoot(NavigationPage.Home);
            } else
            {
                AlertService.ShowToastMessage(Resources.StringResources.error_msg_invalid_selected_machine);
            }
        }

        private Machines GetSelectedMachine(List<Machines> machinesList)
        {
            string storedMachineId = Settings.SelectedMachineId;
            foreach (var machine in machinesList)
            {
                if (machine.Id.ToString().Equals(storedMachineId))
                {
                    return machine;
                }
            }
            return null;
        }
    }
}
