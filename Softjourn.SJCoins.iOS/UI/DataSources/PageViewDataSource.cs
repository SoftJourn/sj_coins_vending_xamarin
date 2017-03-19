using System;
using System.Collections.Generic;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.DataSources
{
	public class PageViewDataSource: UIPageViewControllerDataSource
	{
		public event EventHandler<int> CurrentIndexChanged;

		private List<UIViewController> pages;
		private int currentIndex = 0;

		public PageViewDataSource(List<UIViewController> pages)
		{
			this.pages = pages;
		}

		public override UIViewController GetNextViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
		{
			currentIndex = pages.IndexOf(referenceViewController);
			var controller = currentIndex == pages.Count - 1 ? null : pages[(currentIndex + 1) % pages.Count];
			CurrentIndexChanged?.Invoke(this, currentIndex);
			return controller;
		}

		public override UIViewController GetPreviousViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
		{
			currentIndex = pages.IndexOf(referenceViewController);
			var controller = currentIndex == 0 ? null : pages[(currentIndex - 1) % pages.Count];
			CurrentIndexChanged?.Invoke(this, currentIndex);
			return controller;		
		}
	}
}
