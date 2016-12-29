using System;
using UIKit;

namespace Softjourn.SJCoins.iOS
{
	public class InformativeDataSource :  UIPageViewControllerDataSource
	{
		//Properties
		public UIViewController[] _pages;

		//Constructor
		public InformativeDataSource(UIViewController[] pages)
		{
			_pages = pages;
		}

		//UIPageViewControllerDataSource
		public override UIViewController GetNextViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
		{
			UIViewController currentPage = referenceViewController as UIViewController;
			if (currentPage.Index == 0)
			{
				return pages[pages.Count - 1];
			}
			else
			{
				return pages[currentPage.Index - 1];
			}
		}

		public override UIViewController GetPreviousViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
		{
			
		}

		public UIViewController GetViewController(int page)
		{
			return new DetailDialog(page);
		}
	}
}
