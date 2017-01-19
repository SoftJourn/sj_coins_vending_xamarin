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
		private List<string> _pageTitles;
		private List<string> _pageImages;
		private List<int> _r;
		private List<int> _g;
		private List<int> _b;

		private List<ContentViewController> _pages;
		private static int index = 0;
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
			_pageTitles = new List<string> { "How to Log in?", "Buy Products", "Want More Coins?", "Add Favorites" };
			_pageImages = new List<string> { "InfoLoginLogo", "InfoBuyLogo", "InfoCoinsLogo", "InfoFavoritesLogo" };
			_r = new List<int> { 246, 33, 51, 200 };
			_g = new List<int> { 76, 193, 149, 115 };
			_b = new List<int> { 115, 173, 255, 244 };

			_pages = new List<ContentViewController>();
			_pages = CreateInformativePages(_pageTitles);

			// Create UIPageViewController and configure it
			pageViewController = Instantiate(StoryboardConstants.StoryboardLogin, StoryboardConstants.PageViewController) as UIPageViewController;

			ConfigurePageViewController();
			ConfigurePageControl();
			ConfigureGotItButton();
		}
		#endregion

		#region Private methods
		//-----------------------------------> Helpers
		private List<ContentViewController> CreateInformativePages(List<string> titles)
		{
			var pages = new List<ContentViewController>();
			for (int i = 0; i < 4; i++)
			{
				ContentViewController content = Instantiate(StoryboardConstants.StoryboardLogin, StoryboardConstants.ContentViewController) as ContentViewController;
				content.Index = i;
				content.Title = titles[i];
				content.View.BackgroundColor = UIColor.FromRGB(_r[i], _g[i], _b[i]);
				//content.LogoString = _pageImages[i];
				pages.Add(content);
			}
			return pages;
		}

		private UIViewController Instantiate(string storyboard, string viewcontroller) => UIStoryboard.FromName(storyboard, null).InstantiateViewController(viewcontroller);
		//-----------------------------------> Helpers

		private void ConfigurePageViewController()
		{
			pageViewController.DataSource = new PageViewControllerDataSource(_pages);
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

			GotItButton.TouchUpInside += (sender, e) => {
				Presenter.ToLoginScreen();
			};
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
			private List<ContentViewController> _pages;

			public PageViewControllerDataSource(List<ContentViewController> pages)
			{
				_pages = pages;
			}

			public override UIViewController GetNextViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
			{
				var currentViewController = referenceViewController as ContentViewController;
				index = currentViewController.Index;
				index++;

				if (index == _pages.Count)
				{
					gotItButton.Hidden = false;
					return null;
				} 
				else {
					gotItButton.Hidden = true;
					return _pages[index];
				}
			}

			public override UIViewController GetPreviousViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
			{
				var currentViewController = referenceViewController as ContentViewController;
				index = currentViewController.Index;

				if (currentViewController.Index == 0)
				{
					return null;
				}
				else {
					index--;
					return _pages[index];
				}
			}
		}
		#endregion

		#region PageViewControllerDelegate implementation
		private class PageViewControllerDelegate : UIPageViewControllerDelegate
		{
			public override void DidFinishAnimating(UIPageViewController pageViewController, bool finished, UIViewController[] previousViewControllers, bool completed)
			{
				if (completed)
				{
					pageControl.CurrentPage = index;
					Console.WriteLine("index: {0}", index);

				}
			}
		}
		#endregion
	}
}
