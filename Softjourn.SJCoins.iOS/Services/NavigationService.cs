using System;
using UIKit;

namespace Softjourn.SJCoins.iOS.Services
{
	public class NavigationService //: INavigationService
	{
		private AppDelegate _currentApplication;

		public NavigationService()
		{
			_currentApplication = (AppDelegate)UIApplication.SharedApplication.Delegate;
		}

		// INavigationService interface
		//void Navigate(INavigationParameters navigationParams);

		//void NavigateAsRoot(INavigationParameters navigationParams);

		//void NavigateBackToAdminRoot();

		//void NavigateBack();
	}
}
