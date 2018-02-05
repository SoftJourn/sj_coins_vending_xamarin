using System;
using System.Collections.Generic;
using Foundation;
using Softjourn.SJCoins.Core.API.Model;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers.AccountPage
{
	[Register("PurchaseViewController")]
	public partial class PurchaseViewController : BaseViewController<PurchasePresenter>, IPurchaseView
	{
		#region Constants
		public const string Purchases = "Purchases";
		#endregion

		#region Properties
		#endregion

		#region Constructor
		public PurchaseViewController(IntPtr handle) : base(handle)
		{
		}
		#endregion

		#region Controller Life cycle
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
            ConfigurePage();
			Presenter.OnStartLoadingPage();
		}

        public override void ViewWillDisappear(bool animated)
        {
            //MakeNavigationBarTransparent();
            base.ViewWillDisappear(animated);
        }
		#endregion

		#region BaseViewController
		public override void ShowProgress(string message)
		{
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
		}

		public override void HideProgress()
		{
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
		}
		#endregion

		#region IAccountView implementation
		public void SetData(List<History> purchaseList)
		{
            TableView.Source = new PurchaseViewSource(purchaseList);
            TableView.ReloadData();
            ShowScreenAnimated(true);
		}

		public void ShowEmptyView()
		{
            ShowScreenAnimated(false);
		}
		#endregion

		#region Private methods
        private void ConfigurePage()
        {
            // Hide NoItems label
            Title = Purchases;
            NoItemsLabel.Hidden = true;
            NoItemsLabel.Alpha = 0.0f;
            TableView.Alpha = 0.0f;
        }
		#endregion

		// Throw TableView to parent
		protected override UIScrollView GetRefreshableScrollView() => TableView;

		protected override void PullToRefreshTriggered(object sender, EventArgs e)
		{
			StopRefreshing();
			Presenter.OnStartLoadingPage();
		}

        protected override void ShowAnimated(bool loadSuccess)
        {
            NoItemsLabel.Hidden = loadSuccess;
            NoItemsLabel.Alpha = !loadSuccess ? 1.0f : 0f;
            TableView.Alpha = 1.0f;
        }
	}
}
