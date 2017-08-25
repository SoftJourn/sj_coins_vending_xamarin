using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using Foundation;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.iOS.General.Constants;
using Softjourn.SJCoins.iOS.UI.DataSources;
using Softjourn.SJCoins.iOS.UI.Delegates;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers.Informative
{
	[Register("InformativeViewController")]
	public partial class InformativeViewController : BaseViewController<WelcomePresenter>, IWelcomeView
	{
		#region Properties
		private UIPageViewController pageViewController;
		private List<UIViewController> pages;
		private PageViewDataSource pageDataSource;
		private PageViewDelegate pageDelegate;
		private int currentIndex = 0;
        private InformativeFavoritesPage favoritePage;
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
			//Set configuration of internal view elements
			ConfigurePageViewController();
			ConfigurePageControl();
			ConfigureFirstPage();
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
			pageViewController = null;
			pages = null;
			pageDataSource = null;
			pageDelegate = null;
			favoritePage = null;
            PageControl = null;
		}
		#endregion

		#region BaseViewController -> IBaseView implementation
		public override void AttachEvents()
		{
            // ToLoginPage event
            favoritePage.GotItButtonTapped += GotItButtonClickHandler;
			pageDelegate.CurrentIndexChanged += PageChangeHandler;
		}

		public override void DetachEvents()
		{
			// ToLoginPage event
			favoritePage.GotItButtonTapped -= GotItButtonClickHandler;
			pageDelegate.CurrentIndexChanged -= PageChangeHandler;
		}
		#endregion

		#region IWelcomeView implementation
		#endregion

		#region Private methods
		private List<UIViewController> CreateInformativePages()
		{
			var _pages = new List<UIViewController>();
			_pages.Add(Instantiate(StoryboardConstants.StoryboardLogin, StoryboardConstants.InformativeLoginPage));
			_pages.Add(Instantiate(StoryboardConstants.StoryboardLogin, StoryboardConstants.InformativeBuyPage));
			_pages.Add(Instantiate(StoryboardConstants.StoryboardLogin, StoryboardConstants.InformativeCoinsPage));
            favoritePage = Instantiate(StoryboardConstants.StoryboardLogin, StoryboardConstants.InformativeFavoritesPage) as InformativeFavoritesPage;
			_pages.Add(favoritePage);
			return _pages;
		}

		private UIViewController Instantiate(string storyboard, string viewcontroller) => UIStoryboard.FromName(storyboard, null).InstantiateViewController(viewcontroller);

		private void ConfigurePageViewController()
		{
			// Create UIPageViewController and configure it
			pageViewController = Instantiate(StoryboardConstants.StoryboardLogin, StoryboardConstants.PageViewController) as UIPageViewController;
			pages = CreateInformativePages();
			pageDataSource = new PageViewDataSource(pages);
			pageViewController.DataSource = pageDataSource;
			pageDelegate = new PageViewDelegate(pages);
			pageViewController.Delegate = pageDelegate;
			var viewControllers = new UIViewController[] { pages.ElementAt(0) };
			pageViewController.SetViewControllers(viewControllers, UIPageViewControllerNavigationDirection.Forward, false, null);
			pageViewController.View.Frame = new CGRect(0, 0, this.View.Frame.Width, this.View.Frame.Size.Height);
			View.AddSubview(this.pageViewController.View);
		}

        private void ConfigurePageControl()
        {
            View.BringSubviewToFront(PageControl);
            PageControl.Pages = pages.Count;
            PageControl.CurrentPage = 0;
        }

		private void ConfigureFirstPage()
		{
			// set background color as first page and hide button
			View.BackgroundColor = UIColor.FromRGB(246, 76, 115).ColorWithAlpha(1.0f);
		}

		private void ConfigureLastPage()
		{
			// set background color as last page and show button
			View.BackgroundColor = UIColor.FromRGB(200, 115, 244).ColorWithAlpha(1.0f);
		}

		#region Event handlers
		private void GotItButtonClickHandler(object sender, EventArgs e)
		{
			Presenter.ToLoginScreen();
			Presenter.DisableWelcomePageOnLaunch();
		}

		private void PageChangeHandler(object sender, int index)
		{
			currentIndex = index;
			PageControl.CurrentPage = currentIndex;

			if (currentIndex == pages.Count - 1)
				ConfigureLastPage();
			else
				ConfigureFirstPage();
		}
		#endregion Event handlers
		#endregion Private methods
	}
}
