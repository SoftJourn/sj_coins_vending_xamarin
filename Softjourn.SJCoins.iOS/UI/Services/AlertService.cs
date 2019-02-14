﻿using System;
using System.Collections.Generic;
using System.Globalization;
using Softjourn.SJCoins.Core.Models.Products;
using Softjourn.SJCoins.Core.UI.Services.Alert;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Services
{
    public class AlertService : IAlertService
    {
        private const string ConfirmTitle = "Confirm Purchase";

        private readonly AppDelegate _currentApplicationDelegate;

        public AlertService()
        {
            _currentApplicationDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
        }

        #region IAlertService implementation

        public void ShowConfirmationDialog(string title, string msg, Action btnOkClicked, Action btnCancelClicked) =>
            PresentAlert(title, msg, null, null, UIAlertActionStyle.Default); // Present confirmation alert with two buttons  

        public void ShowInformationDialog(string title, string msg, string btnName, Action btnClicked) =>
            PresentAlert(title, msg, btnName, null, UIAlertActionStyle.Default); // Present information alert with one button

        public void ShowMessageWithUserInteraction(string title, string msg, string btnName, Action btnClicked) =>
            PresentAlert(title, msg, btnName, null, UIAlertActionStyle.Default); // Present information alert with one button (after purchase message)

        public void ShowToastMessage(string msg) =>
            PresentAlert(string.Empty, msg, "Ok", null, UIAlertActionStyle.Default); // Present information alert with one bottom

        public void ShowPurchaseConfirmationDialog(Product product, Action<Product> onPurchaseProductAction)
        {
            // Present purchase confirmation alert with two buttons
            var price = product.Price.ToString(CultureInfo.InvariantCulture);
            var confirmMessage = $"Buy {product.Name} for the {price} coins";

            PresentAlert(ConfirmTitle, confirmMessage, "Confirm", "Cancel", UIAlertActionStyle.Default, onPurchaseProductAction, null, product);
        }

        public void ShowPhotoSelectorDialog(List<string> photoSource, Action fromCamera, Action fromGallery)
        {
            // Show action sheet with 2 buttons
            var actions = new List<Action>
            {
                fromCamera,
                fromGallery
            };
            PresentActionSheet(null, null, photoSource.ToArray(), actions.ToArray());
        }

        public void ShowQrSelectorDialog(List<string> optionsList, Action scanCode, Action generateCode)
        {
            // Show action sheet with 2 buttons
            var actions = new List<Action>
            {
                scanCode,
                generateCode
            };
            PresentActionSheet(null, null, optionsList.ToArray(), actions.ToArray());
        }

        #endregion

        #region Private methods

        private void PresentAlert(string title, string message, string accept, string cancel, UIAlertActionStyle acceptStyle, Action<Product> acceptClicked = null, Action cancelClicked = null, Product product = null)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                try
                {
                    var alertController = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
                    alertController.View.TintColor = UIColorConstants.MainGreenColor;
                    if (acceptClicked != null)
                    {
                        var cancelAction = UIAlertAction.Create(cancel, UIAlertActionStyle.Cancel, null);
                        var acceptAction = UIAlertAction.Create(accept, acceptStyle, (action) => { acceptClicked?.Invoke(product); });
                        alertController.AddAction(cancelAction);
                        alertController.AddAction(acceptAction);
                    }
                    else
                    {
                        var okAction = UIAlertAction.Create("Ok", UIAlertActionStyle.Cancel, null);
                        alertController.AddAction(okAction);
                    }
                    _currentApplicationDelegate.VisibleViewController.PresentViewController(alertController, true, null);
                }
                catch { }
            });
        }

        private void PresentActionSheet(string title, string message, IReadOnlyList<string> items, IReadOnlyList<Action> itemActions)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                var alertController = UIAlertController.Create(title, message, UIAlertControllerStyle.ActionSheet);
                alertController.View.TintColor = UIColorConstants.MainGreenColor;
                for (var i = 0; i < items.Count; i++)
                {
                    AddActionToAlert(alertController, items[i], UIAlertActionStyle.Default, itemActions[i]);
                }
                AddActionToAlert(alertController, "Cancel", UIAlertActionStyle.Cancel, null);

                _currentApplicationDelegate.VisibleViewController.PresentViewController(alertController, true, null);
            });
        }

        private static void AddActionToAlert(UIAlertController alertController, string title = null, UIAlertActionStyle style = UIAlertActionStyle.Default, Action handler = null)
        {
            if (!string.IsNullOrEmpty(title))
            {
                var alertAction = UIAlertAction.Create(title, style, action => { handler?.Invoke(); });
                alertController.AddAction(alertAction);
            }
        }

        #endregion

        #region Public methods

        public void ShowConfirmationAlert(string title, string msg, Action btnOkClicked, Action btnCancelClicked)
        {
            // Present confirmation alert with two buttons  
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                try
                {
                    var alertController = UIAlertController.Create(title, msg, UIAlertControllerStyle.Alert);
                    alertController.View.TintColor = UIColorConstants.MainGreenColor;
                    var cancelAction = UIAlertAction.Create("cancel", UIAlertActionStyle.Cancel, (action) => { btnCancelClicked?.Invoke(); });
                    var acceptAction = UIAlertAction.Create("accept", UIAlertActionStyle.Default, (action) => { btnOkClicked?.Invoke(); });
                    alertController.AddAction(cancelAction);
                    alertController.AddAction(acceptAction);
                    _currentApplicationDelegate.VisibleViewController.PresentViewController(alertController, true, null);
                }
                catch { }
            });
        }

        #endregion
    }
}
