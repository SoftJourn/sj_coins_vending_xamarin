using System;
using Foundation;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers.Informative
{
	[Register("InformativeViewController")]
	public class InformativeViewController : UIViewController
	{
		//Properties
		private UIPageViewController pageViewController;
		private List<string> _pageTitles;
		private List<ContentViewController> _pages;

		//Constructor
		public InformativeViewController(IntPtr handle) : base(handle)
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
			pageViewController.DataSource = new PageViewControllerDataSource(_pages);

			var viewControllers = new UIViewController[] { _pages.ElementAt(0) };
			pageViewController.SetViewControllers(viewControllers, UIPageViewControllerNavigationDirection.Forward, false, null);
			pageViewController.View.Frame = new CGRect(0, 0, this.View.Frame.Width, this.View.Frame.Size.Height - 50);
			View.AddSubview(this.pageViewController.View);
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
			//ContentViewController Content = this.Storyboard.InstantiateViewController("ContentViewController") as ContentViewController;
			for (int i = 0; i < 4; i++)
			{
				ContentViewController content = Instantiate("Main", "ContentViewController") as ContentViewController;
				content.Index = i;
				content.Title = _pageTitles.ElementAt(i);
				_pages.Add(content);
			}
		}

		private UIViewController Instantiate(string storyboard, string viewcontroller)
		{
			return UIStoryboard.FromName(storyboard, null).InstantiateViewController(viewcontroller);
		}

		//private void RestartTutorial(object sender, EventArgs e)
		//{
		//	var startVC = this.ViewControllerAtIndex(0) as ContentViewController;
		//	var viewControllers = new UIViewController[] { startVC };
		//	this.pageViewController.SetViewControllers(viewControllers, UIPageViewControllerNavigationDirection.Forward, false, null);
		//}


	}
}
