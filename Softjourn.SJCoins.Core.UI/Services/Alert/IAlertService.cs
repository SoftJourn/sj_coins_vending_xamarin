using Softjourn.SJCoins.Core.API.Model.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softjourn.SJCoins.Core.UI.Services.Alert
{
    public interface IAlertService
    {   
        // show dialog with simple text message and one button.
        void ShowInformationDialog(string title, string msg, string btnName, Action btnClicked);

        // show dialog with simple message and two buttons - OK and Cancel.
        void ShowConfirmationDialog(string title, string msg, Action btnOkClicked, Action btnCancelClicked);

        // show Toast message on Android or Dialog with text message on Ios
        void ShowToastMessage(string msg);

        // show SnackBar with message and Button on Android or Dialog with text message and Button on Ios
        void ShowMessageWithUserInteraction(string title, string msg, string btnName, Action btnClicked);

        // show Dialog with product info and button for purchaising
        void ShowPurchaseConfirmationDialod(Product product, Action<Product> onPurchaseProductAction);

        void ShowPhotoSelectorDialog(List<string> photoSource, Action fromCamera, Action fromGallery);
    }
}
