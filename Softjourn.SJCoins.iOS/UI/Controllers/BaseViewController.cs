using System;
using Autofac;
using Softjourn.SJCoins.Core.UI.Bootstrapper;
using Softjourn.SJCoins.Core.UI.Presenters.IPresenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers
{
	public class BaseViewController<TPresenter> : UIViewController, IBaseView where TPresenter : class, IBasePresenter
	{
		#region Properties
		protected TPresenter Presenter { get; set; }
		protected AppDelegate currentApplication;
		protected ILifetimeScope _scope;
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
		}

		public virtual void AttachEvents()
		{
		}

		public virtual void DetachEvents()
		{
		}

		#endregion

		#region Controller Lifecycle
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			InitPresenter();
			Presenter.AttachView(this);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			//AttachEvents();
			Presenter.ViewShowed();
		}
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			//Set this view controller when visible
			currentApplication.VisibleViewController = this;
		}

		public override void ViewWillDisappear(bool animated)
		{
			//DetachEvents();
			Presenter.ViewHidden();
			base.ViewWillDisappear(animated);
		}

		public override void ViewDidUnload()
		{
			//Presenter.DetachView();
			_scope.Dispose();
			base.ViewDidUnload();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			Presenter = null;
			Console.WriteLine("Presenter is equal NULL");
		}
		#endregion

		#region Private methods
		private void InitPresenter()
		{
			_scope = BaseBootstrapper.Container.BeginLifetimeScope();

			Presenter = _scope.Resolve<TPresenter>();
			Presenter.AttachView(this);
		}

		//private void AttachPullToRefresh()
		//{

		//}
		#endregion
	}
}
