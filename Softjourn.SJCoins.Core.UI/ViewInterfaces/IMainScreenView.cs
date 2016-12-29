
using System.Collections.Generic;

using Softjourn.SJCoins.Core.API.Model.Machines;
using Softjourn.SJCoins.Core.API.Model.Products;

namespace Softjourn.SJCoins.Core.UI.ViewInterfaces
{
    public interface IMainScreenView
    {
        void ShowProgress(string message);

        void HideProgress();

        /**
         * Creates Dialog to confirm or decline purchase
         *
         * @param product = Chosen product
         */
        void NavigateToBuyProduct(Product product);

        /**
         * Load all fragments according to their quantity
         *
         */
        void NavigateToFragments();

        void LoadUserBalance();

        /**
         * Resetting amount after purchase
         * @param amount = response after success purchase
         */
        void UpdateBalanceAmount(string amount);

        /**
         * Dynamically creates category header according to the categoryName
         * and creates container for fragment.
         * @param categoryName = name of category loaded from the server.
         */
        void CreateContainer(string categoryName);

        /**
         * Show dialog for choosing machine.
         * By default is choosing after login.
         * If ther4e is only one machine available dialog is not shown and available
         * machine sets as chosen. If there are multiple machines dialog appears and chosen machine
         * sets as chosen
         * @param machines = response from server on call getMachinesList()
         */
        void ShowMachinesSelector(List<Machines> machines);

        void LoadProductList();

        void GetMachinesList();

        void ShowSnackBar(string message);

        void OnCreateErrorDialog(string message);

        /**
         * Revokes tokens on the server and Navigates to Login Screen
         */
        void LogOut();

        void ShowMessage(string message);

        void ShowNoInternetError();
    }
}
