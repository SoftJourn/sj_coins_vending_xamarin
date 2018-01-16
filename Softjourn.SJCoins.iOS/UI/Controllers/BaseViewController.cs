﻿using System;
using Autofac;
using Softjourn.SJCoins.Core.UI.Bootstrapper;
using Softjourn.SJCoins.Core.UI.Presenters.IPresenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.iOS.General.Constants;
using Softjourn.SJCoins.iOS.UI.Services;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers
{
	public class BaseViewController<TPresenter> : UIViewController, IBaseView where TPresenter : class, IBasePresenter
	{
		#region Properties
		protected TPresenter Presenter { get; set; }
		protected AppDelegate currentApplication;
		protected ILifetimeScope _scope;
		protected UIRefreshControl _refreshControl;
		#endregion

		#region Constructor
		public BaseViewController(IntPtr handle) : base(handle)
		{
			currentApplication = (AppDelegate)UIApplication.SharedApplication.Delegate;
		}
		#endregion

		#region IBaseView implementation

		public virtual void SetUIAppearance()
		{
			//AttachPullToRefresh();
		}

		public virtual void AttachEvents()
		{
			if (_refreshControl != null)
				_refreshControl.ValueChanged += PullToRefreshTriggered;
		}

		public virtual void DetachEvents()
		{
			if (_refreshControl != null)
				_refreshControl.ValueChanged -= PullToRefreshTriggered;
		}

		//IBaseView
		public virtual void ShowProgress(string message)
		{
			LoaderService.Show(message);
		}

		public virtual void HideProgress()
		{
			LoaderService.Hide();
		}
		#endregion

		#region Controller Lifecycle
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			InitPresenter();
			Presenter.AttachView(this);
			AttachPullToRefresh();
            System.Diagnostics.Debug.WriteLine(String.Format("{0} created", this.GetType()));
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			AttachEvents();
			Presenter.ViewShowed();
			currentApplication.VisibleViewController = this;
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			//Set this view controller when visible
		}

		public override void ViewWillDisappear(bool animated)
		{
			DetachEvents();
			Presenter.ViewHidden();
			base.ViewWillDisappear(animated);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			Presenter = null;
			GC.Collect(GC.MaxGeneration);
			System.Diagnostics.Debug.WriteLine(String.Format("{0} disposed", this.GetType()));
		}

        public void ShowScreenAnimated(bool loadSuccess)
        {
            UIView.Animate(Const.ShowScreenInfoDelay, 0, UIViewAnimationOptions.CurveLinear, () => { ShowAnimated(loadSuccess); }, null);
        }
		#endregion

		#region Methods for inheritanse
		protected virtual UIScrollView GetRefreshableScrollView()
		{
			// virtual method for taking UIScrollView (tableview, collectionview) from child of this class
			return null;
		}

		protected virtual void PullToRefreshTriggered(object sender, EventArgs e)
		{
			
		}

        protected virtual void ShowAnimated(bool loadSuccess)
        {
            // virtual method for animated showing elements in child controllers
        }
		#endregion

		#region Public methods
		public void StopRefreshing()
		{
			if (_refreshControl != null && _refreshControl.Refreshing)
			{
				Invoke(() => _refreshControl.EndRefreshing(), 1); //IOS 10 fix 
			}
		}

        public void StyleNavigationBar()
        {
            if (NavigationController != null) {
                NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes 
                    {
                        ForegroundColor = UIColorConstants.ProductNameColor,
                        Font = UIFont.SystemFontOfSize(18, UIFontWeight.Medium)
                    };
            }
        }
		#endregion

		#region Private methods
		private void InitPresenter()
		{
			_scope = BaseBootstrapper.Container.BeginLifetimeScope();

			Presenter = _scope.Resolve<TPresenter>();
			Presenter.AttachView(this);
		}

		private void AttachPullToRefresh()
		{
			var refreshableScrollView = GetRefreshableScrollView();
			if (refreshableScrollView != null)
			{
				_refreshControl = new UIRefreshControl();
                _refreshControl.TintColor = UIColorConstants.ProductNameColor.ColorWithAlpha(0.7f);
				_refreshControl.TintColorDidChange();
				refreshableScrollView.AddSubview(_refreshControl);
			}
		}
		#endregion
	}
}
