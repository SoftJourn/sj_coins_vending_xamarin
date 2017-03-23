using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using Foundation;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.iOS.General.Constants;
using Softjourn.SJCoins.iOS.UI.Services;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers.Informative
{
	[Register("InformativeViewController")]
	public partial class InformativeViewController : BaseViewController<WelcomePresenter>, IWelcomeView
	{
		#region Properties
		private UIPageViewController pageViewController;
		private List<ContentViewController> _pages;
		private int currentIndex = 0;
		private int pendingIndex;
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

			//Set configuration of internal view elements
			ConfigurePageViewController();
			ConfigurePageControl();
			ConfigureGotItButton();
			ConfigureFirstPage();
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
			pageViewController.DataSource = new PageViewControllerDataSource(this);
			pageViewController.Delegate = new PageViewControllerDelegate(this);
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
		}

		private void ConfigureGotItButton()
		{
			View.BringSubviewToFront(GotItButton);
			GotItButton.Hidden = true;

			// Add event to button
			GotItButton.TouchUpInside += (sender, e) => 
			{ 
				Presenter.ToLoginScreen();
				Presenter.DisableWelcomePageOnLaunch();
			};
		}

		public void ConfigureDinamicUIElements()
		{
			if (currentIndex == _pages.Count - 1)
			{
				ConfigureLastPage();
			}
			else {
				ConfigureFirstPage();
			}
		}

		private void ConfigureFirstPage()
		{
			// set background color as first page and hide button
			View.BackgroundColor = UIColor.FromRGB(246, 76, 115).ColorWithAlpha(1.0f);
			GotItButton.Hidden = true;
		}

		private void ConfigureLastPage()
		{
			// set background color as last page and show button
			View.BackgroundColor = UIColor.FromRGB(200, 115, 244).ColorWithAlpha(1.0f);
			GotItButton.Hidden = false;
		}
		#endregion

		#region IWelcomeView implementation
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
			private InformativeViewController parent;

			public PageViewControllerDataSource(InformativeViewController parent)
			{
				this.parent = parent;
			}

			public override UIViewController GetNextViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
			{
				parent.currentIndex = parent._pages.IndexOf(referenceViewController as ContentViewController);
				return parent.currentIndex == parent._pages.Count - 1 ? null : parent._pages[(parent.currentIndex + 1) % parent._pages.Count];
			}

			public override UIViewController GetPreviousViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
			{
				parent.currentIndex = parent._pages.IndexOf(referenceViewController as ContentViewController);
				return parent.currentIndex == 0 ? null : parent._pages[(parent.currentIndex - 1) % parent._pages.Count];
			}
		}
		#endregion

		#region UIPageViewControllerDelegate implementation
		private class PageViewControllerDelegate : UIPageViewControllerDelegate
		{
			private InformativeViewController parent;

			public PageViewControllerDelegate(InformativeViewController parent)
			{
				this.parent = parent;
			}

			public override void WillTransition(UIPageViewController pageViewController, UIViewController[] pendingViewControllers)
			{
				parent.pendingIndex = parent._pages.IndexOf(pendingViewControllers.First() as ContentViewController);
			}

			public override void DidFinishAnimating(UIPageViewController pageViewController, bool finished, UIViewController[] previousViewControllers, bool completed)
			{
				if (completed)
				{
					parent.currentIndex = parent.pendingIndex;
					parent.PageControl.CurrentPage = parent.currentIndex;

					parent.ConfigureDinamicUIElements();
				}
			}
		}
		#endregion
	}
}
