using System;
using System.Collections.Generic;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers.Main
{
	[Register("AllItemsViewController")]
	public partial class AllItemsViewController : UIViewController
	{
		#region Properties
		private List<Product> filteredItems;
		private List<Product> searchData;
		private UISearchController resultSearchController;
		#endregion

		#region Constructor
		public AllItemsViewController(IntPtr handle) : base(handle)
		{
		}
		#endregion

		#region Controller Life cycle
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			resultSearchController = new UISearchController();
			resultSearchController.SearchResultsUpdater = new AllItemsViewControllerSearch(this);
			resultSearchController.DimsBackgroundDuringPresentation = false;
			resultSearchController.SearchBar.SizeToFit();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
		}
		#endregion

		#region BaseViewController -> IBaseView implementation
		#endregion

		#region UITableViewSource implementation
		private class AllItemsViewControllerDataSource : UITableViewSource
		{
			private AllItemsViewController parent;

			public AllItemsViewControllerDataSource(AllItemsViewController parent)
			{
				this.parent = parent;
			}

			public override nint RowsInSection(UITableView tableview, nint section) => parent.resultSearchController.Active && parent.resultSearchController.SearchBar.Text != "" ? parent.searchData.Count : NumberOfRows();

			private int NumberOfRows()
			{
				if (parent.filteredItems == null || parent.filteredItems.Count != 0)
				{
					// hide segment controll
					parent.NoItemsLabel.Hidden = false;
					parent.NoItemsLabel.Text = "No items";
					return 0;
				}
				else {
					// unhide segment control
					parent.NoItemsLabel.Hidden = true;
					return parent.filteredItems.Count;
				}
			}

			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				return new UITableViewCell();
			}


		}

		#endregion

		#region UITableViewDelegate implementation
		private class AllItemsViewControllerDelegate : UITableViewDelegate
		{
			private AllItemsViewController parent;

			public AllItemsViewControllerDelegate(AllItemsViewController parent)
			{
				this.parent = parent;
			}

		}
		#endregion

		#region UISearchResultsUpdating implementation
		private class AllItemsViewControllerSearch : UISearchResultsUpdating
		{
			private AllItemsViewController parent;

			public AllItemsViewControllerSearch(AllItemsViewController parent)
			{
				this.parent = parent;
			}

			public override void UpdateSearchResultsForSearchController(UISearchController searchController)
			{
				
			}
		}
		#endregion
	}
}
