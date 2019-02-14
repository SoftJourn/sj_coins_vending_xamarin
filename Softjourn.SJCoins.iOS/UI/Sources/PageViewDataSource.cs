using System.Collections.Generic;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Sources
{
    public class PageViewDataSource : UIPageViewControllerDataSource
    {
        private readonly List<UIViewController> pages;
        private int currentIndex;

        public PageViewDataSource(List<UIViewController> pages)
        {
            this.pages = pages;
        }

        public override UIViewController GetNextViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
        {
            currentIndex = pages.IndexOf(referenceViewController);
            var controller = currentIndex == pages.Count - 1
                ? null
                : pages[(currentIndex + 1) % pages.Count];

            return controller;
        }

        public override UIViewController GetPreviousViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
        {
            currentIndex = pages.IndexOf(referenceViewController);
            var controller = currentIndex == 0
                ? null
                : pages[(currentIndex - 1) % pages.Count];

            return controller;
        }
    }
}
