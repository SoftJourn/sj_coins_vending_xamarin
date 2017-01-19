using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using Foundation;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.iOS.General.Constants;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers.Informative
{
	[Register("InformativeViewController")]
	public partial class InformativeViewController : BaseViewController<WelcomePresenter>, IWelcomeView
	{
		#region Properties
		private UIPageViewController pageViewController;
		private static List<ContentViewController> _pages;
		private static int currentIndex = 0;
		private static int pendingIndex;

		private static UIButton gotItButton;
		private static UIPageControl pageControl;
		#endregion

		#region Controller Life cycle
		public InformativeViewController(IntPtr handle) : base(handle)
		{  
		}
		#endregion

		#region Controller Life cycle
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Create informative content
			_pages = new List<ContentViewController>();
			_pages = CreateInformativePages();

			ConfigurePageViewController();
			ConfigurePageControl();
			ConfigureGotItButton();
		}
		#endregion

		#region Private methods
		private List<ContentViewController> CreateInformativePages()
		{
			var pages = new List<ContentViewController>();
			pages.Add(Instantiate(StoryboardConstants.StoryboardLogin, StoryboardConstants.InformativeLoginPage) as ContentViewController);
			pages.Add(Instantiate(StoryboardConstants.StoryboardLogin, StoryboardConstants.InformativeBuyPage) as ContentViewController);
			pages.Add(Instantiate(StoryboardConstants.StoryboardLogin, StoryboardConstants.InformativeCoinsPage) as ContentViewController);
			pages.Add(Instantiate(StoryboardConstants.StoryboardLogin, StoryboardConstants.InformativeFavoritesPage) as ContentViewController);
			return pages;
		}

		private UIViewController Instantiate(string storyboard, string viewcontroller) => UIStoryboard.FromName(storyboard, null).InstantiateViewController(viewcontroller);

		private void ConfigurePageViewController()
		{
			// Create UIPageViewController and configure it
			pageViewController = Instantiate(StoryboardConstants.StoryboardLogin, StoryboardConstants.PageViewController) as UIPageViewController;
			pageViewController.DataSource = new PageViewControllerDataSource();
			pageViewController.Delegate = new PageViewControllerDelegate();
			var viewControllers = new UIViewController[] { _pages.ElementAt(0) };
			pageViewController.SetViewControllers(viewControllers, UIPageViewControllerNavigationDirection.Forward, false, null);
			pageViewController.View.Frame = new CGRect(0, 0, this.View.Frame.Width, this.View.Frame.Size.Height);
			View.AddSubview(this.pageViewController.View);
		}

		private void ConfigurePageControl()
		{
			View.BringSubviewToFront(PageControl);
			PageControl.Pages = _pages.Count;
			PageControl.CurrentPage = 0;
			pageControl = PageControl;
		}

		private void ConfigureGotItButton()
		{
			View.BringSubviewToFront(GotItButton);
			GotItButton.Hidden = true;
			gotItButton = GotItButton;
			// Add event to button
			GotItButton.TouchUpInside += (sender, e) => { Presenter.ToLoginScreen(); };
		}
		#endregion

		#region IWelcomeView implementation
		public void ToLoginPage()
		{
			// navigate to login page
		}
		#endregion

		#region BaseViewController -> IBaseView implementation
		public override void SetUIAppearance()
		{
			base.SetUIAppearance();
		}

		public override void AttachEvents()
		{
			// ToLoginPage event
		}

		public override void DetachEvents()
		{
			// ToLoginPage event
		}
		#endregion

		#region UIPageViewControllerDataSource implementation
		private class PageViewControllerDataSource : UIPageViewControllerDataSource
		{
			public override UIViewController GetNextViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
			{
				currentIndex = _pages.IndexOf(referenceViewController as ContentViewController);
				return currentIndex == _pages.Count - 1 ? null : _pages[(currentIndex + 1) % _pages.Count];
			}

			public override UIViewController GetPreviousViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
			{
				currentIndex = _pages.IndexOf(referenceViewController as ContentViewController);
				return currentIndex == 0 ? null : _pages[(currentIndex - 1) % _pages.Count];
			}
		}
		#endregion

		#region PageViewControllerDelegate implementation
		private class PageViewControllerDelegate : UIPageViewControllerDelegate
		{
			public override void WillTransition(UIPageViewController pageViewController, UIViewController[] pendingViewControllers)
			{
				pendingIndex = _pages.IndexOf(pendingViewControllers.First() as ContentViewController);
			}

			public override void DidFinishAnimating(UIPageViewController pageViewController, bool finished, UIViewController[] previousViewControllers, bool completed)
			{
				if (completed)
					currentIndex = pendingIndex;
					pageControl.CurrentPage = currentIndex;

				if (currentIndex == _pages.Count - 1)
					gotItButton.Hidden = false;
				else 
					gotItButton.Hidden = true;
			}
		}
		#endregion
	}
}
