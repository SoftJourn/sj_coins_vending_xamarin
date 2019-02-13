using System;
using System.Collections.Generic;
using System.Linq;
using Softjourn.SJCoins.Core.Common;
using Softjourn.SJCoins.Core.Common.Exceptions;
using Softjourn.SJCoins.Core.Common.Utils;
using Softjourn.SJCoins.Core.Models.Machines;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
    public class SelectMachinePresenter : BasePresenter<ISelectMachineView>
    {
        public SelectMachinePresenter() { }

        public bool IsMachineSet()
        {
            return Settings.SelectedMachineId != string.Empty;
        }

        public async void GetMachinesList()
        {
            View.ShowProgress(Resources.UiMessageResources.progress_loading);

            try
            {
                var machinesList = await RestApiService.GetMachinesListAsync();
                View.HideProgress();
                if (machinesList != null && machinesList.Count != 0)
                {
                    if (machinesList.Count == 1)
                    {
                        Settings.OnlyOneVendingMachine = true;
                        OnMachineSelected(machinesList.First());
                    }
                    else
                    {
                        Settings.OnlyOneVendingMachine = false;
                        var selectedMachine = GetSelectedMachine(machinesList);
                        View.ShowMachinesList(machinesList, selectedMachine);
                    }
                }
                else
                {
                    View.ShowNoMachineView(Resources.UiMessageResources.error_msg_empty_machines_list);
                }
            }
            catch (ApiNotAuthorizedException)
            {
                View.HideProgress();
                //AlertService.ShowToastMessage(ex.Message);
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
            }
            else
            {
                AlertService.ShowToastMessage(Resources.UiMessageResources.error_msg_invalid_selected_machine);
            }
        }

        private Machines GetSelectedMachine(IEnumerable<Machines> machinesList)
        {
            var storedMachineId = Settings.SelectedMachineId;
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
