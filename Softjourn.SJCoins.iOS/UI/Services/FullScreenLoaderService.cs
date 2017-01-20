using System;
using BigTed;

namespace Softjourn.SJCoins.iOS
{
	public class FullScreenLoaderService //: IFullScreenLoaderService
	{
		//ILoaderService
		public void Show()
		{
			BTProgressHUD.Show(null, -1, ProgressHUD.MaskType.Black);
		}

		public void Hide()
		{
			BTProgressHUD.Dismiss();
		}
	}
}
