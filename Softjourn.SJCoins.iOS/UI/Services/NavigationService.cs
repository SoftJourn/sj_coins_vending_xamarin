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

		#region INavigationService implementation
		void Navigate(NavigationPage page)
		{
			try
			{
				if (page == null)
					throw new ArgumentNullException("page");

				var visibleController = _currentApplication.VisibleViewController;
				if (visibleController == null)
					throw new Exception("Visible Controller is null");

				PresentAsRoot(GetRootController(page));
			}
			catch { }
		}

		void NavigateAsRoot(NavigationPage page)
		{					
			try
			{
				if (page == null)
					throw new ArgumentNullException("page");

				PresentAsRoot(GetRootController(page));
			}
			catch { }
		}

		//void NavigateBackToAdminRoot();

		//void NavigateBack();

		#endregion

		private UIViewController GetRootController(NavigationPage page)
		{
			switch (page)
			{
				case NavigationPage.Login:
					storyboard = UIStoryboard.FromName(StoryboardConstants.StoryboardLogin, null);
					break;

				case NavigationPage.Welcome:
					storyboard = UIStoryboard.FromName(StoryboardConstants.StoryboardAdmin, null);
					break;

				default:
					throw new ArgumentException("Not valid page");
			}


		}

		private UIViewController GetController(NavigationPage page)
		{

		}


		private UIViewController Instantiate(string storyboard, string viewcontroller)
		{
			return UIStoryboard.FromName(storyboard, null).InstantiateViewController(viewcontroller);
		}

		private void PresentAsRoot(UIViewController viewController)
		{
			UIApplication.SharedApplication.KeyWindow.RootViewController = viewController;
		}
	}
}
