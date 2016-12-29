using System;
using UIKit;

namespace Softjourn.SJCoins.iOS.Managers
{
	public class AlertManager
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

		public void PresentActionSheet(UIAlertAction[] actions, UIButton sender)
		{
			//Method for presenting action sheet.
		}

		public void PresentConfirmation(string name, int price, UIAlertAction[] actions)
		{
			//Method for presenting buying confirmation alert.
		}
	}
}
