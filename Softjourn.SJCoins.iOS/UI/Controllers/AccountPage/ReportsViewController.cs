using System;
using System.Collections.Generic;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.TransactionReports;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.iOS.UI.Cells;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers.AccountPage
{
	[Register("ReportsViewController")]
	public partial class ReportsViewController : BaseViewController<TransactionReportPresenter>, ITransactionReportView
	{
		#region Constants
		#endregion

		#region Properties
		private ReportsSource _tableSource;
		#endregion

		#region Constructor
		public ReportsViewController(IntPtr handle) : base(handle)
		{
		}
		#endregion

		#region Controller Life cycle
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			ConfigurePage();
			ConfigureTableView();
			Presenter.OnStartLoadingPage();
		}
		#endregion

		#region IReportsView implementation
		public void ShowEmptyView()
		{
			NoItemsLabel.Hidden = false;
		}

		public void SetData(List<Transaction> transactionsList)
		{
			_tableSource.SetItems(transactionsList);
			TableView.ReloadData();
		}
		#endregion

		#region BaseViewController
		public override void AttachEvents()
		{
			base.AttachEvents();
			//SegmentControl.TouchUpInside += SegmentControl_SameButtonClicked;
			SegmentControl.ValueChanged += SegmentControl_AnotherButtonClicked;
		}

		public override void DetachEvents()
		{
			//SegmentControl.TouchUpInside -= SegmentControl_SameButtonClicked;
			SegmentControl.ValueChanged -= SegmentControl_AnotherButtonClicked;
			base.DetachEvents();
		}
		#endregion

		#region Private methods
		private void ConfigurePage()
		{
			NoItemsLabel.Hidden = true;
		}

		private void ConfigureTableView()
		{
			_tableSource = new ReportsSource();
			TableView.Source = _tableSource;
		}

		// -------------------- Event handlers --------------------
		// SegmentControl methods 
		public void SegmentControl_SameButtonClicked(object sender, EventArgs e)
		{

		}

		public void SegmentControl_AnotherButtonClicked(object sender, EventArgs e)
		{
			
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
	public class ReportsSource : UITableViewSource
	{
		private List<Transaction> items = new List<Transaction>();

		public event EventHandler TakeNexPage;

		public void SetItems(List<Transaction> items)
		{
			this.items = items;
		}

		public override nint RowsInSection(UITableView tableview, nint section) => items.Count;

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) => tableView.DequeueReusableCell(TransactionCell.Key, indexPath);

		public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
		{
			var _cell = (TransactionCell)cell;
			if (indexPath.Row > items.Count)
			{
				// trigg presenter give next page.
				TakeNexPage?.Invoke(this, null);
			}
			else
			{
				_cell.ConfigureWith(items[indexPath.Row]);
			}
		}
	}
	#endregion
}
