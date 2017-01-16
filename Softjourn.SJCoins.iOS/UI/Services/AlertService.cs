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

		#region IDialogService implementation

		public void ShowConfirmationDialog(string title, string msg, Action btnOkClicked, Action btnCancelClicked)
		{
		}

		public void ShowInformationDialog(string title, string msg, string btnName, Action btnClicked)
		{
		}

		public void ShowMessageWithUserInteraction(string title, string msg, string btnName, Action btnClicked)
		{
		}

		public void ShowToastMessage(string msg)
		{
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




		//private void AddCustomAction(UIAlertController alertController, string title = null, UIAlertActionStyle style = UIAlertActionStyle.Default, Action handler = null)
		//{
		//	if (String.IsNullOrEmpty(title))
		//		var alertAction = UIAlertAction.Create(title, style, ((action) =>
		//	{
		//		if (handler != null)
		//			handler();
		//	});
		//		alertController.AddAction
		//}

	}
