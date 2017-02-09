using System;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.iOS.General.Constants;
using UIKit;

namespace Softjourn.SJCoins.iOS.Services
{
	public class NavigationService : INavigationService
	{
		private AppDelegate _currentApplication 
		{ 
			get { return (AppDelegate)UIApplication.SharedApplication.Delegate; }
		}

		#region INavigationService implementation

		public void NavigateTo(NavigationPage page)
		{
			try { Navigate(page); }
			catch { throw new Exception("Navigation to controller went wrong"); }
		}

		public void NavigateToAsRoot(NavigationPage page)
		{
			try { PresentAs(RootController(page)); }
			catch { throw new Exception("Navigation to rootController went wrong"); }
		}

		//void NavigateBackToAdminRoot();

		//void NavigateBack();

		#endregion

		private UIViewController RootController(NavigationPage page)
		{
			switch (page)
			{
				// If Welcome page or Login page instantiate from Login storyboard
				case NavigationPage.Welcome:
					return Instantiate(StoryboardConstants.StoryboardLogin, StoryboardConstants.InformativeViewController);
				case NavigationPage.Login:
					return Instantiate(StoryboardConstants.StoryboardLogin, StoryboardConstants.LoginViewController);
				// If Home page or SelectMachine page instantiate from Main storyboard
				case NavigationPage.SelectMachine:
					return Instantiate(StoryboardConstants.StoryboardMain, StoryboardConstants.SelectMachineViewController);
				case NavigationPage.Home:
					return Instantiate(StoryboardConstants.StoryboardMain, StoryboardConstants.MainTabBarViewController) as UITabBarController;
				default:
					throw new ArgumentException("Not valid page");
			}
		}

		private UIViewController GetController(NavigationPage page)
		{
			switch (page)
			{
				// If Settings page instantiate from Login storyboard
				case NavigationPage.Settings:
					return Instantiate(StoryboardConstants.StoryboardMain, StoryboardConstants.SelectMachineViewController);
				case NavigationPage.Detail:
					return Instantiate(StoryboardConstants.StoryboardMain, StoryboardConstants.DetailViewController);
				default:
					throw new ArgumentException("Not valid page");
			}
		}

		private UIViewController Instantiate(string storyboard, string identifier) => UIStoryboard.FromName(storyboard, null).InstantiateViewController(identifier);

		private void PresentAs(UIViewController viewController)
		{
			UIApplication.SharedApplication.KeyWindow.RootViewController = viewController;
		}

		private void Navigate(NavigationPage page)
		{
			var visibleController = _currentApplication.VisibleViewController;

			if (visibleController != null)
			{
				switch (page)
				{
					case NavigationPage.Detail:
						visibleController.NavigationController.PushViewController(GetController(page), animated: true);
						break;
					default:
						throw new ArgumentException("Not valid page");
				}
			}
			else {
				throw new Exception("Visible Controller is null");
			}
		}
	}
}
