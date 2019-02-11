using Softjourn.SJCoins.Core.API.Model.Products;
using System;
using System.Collections.Generic;

namespace Softjourn.SJCoins.Core.UI.Services.Alert
{
    public interface IAlertService
    {
        /// <summary>
        /// show dialog with simple text message and one button.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        /// <param name="btnName"></param>
        /// <param name="btnClicked"></param>
        void ShowInformationDialog(string title, string msg, string btnName, Action btnClicked);

        /// <summary>
        /// show dialog with simple message and two buttons - OK and Cancel.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        /// <param name="btnOkClicked"></param>
        /// <param name="btnCancelClicked"></param>
        void ShowConfirmationDialog(string title, string msg, Action btnOkClicked, Action btnCancelClicked);

        /// <summary>
        /// show Toast message on Android or Dialog with text message on Ios
        /// </summary>
        /// <param name="msg"></param>
        void ShowToastMessage(string msg);

        /// <summary>
        /// show SnackBar with message and Button on Android or Dialog with text message and Button on Ios
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        /// <param name="btnName"></param>
        /// <param name="btnClicked"></param>
        void ShowMessageWithUserInteraction(string title, string msg, string btnName, Action btnClicked);

        /// <summary>
        /// show Dialog with product info and button for purchasing
        /// </summary>
        /// <param name="product"></param>
        /// <param name="onPurchaseProductAction"></param>
        void ShowPurchaseConfirmationDialod(Product product, Action<Product> onPurchaseProductAction);

        /// <summary>
        /// show Dialog to select photo source (Camera or Gallery)
        /// </summary>
        /// <param name="photoSource"></param>
        /// <param name="fromCamera"></param>
        /// <param name="fromGallery"></param>
        void ShowPhotoSelectorDialog(List<string> photoSource, Action fromCamera, Action fromGallery);

        /// <summary>
        /// show Dialog to select what to do (Generate QR code or scan existing one)
        /// </summary>
        /// <param name="optionsList"></param>
        /// <param name="scanCode"></param>
        /// <param name="generateCode"></param>
        void ShowQrSelectorDialog(List<string> optionsList, Action scanCode, Action generateCode);
    }
}
