using System;
using BigTed;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Services
{
	public class LoaderService
	{
		//ILoaderService
		public static void Show(string message)
		{
			BTProgressHUD.Show(message, -1, ProgressHUD.MaskType.Clear);
		}

		public static void Hide()
		{
			BTProgressHUD.Dismiss();
		}
	}
}
