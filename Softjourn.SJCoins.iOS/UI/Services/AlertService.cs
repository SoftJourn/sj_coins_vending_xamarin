using System;
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
			//PresentAlert(null, msg, "Ok", null, UIAlertActionStyle.Default, null, null);
			//BTProgressHUD.ShowToast(msg, true, 1000.0);
		}

		public void ShowPurchaseConfirmationDialod(Product product, Action<Product> onPurchaseProductAction)
		{
			// Present purchace confirmation alert with two buttons
			string price = product.Price.ToString();
			string confirmMessage = "Buy " + product.Name + " for the " + price + " coins";

			PresentAlert(confirmTitle, confirmMessage, "Confirm", "Cancel", UIAlertActionStyle.Default, onPurchaseProductAction, null, product);
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
	}
}
