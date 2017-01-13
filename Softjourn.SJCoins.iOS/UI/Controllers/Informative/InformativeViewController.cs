using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers.Informative
{
	[Register("InformativeViewController")]
	public partial class InformativeViewController : UIViewController
	{
		//Properties
		private UIPageViewController pageViewController;
		private List<string> _pageTitles;
		private List<ContentViewController> _pages;
		private static int index = 0;
		private static UIPageControl pageControl;

		//Constructor
		public InformativeViewController(IntPtr handle) : base(handle)
		{  
		}

		//Life cycle
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Create informative content
			_pageTitles = new List<string> { "How to Log in?", "Buy Products", "Want More Coins?", "Add Favorites" };
			var navigationManager = new NavigationManager();
			_pages = new List<ContentViewController>();
			_pages = navigationManager.CreateInformativePages(_pageTitles);

			// Create UIPageViewController and configure it
			pageViewController = navigationManager.Instantiate("Login", "PageViewController") as UIPageViewController;

			ConfigurePageViewController();
			ConfigurePageControl();
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			Console.WriteLine("InformativeVC disposed");
		}

		// Methods
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

		//UIPageViewControllerDataSource
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
				//Console.WriteLine("index: {0}", index);
				index++;

				if (index == _pages.Count)
					return null;
				else {
					return _pages[index];
				}
			}

			public override UIViewController GetPreviousViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
			{
				var currentViewController = referenceViewController as ContentViewController;
				index = currentViewController.Index;
				//Console.WriteLine("index: {0}", index);

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

		//UIPageViewControllerDataSource
		private class PageViewControllerDelegate : UIPageViewControllerDelegate
		{
			public PageViewControllerDelegate()
			{
			}

			public override void DidFinishAnimating(UIPageViewController pageViewController, bool finished, UIViewController[] previousViewControllers, bool completed)
			{
				if (completed)
				{
					pageControl.CurrentPage = index;
					Console.WriteLine("index: {0}", index);
				}
			}
		}
	}
}
