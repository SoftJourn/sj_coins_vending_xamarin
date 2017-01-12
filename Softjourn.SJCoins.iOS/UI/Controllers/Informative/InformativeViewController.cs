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
		private List<string> _pageTitles = new List<string> { "How to Log in?", "Buy Products", "Want More Coins?", "Add Favorites" };
		private List<ContentViewController> _pages;

		//Constructor
		public InformativeViewController(IntPtr handle) : base(handle)
		{  
		}

		//Life cycle
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Create informative content
			var navigationManager = new NavigationManager();
			_pages = new List<ContentViewController>();
			_pages = navigationManager.CreateInformativePages(_pageTitles);

			// Create UIPageViewController and configure it
			pageViewController = navigationManager.Instantiate("Login", "PageViewController") as UIPageViewController;
			ConfigurePageViewController();
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
			var viewControllers = new UIViewController[] { _pages.ElementAt(0) };
			pageViewController.SetViewControllers(viewControllers, UIPageViewControllerNavigationDirection.Forward, false, null);
			pageViewController.View.Frame = new CGRect(0, 0, this.View.Frame.Width, this.View.Frame.Size.Height);
			View.AddSubview(this.pageViewController.View);
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
			private List<ContentViewController> _pages;

			public PageViewControllerDataSource(List<ContentViewController> pages)
			{
				_pages = pages;
			}

			public override UIViewController GetNextViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
			{
				var currentViewController = referenceViewController as ContentViewController;
				if (currentViewController.Index == _pages.Count - 1)
					return null;
				else {
					return _pages[(currentViewController.Index + 1) % _pages.Count];
				}
			}

			public override UIViewController GetPreviousViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
			{
				var currentViewController = referenceViewController as ContentViewController;
				if (currentViewController.Index == 0)
				{
					return null;
				}
				else {
					return _pages[currentViewController.Index - 1];
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
