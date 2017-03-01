using System;
using Foundation;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.iOS.UI.Cells;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers.AccountPage
{
	[Register("ReportsViewController")]
	public partial class ReportsViewController : BaseViewController<TransactionReportPresenter>
	{
		#region Constants
		//public const string Purchases = "Purchases";
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
			//Presenter.OnStartLoadingPage();
		}
		#endregion

		#region IReportsView implementation
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
		//private List<History> items = new List<History>();

		public void SetItems() //List<History> items)
		{
			//this.items = items;
		}

		public override nint RowsInSection(UITableView tableview, nint section) => 15;

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) => tableView.DequeueReusableCell(TransactionCell.Key, indexPath);

		public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
		{
			var _cell = (TransactionCell)cell;
			//var item = items[indexPath.Row];
			//_cell.ConfigureWith(item);
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			tableView.DeselectRow(indexPath, true);
		}
	}
	#endregion
}
