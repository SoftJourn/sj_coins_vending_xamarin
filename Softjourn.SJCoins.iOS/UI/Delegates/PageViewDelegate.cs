using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Delegates
{
	public class PageViewDelegate : UIPageViewControllerDelegate
	{
		public event EventHandler<int> CurrentIndexChanged;

		private List<UIViewController> pages;
		private int pendingIndex;
		private int currentIndex = 0;

		public PageViewDelegate(List<UIViewController> pages)
		{
			this.pages = pages;
		}

		public override void WillTransition(UIPageViewController pageViewController, UIViewController[] pendingViewControllers)
		{
			pendingIndex = pages.IndexOf(pendingViewControllers.First());
		}

		public override void DidFinishAnimating(UIPageViewController pageViewController, bool finished, UIViewController[] previousViewControllers, bool completed)
		{
			if (completed)
			{
				currentIndex = pendingIndex;
				CurrentIndexChanged?.Invoke(this, currentIndex);
			}
		}
	}
}
