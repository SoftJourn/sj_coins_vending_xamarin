using System;
using System.Collections.Generic;
using System.Linq;
using BigTed;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Core.UI.Services.Alert;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Services
{
	public class AlertService : IAlertService
	{
		private const string confirmTitle = "Confirm Purchase";

		private AppDelegate _currentApplicationDelegate;

		public AlertService()
		{
			_currentApplicationDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
		}

		#region IAlertService implementation
		public void ShowConfirmationDialog(string title, string msg, Action btnOkClicked, Action btnCancelClicked)
		{
			// Present confirmation alert with two buttons  
			PresentAlert(title, msg, null, null, UIAlertActionStyle.Default, null, null);
		}

		public void ShowInformationDialog(string title, string msg, string btnName, Action btnClicked)
		{
			// Present information alert with one button
			PresentAlert(title, msg, btnName, null, UIAlertActionStyle.Default, null, null);
		}

		public void ShowMessageWithUserInteraction(string title, string msg, string btnName, Action btnClicked)
		{
			// Present information alert with one button (after purchase message)
			PresentAlert(title, msg, btnName, null, UIAlertActionStyle.Default, null, null);
		}

		public void ShowToastMessage(string msg)
		{
			// Present information alert with one botton
			PresentAlert("", msg, "Ok", null, UIAlertActionStyle.Default, null, null);
			//BTProgressHUD.ShowToast(msg, true, 1000.0);
		}

		public void ShowPurchaseConfirmationDialod(Product product, Action<Product> onPurchaseProductAction)
		{
			// Present purchace confirmation alert with two buttons
			string price = product.Price.ToString();
			string confirmMessage = "Buy " + product.Name + " for the " + price + " coins";

			PresentAlert(confirmTitle, confirmMessage, "Confirm", "Cancel", UIAlertActionStyle.Default, onPurchaseProductAction, null, product);
		}

		public void ShowPhotoSelectorDialog(List<string> photoSource, Action fromCamera, Action fromGallery)
		{
			// Show action sheet with 2 buttons
			var actions = new List<Action>();
			actions.Add(fromCamera);
			actions.Add(fromGallery);

			PresentActionSheet(null, null, photoSource.ToArray(), actions.ToArray());
		}

		public void ShowQrSelectorDialog(List<string> optionsList, Action scanCode, Action generateCode)
		{
			// Show action sheet with 2 buttons
			var actions = new List<Action>();
			actions.Add(scanCode);
			actions.Add(generateCode);

			PresentActionSheet(null, null, optionsList.ToArray(), actions.ToArray());
		}
		#endregion

		private void PresentAlert(string title, string message, string accept, string cancel, UIAlertActionStyle acceptStyle, Action<Product> acceptClicked = null, Action cancelClicked = null, Product product = null)
		{
			UIApplication.SharedApplication.InvokeOnMainThread(() =>
				{
				try
				{
					var alertController = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
					if (acceptClicked != null)
					{
						var cancelAction = UIAlertAction.Create(cancel, UIAlertActionStyle.Cancel, null);
						var acceptAction = UIAlertAction.Create(accept, acceptStyle, (action) =>
						{
							if (acceptClicked != null)
							{
								acceptClicked(product);
							}
						});
					alertController.AddAction(cancelAction);
					alertController.AddAction(acceptAction);
					}
					else {
						var okAction = UIAlertAction.Create("Ok", UIAlertActionStyle.Cancel, null);
						alertController.AddAction(okAction);
					}
					_currentApplicationDelegate.VisibleViewController.PresentViewController(alertController, true, null);
				}
				catch { }
				});
		}

		private void PresentActionSheet(string title, string message, string[] items, Action[] itemActions)
		{
			UIApplication.SharedApplication.InvokeOnMainThread(() =>
			{
				var alertController = UIAlertController.Create(title, message, UIAlertControllerStyle.ActionSheet);
                alertController.View.TintColor = UIColorConstants.MainGreenColor;
				for (int i = 0; i < items.Length; i++)
				{
					AddActionToAlert(alertController, items[i], UIAlertActionStyle.Default, itemActions[i]);
				}
				AddActionToAlert(alertController, "Cancel", UIAlertActionStyle.Cancel, null);

				_currentApplicationDelegate.VisibleViewController.PresentViewController(alertController, true, null);
			});
		}

		private void AddActionToAlert(UIAlertController alertController, string title = null, UIAlertActionStyle style = UIAlertActionStyle.Default, Action handler = null)
		{
			if (!String.IsNullOrEmpty(title))
			{
				var alertAction = UIAlertAction.Create(title, style, (action) =>
				{
					if (handler != null)
					{
						handler();
					}
				});
				alertController.AddAction(alertAction);
			}
		}
	}
}
