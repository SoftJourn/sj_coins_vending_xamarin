using System;
using UIKit;

namespace Softjourn.SJCoins.iOS
{
	public class AlertService : IAlertService
	{
		private AppDelegate _currentApplicationDelegate;

		public AlertService()
		{
			_currentApplicationDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
		}

		#region IDialogService implementation


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
