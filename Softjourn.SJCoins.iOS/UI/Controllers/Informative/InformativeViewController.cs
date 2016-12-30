using System;
using UIKit;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;

namespace Softjourn.SJCoins.iOS.UI.Controllers.Informative
{
	public partial class InformativeViewController : UIViewController
	{
		//Properties
		private UIPageViewController pageViewController;
		private List<string> _images;
		private List<string> _pageTitles;
		private List<string> _pageDetails;

		private List<ContentViewController> _pages;

		//Constructor
		public InformativeViewController(IntPtr handle) : base (handle)
		{
		}

		//Life cycle
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Set pages titles
			_pageTitles = new List<string> { "How to Log in?", "Buy Products", "Want More Coins?", "Add Favorites" };

			// Create pages for datasource
			CreatePages();

			// Create UIPageViewController and show first page
			pageViewController = this.Storyboard.InstantiateViewController("InformativePageViewController") as UIPageViewController;
			pageViewController.DataSource = new PageViewControllerDataSource(this, _pages);

			var startViewController = _pages.ElementAt(0) as ContentViewController;
			var viewControllers = new UIViewController[] { startViewController };

			pageViewController.SetViewControllers([_pages.Elemen, UIPageViewControllerNavigationDirection.Forward, false, null);

			pageViewController.View.Frame = new CGRect(0, 0, this.View.Frame.Width, this.View.Frame.Size.Height - 50);
			AddChildViewController(this.pageViewController);
			View.AddSubview(this.pageViewController.View);
			pageViewController.DidMoveToParentViewController(this);

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
		private void CreatePages()
		{
			_pageTitles.ForEach(delegate (String name)
		{
			Console.WriteLine(name);

				//var viewController = this.Storyboard.InstantiateViewController("ContentViewController") as ContentViewController;
			//viewController.titleText = _pageTitles.ElementAt(index);
			//viewController.imageFile = _images.ElementAt(index);
			//viewController.pageIndex = index;
			//return viewController;

		});

		}

		//private void RestartTutorial(object sender, EventArgs e)
		//{
		//	var startVC = this.ViewControllerAtIndex(0) as ContentViewController;
		//	var viewControllers = new UIViewController[] { startVC };
		//	this.pageViewController.SetViewControllers(viewControllers, UIPageViewControllerNavigationDirection.Forward, false, null);
		//}



		//UIPageViewControllerDataSource
		private class PageViewControllerDataSource : UIPageViewControllerDataSource
		{
			private InformativeViewController _parentViewController;
			private List<ContentViewController> _pages;

			public PageViewControllerDataSource(UIViewController parentViewController, List<ContentViewController> pages)
			{
				_parentViewController = parentViewController as InformativeViewController;
				_pages = pages;
			}

			public override UIViewController GetNextViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
			{
				var viewController = referenceViewController as ContentViewController;
				var index = viewController.pageIndex;
				index ++;
				if (index == _pages.Count)
				{
					return null;
				}
				else {
					return _pages.ElementAt(index);
				}
			}

			public override UIViewController GetPreviousViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
			{
				var viewController = referenceViewController as ContentViewController;
				var index = viewController.pageIndex;
				if (index == 0)
				{
					return null;
				}
				else {
					index --;
					return _pages.ElementAt(index);
				}
			}

			public override nint GetPresentationCount(UIPageViewController pageViewController)
			{
				return _pages.Count;
			}

			public override nint GetPresentationIndex(UIPageViewController pageViewController)
			{
				return 0;
			}
		}
	}
}
