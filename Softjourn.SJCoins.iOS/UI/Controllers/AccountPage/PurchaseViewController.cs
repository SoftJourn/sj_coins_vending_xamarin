using System;
using System.Collections.Generic;
using Foundation;
using Softjourn.SJCoins.Core.Models;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.iOS.UI.Sources.AccountPage;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers.AccountPage
{
	[Register("PurchaseViewController")]
	public partial class PurchaseViewController : BaseViewController<PurchasePresenter>, IPurchaseView
	{
		public const string Purchases = "Purchases";

		public PurchaseViewController(IntPtr handle) : base(handle)
		{
		}

		#region Controller Life cycle

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
            ConfigurePage();
			Presenter.OnStartLoadingPage();
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
