using System;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.iOS.General.Constants;
using Softjourn.SJCoins.iOS.UI.Controllers;
using Softjourn.SJCoins.iOS.UI.Controllers.AccountPage;
using Softjourn.SJCoins.iOS.UI.Controllers.Main;
using UIKit;

namespace Softjourn.SJCoins.iOS.Services
{
	public class NavigationService : INavigationService
	{
		private enum NavigationType
		{
			Push, Present, PresentExisting
		}
		private AppDelegate _currentApplication 
		{ 
			get { return (AppDelegate)UIApplication.SharedApplication.Delegate; }
		}

		#region INavigationService implementation
		public void NavigateTo(NavigationPage page, object initialParameter = null)
		{
			try { Present(page, initialParameter); }
			catch { throw new Exception("Navigation to controller went wrong"); }
		}

		public void NavigateToAsRoot(NavigationPage page)
		{
			try { PresentAsRoot(InitializeControllerWith(page)); }
			catch { throw new Exception("Navigation to rootController went wrong"); }
		}
		#endregion

		#region Private methods
		// -------------------------- Basic methods ---------------------------
		private void Present(NavigationPage page, object initialParameter = null)
		{
			switch (page)
			{
				// Push with initial parameter
				case NavigationPage.ShowAll:
				case NavigationPage.Detail:
				case NavigationPage.ShareFuns:
					ShowControllerWith(page, NavigationType.Push, initialParameter);
					break;	
					
				// Present without initial parameter	
				case NavigationPage.Profile:
					ShowControllerWith(page, NavigationType.Present);
					break;

				// Push without initial parameter	
				case NavigationPage.Purchase:
				case NavigationPage.Reports:
				case NavigationPage.PrivacyTerms:
				case NavigationPage.Help:
				case NavigationPage.SelectMachine:
					ShowControllerWith(page, NavigationType.Push);
					break;
					
				default:
					throw new ArgumentException("Not valid page");
			}
		}

		private void PresentAsRoot(UIViewController viewController)
		{
			UIApplication.SharedApplication.KeyWindow.RootViewController = viewController;
		}
		// --------------------------------------------------------------------

		// ----------------------- Navigation helpers -------------------------
		private void ShowControllerWith(NavigationPage page, NavigationType navigationType, object initialParameter = null)
		{
			var visibleController = _currentApplication.VisibleViewController;
			if (visibleController != null)
			{
				switch (navigationType)
				{
					case NavigationType.Push:
						PushController(page, visibleController, initialParameter);
						break;
					case NavigationType.Present:
						PresentController(page, visibleController);
						break;
					default:
						break;
				}
			}
			else {
				throw new Exception("Visible Controller is null");
			}
		}

		private void PushController(NavigationPage page, UIViewController visibleController, object initialParameter = null) 
		{ 
			visibleController.NavigationController.PushViewController(InitializeControllerWith(page, initialParameter), animated: true); 
		}

		private void PresentController(NavigationPage page, UIViewController visibleController)
		{
			visibleController.PresentViewController(InitializeControllerWith(page), animated: true, completionHandler: null);
		}
		// --------------------------------------------------------------------

		// -------------------- Controllers initialization --------------------
		private UIViewController InitializeControllerWith(NavigationPage page, object parameter = null)
		{
			switch (page)
			{
				// If Welcome page or Login page instantiate from Login storyboard
				case NavigationPage.Welcome:
					return Instantiate(StoryboardConstants.StoryboardLogin, StoryboardConstants.InformativeViewController);
				case NavigationPage.Login:
					return Instantiate(StoryboardConstants.StoryboardLogin, StoryboardConstants.LoginViewController);

				// If Home, Detail and ShowAll pages instantiate from Main storyboard
				case NavigationPage.Home:
					return Instantiate(StoryboardConstants.StoryboardMain, StoryboardConstants.NavigationHomeViewController);
				case NavigationPage.Detail:
					var detailController = (DetailViewController)Instantiate(StoryboardConstants.StoryboardMain, StoryboardConstants.DetailViewController);
					detailController.SetInitialParameter(parameter);
					return detailController;
				case NavigationPage.ShowAll:
					var showAllController = (ShowViewController)Instantiate(StoryboardConstants.StoryboardMain, StoryboardConstants.ShowViewController);
					showAllController.SetInitialParameter(parameter);
					return showAllController;
					
				// Profile, Purchase, Reports, PrivacyTerms, Help, ShareFuns, SelectMachine pages instantiate from Account storyboard
				case NavigationPage.Profile:
					return Instantiate(StoryboardConstants.StoryboardAccount, StoryboardConstants.NavigationAccountViewController);
				case NavigationPage.Purchase:
					return Instantiate(StoryboardConstants.StoryboardAccount, StoryboardConstants.PurchaseViewController);
				case NavigationPage.Reports:
					return Instantiate(StoryboardConstants.StoryboardAccount, StoryboardConstants.ReportsViewController);
				case NavigationPage.PrivacyTerms:
					return Instantiate(StoryboardConstants.StoryboardAccount, StoryboardConstants.PrivacyTermsViewController);
				case NavigationPage.Help:
					return Instantiate(StoryboardConstants.StoryboardAccount, StoryboardConstants.HelpViewController);
				case NavigationPage.ShareFuns:
					var qrcodeViewController = (QRCodeViewController)Instantiate(StoryboardConstants.StoryboardAccount, StoryboardConstants.QRCodeViewController);
					qrcodeViewController.SetInitialParameter(parameter);
					return qrcodeViewController;
				case NavigationPage.SelectMachine:
					return Instantiate(StoryboardConstants.StoryboardAccount, StoryboardConstants.SelectMachineViewController);

				default:
					throw new ArgumentException("Not valid page");
			}
		}

		private UIViewController Instantiate(string storyboard, string identifier) => UIStoryboard.FromName(storyboard, null).InstantiateViewController(identifier);
		// --------------------------------------------------------------------
		#endregion
	}
}
