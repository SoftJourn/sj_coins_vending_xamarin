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
		//public List<Product> searchData;
		//public UISearchController resultSearchController;
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

			TableView.Source = new ShowAllSource(this);

			TableView.RegisterNibForCellReuse(ProductCell.Nib, ProductCell.Key);

			//resultSearchController = new UISearchController();
			//resultSearchController.SearchResultsUpdater = new SearchResultsUpdator(this);
			//resultSearchController.DimsBackgroundDuringPresentation = false;
			//resultSearchController.SearchBar.SizeToFit();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			Title = CategoryName;
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

		public override nint RowsInSection(UITableView tableview, nint section) //=> parent.resultSearchController.Active && parent.resultSearchController.SearchBar.Text != "" ? parent.searchData.Count : NumberOfRows();
		{
			return parent.filteredItems.Count; //parent.resultSearchController.Active && parent.resultSearchController.SearchBar.Text != "" ? parent.searchData.Count : NumberOfRows();
		}

		//private int NumberOfRows() => (parent.filteredItems == null || parent.filteredItems.Count != 0) ? 0 : parent.filteredItems.Count;

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			return (ProductCell)tableView.DequeueReusableCell(ProductCell.Key, indexPath);
		}

		public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
		{
			var _cell = (ProductCell)cell;
			var item = parent.filteredItems[indexPath.Row];

			_cell.ConfigureWith(item);
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			var item = parent.filteredItems[indexPath.Row];

		}
	}
	#endregion

	#region UISearchResultsUpdating implementation
	public class SearchResultsUpdator : UISearchResultsUpdating
	{
		//public event Action<string> UpdateSearchResults = delegate { };
		private ShowViewController parent;

		public SearchResultsUpdator(ShowViewController parent)
		{
			this.parent = parent;
		}

		public override void UpdateSearchResultsForSearchController(UISearchController searchController)
		{
			//this.UpdateSearchResults(searchController.SearchBar.Text);
		}
	}
	#endregion
}
