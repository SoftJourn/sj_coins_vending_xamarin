using System;
using Autofac;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers
{
	public class BaseViewController : UIViewController
	{
		protected AppDelegate currentApplication;
		protected ILifetimeScope _scope;

		public BaseViewController(IntPtr handle) : base(handle)
		{
			currentApplication = (AppDelegate)UIApplication.SharedApplication.Delegate;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			_scope = Bootstraper.Bootstraper.Container.BeginLifetimeScope();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
		}

		public override void ViewWillDisappear(bool animated)
		{
			//DetachEvents();
			//ViewPresenter.ViewHidden();
			base.ViewWillDisappear(animated);
		}

		public override void ViewDidUnload()
		{
			//ViewPresenter.DetachView();
			_scope.Dispose();

			base.ViewDidUnload();
		}

		private void AttachPullToRefresh()
		{

		}
	}
}
