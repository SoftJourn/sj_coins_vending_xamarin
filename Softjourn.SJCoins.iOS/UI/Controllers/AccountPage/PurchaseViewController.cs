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
			// Hide NoItems label
			NoItemsLabel.Hidden = true;
			// SetTitle;
			Title = Purchases;
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
            ReloadTable();
		}

		public void ShowEmptyView()
		{
			NoItemsLabel.Hidden = false;
		}
		#endregion

		#region Private methods
		private void ReloadTable()
		{
			TableView.ReloadSections(new NSIndexSet(0), UITableViewRowAnimation.Fade);
		}
		#endregion

		// Throw TableView to parent
		protected override UIScrollView GetRefreshableScrollView() => TableView;

		protected override void PullToRefreshTriggered(object sender, System.EventArgs e)
		{
			StopRefreshing();
			Presenter.OnStartLoadingPage();
		}
	}
}
