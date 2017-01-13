using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softjourn.SJCoins.Core.UI.Managers
{
    public interface IAlertManager
    {
        void ShowInformationDialog(string title, string msg, string btnName, Action btnClicked);
        void ShowConfirmationDialog(string title, string msg, Action btnOkClicked, Action btnCancelClicked);
        void ShowToastMessage(string msg);
        void ShowMessageWithUserInteraction(string title, string msg, string btnName, Action btnClicked);
    }
}
