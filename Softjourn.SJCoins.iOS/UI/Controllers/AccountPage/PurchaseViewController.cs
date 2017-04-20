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
		private PurchaseSource _tableSource;
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
			// Configure table view with source and events
			ConfigureTableView();
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
			_tableSource.SetItems(purchaseList);
			TableView.ReloadData();
		}

		public void ShowEmptyView()
		{
			NoItemsLabel.Hidden = false;
		}
		#endregion

		#region Private methods
		private void ConfigureTableView()
		{
			_tableSource = new PurchaseSource(); 
			TableView.Source = _tableSource;
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

	#region UITableViewSource implementation
	public class PurchaseSource : UITableViewSource
	{
		private List<History> items = new List<History>();

		public void SetItems(List<History> items)
		{
			this.items = items ?? new List<History>();
		}

		public override nint RowsInSection(UITableView tableview, nint section) => items.Count;

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) => tableView.DequeueReusableCell(PurchaseCell.Key, indexPath);

		public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
		{
			var _cell = (PurchaseCell)cell;
			var item = items[indexPath.Row];
			_cell.ConfigureWith(item);
		}
	}
	#endregion
}
