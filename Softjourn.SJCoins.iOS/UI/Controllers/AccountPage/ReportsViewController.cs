using System;
using System.Collections.Generic;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.TransactionReports;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.iOS.UI.Cells;
using UIKit;
using CoreGraphics;

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
			TableView.TableFooterView.Hidden = false;
		}

		public void AddItemsToExistedList(List<Transaction> transactionsList)
		{
			_tableSource.AddItems(transactionsList, TableView);
		}

		public void SetCompoundDrawableInput(bool? isAsc)
		{
			//throw new NotImplementedException();
		}

		public void SetCompoundDrawableOutput(bool? isAsc)
		{
			//throw new NotImplementedException();
		}
		#endregion

		#region BaseViewController
		public override void AttachEvents()
		{
			base.AttachEvents();
			SegmentControl.TouchUpInside += SegmentControl_SameButtonClicked;
			SegmentControl.ValueChanged += SegmentControl_AnotherButtonClicked;
			_tableSource.GetNexPage += TableSource_GetNextPageExecuted;
		}

		public override void DetachEvents()
		{
			SegmentControl.TouchUpInside -= SegmentControl_SameButtonClicked;
			SegmentControl.ValueChanged -= SegmentControl_AnotherButtonClicked;
			_tableSource.GetNexPage -= TableSource_GetNextPageExecuted;
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

		private void SortItems()
		{
			switch (SegmentControl.SelectedSegment)
			{
				case 0:
					Presenter.OnInputClicked();
					break;
				case 1:
					Presenter.OnOutputClicked();
					break;
				default:
					break;
			}
		}

		// -------------------- Event handlers --------------------
		// SegmentControl methods 
		public void SegmentControl_SameButtonClicked(object sender, EventArgs e)
		{
			SortItems();
		}

		public void SegmentControl_AnotherButtonClicked(object sender, EventArgs e)
		{
			SortItems();
		}

		// TableSource methods 
		public void TableSource_GetNextPageExecuted(object sender, EventArgs e)
		{
			Presenter.GetNextPage();
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
		private const int tableSection = 0;
		private const int rowBeforeEnd = 5;
		private const int numberOfItemsOnOnePage = 50;

		private List<Transaction> _items = new List<Transaction>();

		public event EventHandler GetNexPage;

		public void SetItems(List<Transaction> items)
		{
			_items = items;
		}

		public void AddItems(List<Transaction> items, UITableView tableView)
		{
			// Add new items to existing list
			_items.AddRange(items);

			// Create empty array
			var indexPaths = new List<NSIndexPath>();

			// Add elements to array
			foreach (Transaction item in items)
			{
				if (_items.Contains(item))
				{
					var index = _items.IndexOf(item);
					var indexPath = NSIndexPath.FromRowSection(index, tableSection);
					indexPaths.Add(indexPath);
				}
			}

			// Insert into table
			tableView.InsertRows(atIndexPaths: indexPaths.ToArray(), withRowAnimation: UITableViewRowAnimation.Fade);
		}

		public override nint RowsInSection(UITableView tableview, nint section) => _items.Count;

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) => tableView.DequeueReusableCell(TransactionCell.Key, indexPath);

		public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
		{
			var _cell = (TransactionCell)cell;
			_cell.ConfigureWith(_items[indexPath.Row]);

			if (indexPath.Row == _items.Count - rowBeforeEnd && _items.Count >= numberOfItemsOnOnePage)
			{
				// trigg presenter to give the next page.
				GetNexPage?.Invoke(this, null);
			}
		}
	}
	#endregion
}
