using System;
using Softjourn.SJCoins.Core.UI.Services.Alert;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Services
{
	public class AlertService : IAlertService
	{
		private AppDelegate _currentApplicationDelegate;

		public AlertService()
		{
			_currentApplicationDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
		}

		#region IAlertService implementation

		public void ShowConfirmationDialog(string title, string msg, Action btnOkClicked, Action btnCancelClicked)
		{
			// Present confirmation alert with two buttons  
			PresentAlert(title, msg, null, null, UIAlertActionStyle.Default, btnOkClicked, btnCancelClicked);
		}

		public void ShowInformationDialog(string title, string msg, string btnName, Action btnClicked)
		{
			// Present information alert with one botton
			PresentAlert(title, msg, btnName, null, UIAlertActionStyle.Default, btnClicked, null);
		}

		public void ShowMessageWithUserInteraction(string title, string msg, string btnName, Action btnClicked)
		{
			// Present information alert with one botton
			PresentAlert(title, msg, btnName, null, UIAlertActionStyle.Default, btnClicked, null);
		}

		public void ShowToastMessage(string msg)
		{
			// Present information alert with one botton
			PresentAlert(null, msg, null, null, UIAlertActionStyle.Default, null, null);
		}

		#endregion

		public void ShowActionSheet(string title, string message, string[] items, Action[] itemActions,
			Action cancelClicked = null)
		{
			UIApplication.SharedApplication.InvokeOnMainThread(() =>
			{
				var alertController = UIAlertController.Create(title, message, UIAlertControllerStyle.ActionSheet);
				foreach (string action in items)
				{
					//AddCustomAction(alertController, items[action], UIAlertActionStyle.Default, action);
				}
			});
		}

		private void PresentAlert(string title, string message, string accept, string cancel, UIAlertActionStyle acceptStyle, Action acceptClicked = null, Action cancelClicked = null)
		{
			UIApplication.SharedApplication.InvokeOnMainThread(() =>
				{
					try
					{
						var alertController = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);

						AddCustomAction(alertController, cancel, UIAlertActionStyle.Cancel, cancelClicked);
						AddCustomAction(alertController, accept, acceptStyle, acceptClicked);

						_currentApplicationDelegate.VisibleViewController.PresentViewController(alertController, true, null);
					}
					catch { }
				});
		}

		private void AddCustomAction(UIAlertController alertController, string title = null, UIAlertActionStyle style = UIAlertActionStyle.Default, Action handler = null)
		{
			if (String.IsNullOrEmpty(title))
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
