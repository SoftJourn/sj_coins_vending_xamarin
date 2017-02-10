using System;
using System.Collections.Generic;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.iOS.UI.Controllers;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers.Main
{
	[Register("ShowViewController")]
	public partial class ShowViewController : BaseViewController<ShowAllPresenter>, IShowAllView
	{
		#region Properties
		public string CategoryName { get; set; }
		public List<Product> filteredItems;
		public List<Product> searchData;
		public UISearchController resultSearchController;
		#endregion

		#region Constructor
		public ShowViewController(IntPtr handle) : base(handle)
		{
		}
		#endregion

		#region Controller Life cycle
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Throw to presenter category name what needs to be displayed and take products.
			filteredItems = Presenter.GetProductList(CategoryName);

			//resultSearchController = new UISearchController();
			//resultSearchController.SearchResultsUpdater = new ShowViewController(this);
			//resultSearchController.DimsBackgroundDuringPresentation = false;
			//resultSearchController.SearchBar.SizeToFit();
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
	}

	#region UITableViewSource implementation
	public class ShowAllSource : UITableViewSource
	{
		private ShowViewController parent;

		public ShowAllSource(ShowViewController parent)
		{
			this.parent = parent;
		}

		public override nint RowsInSection(UITableView tableview, nint section) => parent.resultSearchController.Active && parent.resultSearchController.SearchBar.Text != "" ? parent.searchData.Count : NumberOfRows();

		private int NumberOfRows() => (parent.filteredItems == null || parent.filteredItems.Count != 0) ? 0 : parent.filteredItems.Count;

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			return new UITableViewCell();
		}
	}
	#endregion
}

	


	//}

	//#endregion

	//#region UITableViewDelegate implementation
	//private class AllItemsViewControllerDelegate : UITableViewDelegate
	//{
	//	private AllItemsViewController parent;

	//	public AllItemsViewControllerDelegate(AllItemsViewController parent)
	//	{
	//		this.parent = parent;
	//	}

	//}
	//#endregion

	//#region UISearchResultsUpdating implementation
	//private class AllItemsViewControllerSearch : UISearchResultsUpdating
	//{
	//	private AllItemsViewController parent;

	//	public AllItemsViewControllerSearch(AllItemsViewController parent)
	//	{
	//		this.parent = parent;
	//	}

	//	public override void UpdateSearchResultsForSearchController(UISearchController searchController)
	//	{

	//	}
	//}
	//#endregion
