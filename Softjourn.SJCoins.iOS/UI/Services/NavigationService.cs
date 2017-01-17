using System;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.iOS.General.Constants;
using UIKit;

namespace Softjourn.SJCoins.iOS.Services
{
	public class NavigationService : INavigationService
	{
		private AppDelegate _currentApplication;

		public NavigationService()
		{
			_currentApplication = (AppDelegate)UIApplication.SharedApplication.Delegate;
		}

		#region INavigationService implementation

		public void NavigateTo(NavigationPage page)
		{
			try
			{
				//var visibleController = _currentApplication.VisibleViewController;
				//if (visibleController == null)
				//	throw new Exception("Visible Controller is null");


			}
			catch { throw new Exception("Navigation to controller went wrong"); }
		}

		public void NavigateToAsRoot(NavigationPage page)
		{
			try { PresentAsRoot(GetRootController(page)); }
			catch { throw new Exception("Navigation to rootController went wrong"); }
		}

		//void NavigateBackToAdminRoot();

		//void NavigateBack();

		#endregion

		private UIViewController GetRootController(NavigationPage page)
		{
			switch (page)
			{
				// If Welcome page or Login page instantiate from Login storyboard
				case NavigationPage.Welcome:
				case NavigationPage.Login:
					return InstantiateInitial(StoryboardConstants.StoryboardLogin);
				// If Main page instantiate from Main storyboard
				case NavigationPage.Main:
					return InstantiateInitial(StoryboardConstants.StoryboardMain);
				default:
					throw new ArgumentException("Not valid page");
			}
		}

		private UIViewController GetController(NavigationPage page)
		{
			//switch (page)
			//{
				
			//}
		}

		private UIViewController InstantiateInitial(string storyboard)
		{
			return UIStoryboard.FromName(storyboard, null).InstantiateInitialViewController();
		}

		private UIViewController Instantiate(string storyboard, string identifier)
		{
			return UIStoryboard.FromName(storyboard, null).InstantiateViewController(identifier);
		}

		private void PresentAsRoot(UIViewController viewController)
		{
			UIApplication.SharedApplication.KeyWindow.RootViewController = viewController;
		}
	}
}
