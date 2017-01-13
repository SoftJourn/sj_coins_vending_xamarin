using System;
using Softjourn.SJCoins.Core.UI.Managers;
using UIKit;

namespace Softjourn.SJCoins.iOS.Managers
{
	public class AlertManager : IAlertManager
	{
		public AlertManager()
		{
		}

		public void PresentAlert(string message)
		{
			//Method for presenting informative messages.
			var controller = UIAlertController.Create("", message, preferredStyle: UIAlertControllerStyle.Alert);
			controller.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Cancel, null));
			// visibleViewController.PresentViewController(alert, animated: true, completionHandler: null);
		}

		//public void PresentActionSheet(UIAlertAction[] actions, UIButton sender)
		//{
		//	//Method for presenting action sheet.
		//}

		//public void PresentConfirmation(string name, int price, UIAlertAction[] actions)
		//{
		//	//Method for presenting buying confirmation alert.
		//}

		public void ShowInformationDialog(string title, string msg, string btnName, Action btnClicked)
		{
			// realization 1 button alert
		}

		public void ShowConfirmationDialog(string title, string msg, Action btnOkClicked, Action btnCancelClicked)
		{
			// realization 2 button alert
		}

		public void ShowToastMessage(string msg)
		{
			// realization 1 button alert withut title
		}

		public void ShowMessageWithUserInteraction(string title, string msg, string btnName, Action btnClicked)
		{
			// realization 1 button alert
		}
	}
}
