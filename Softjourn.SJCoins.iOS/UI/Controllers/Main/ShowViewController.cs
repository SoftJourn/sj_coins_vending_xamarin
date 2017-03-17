using System;
using System.Collections.Generic;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.iOS.General.Constants;
using Softjourn.SJCoins.iOS.UI.Cells;
using UIKit;
     
namespace Softjourn.SJCoins.iOS.UI.Controllers.Main
{
	[Register("ShowViewController")]
	public partial class ShowViewController : BaseViewController<ShowAllPresenter>, IShowAllView, IUISearchControllerDelegate
	{
		#region Properties
		private string categoryName { get; set; }

		private ShowAllSource _tableSource; 
		private NSIndexPath _favoriteCellIndex;
		private UISearchController searchController;
		private SearchResultsUpdator searchResultsUpdator;
		private List<Product> searchData;

		public List<Product> filteredItems;
		#endregion

		#region Constructor
		public ShowViewController(IntPtr handle) : base(handle)
		{
		}

		public void SetInitialParameter(object categoryName)
		{
			if (categoryName is string)
			{
				this.categoryName = (string)categoryName;
			}
		}
		#endregion

		#region Controller Life cycle
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			ConfigureSearch();
			ConfigureTableView();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			Title = categoryName;

			// Throw to presenter category name what needs to be displayed and take products.
			filteredItems = Presenter.GetProductList(categoryName);
			_tableSource.SetItems(filteredItems);
			TableView.ReloadData();

			SegmentControl.Alpha = 1.0f;
		}

		[Export("willDismissSearchController:")]
		public void WillDismissSearchController(UISearchController searchController)
		{
			_tableSource.SetItems(filteredItems);
			TableView.ReloadData();
		}
		#endregion

		#region BaseViewController
		public override void AttachEvents()
		{
			base.AttachEvents();
			SegmentControl.TouchUpInside += SameButtonClickHandler;
			SegmentControl.ValueChanged += AnotherButtonClickHandler;
			_tableSource.ItemSelected += TableSource_ItemSelected;
			_tableSource.FavoriteClicked += TableSource_FavoriteClicked;
			SearchButton.Clicked += SearchButtonClickHandler;
			searchResultsUpdator.UpdateSearchResults += SearchResultsUpdator_Search;
		}

		public override void DetachEvents()
		{
			SegmentControl.TouchUpInside -= SameButtonClickHandler;
			SegmentControl.ValueChanged -= AnotherButtonClickHandler;
			_tableSource.ItemSelected -= TableSource_ItemSelected;
			_tableSource.FavoriteClicked -= TableSource_FavoriteClicked;
			SearchButton.Clicked -= SearchButtonClickHandler;
			searchResultsUpdator.UpdateSearchResults -= SearchResultsUpdator_Search;
			base.DetachEvents();
		}
		#endregion

		#region IShowAllView implementation
		public void FavoriteChanged(bool isFavorite)
		{
			// table reload row at index
			if (_favoriteCellIndex != null)
			{
				var index = new NSIndexPath[] { _favoriteCellIndex };
				if (categoryName == Const.FavoritesCategory)
				{
					// Set new items to table source
					var newItems = Presenter.GetProductList(categoryName);
					_tableSource.SetItems(newItems);
					// Delete row
					TableView.DeleteRows(atIndexPaths: index, withRowAnimation: UITableViewRowAnimation.Fade);
				}
				else
				{
					TableView.ReloadRows(atIndexPaths: index, withRowAnimation: UITableViewRowAnimation.Fade);
				}
			}
		}

		public void ShowSortedList(List<Product> products)
		{
			_tableSource.SetItems(products);
			TableView.ReloadData();
		}

		public void SetCompoundDrawableName(bool? isAsc)
		{
			//throw new NotImplementedException();
		}

		public void SetCompoundDrawablePrice(bool? isAsc)
		{
			//throw new NotImplementedException();
		}

		public void LastUnavailableFavoriteRemoved()
		{
			
		}
		#endregion

		#region Private methods
		private void ConfigureSearch()
		{
			searchData = new List<Product>();
			searchResultsUpdator = new SearchResultsUpdator();
			searchController = new UISearchController(searchResultsController: null)
			{
				WeakDelegate = this,
				DimsBackgroundDuringPresentation = false,
				WeakSearchResultsUpdater = searchResultsUpdator					
			};
		}

		private void ConfigureTableView()
		{
			_tableSource = new ShowAllSource();

			TableView.Source = _tableSource;
			TableView.RegisterNibForCellReuse(ProductCell.Nib, ProductCell.Key);
		}

		private void Search(string searchString)
		{
			var search = searchString.Trim();
			searchData.Clear();

			if (searchController.SearchBar.Text != "")
			{
				searchData = filteredItems.FindAll(item => item.Name.ToLower().Contains(search.ToLower()));
				_tableSource.SetItems(searchData);
			}
			else
			{
				_tableSource.SetItems(filteredItems);
			}

			if (searchController.Active)
				UIView.Animate(0.5, 0, UIViewAnimationOptions.CurveLinear, () => { SegmentControl.Alpha = 0.0f; }, null);
			else
				UIView.Animate(0.5, 0, UIViewAnimationOptions.CurveLinear, () => { SegmentControl.Alpha = 1.0f; }, null);

			TableView.ReloadData();
		}

		// -------------------- Event handlers --------------------
		private void TableSource_ItemSelected(object sender, Product product)
		{
			// Trigg presenter that user click on some product to see details
			Presenter.OnProductDetailsClick(product.Id);
			if (searchController.Active)
				searchController.DismissViewController(true, null);
		}

		public void TableSource_FavoriteClicked(object sender, ProductCell cell)
		{
			// Trigg presenter that user click on some product for adding it to favorite
			_favoriteCellIndex = TableView.IndexPathForCell(cell);
			Presenter.OnFavoriteClick(cell.Product);
		}

		private void SearchButtonClickHandler(object sender, EventArgs e)
		{
			// Handle clicking on the Search button
			PresentViewController(searchController, true, completionHandler: null);
		}

		// SegmentControl methods 
		private void SameButtonClickHandler(object sender, EventArgs e)
		{
			// Handle clicking on the same button of segment control
			SortItems(categoryName);
		}

		private void AnotherButtonClickHandler(object sender, EventArgs e)
		{
			// Handle clicking on the another button of segment control
			SortItems(categoryName);
		}

		private void SortItems(string category)
		{
			switch (SegmentControl.SelectedSegment)
			{
				case 0: // Name button
					Presenter.OnSortByNameClicked(category);
					break;
				case 1:	// Price button
					Presenter.OnSortByPriceClicked(category);
					break;
				default:
					break;
			}
		}

		private void SearchResultsUpdator_Search(object sender, string searchString)
		{
			Search(searchString);
		}
		// -------------------------------------------------------- 
		#endregion

		// Throw TableView to parent
		protected override UIScrollView GetRefreshableScrollView() => TableView;

		protected override void PullToRefreshTriggered(object sender, System.EventArgs e)
		{
			StopRefreshing();
			Presenter.GetProductList(categoryName);
		}
	}

	#region UITableViewSource implementation
	public class ShowAllSource : UITableViewSource
	{
		private List<Product> items = new List<Product>();

		public event EventHandler<Product> ItemSelected;
		public event EventHandler<ProductCell> FavoriteClicked;

		public void SetItems(List<Product> items)
		{
			this.items = items;
		}

		public override nint RowsInSection(UITableView tableview, nint section) => items.Count;

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) => tableView.DequeueReusableCell(ProductCell.Key, indexPath);

		public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
		{
			var _cell = (ProductCell)cell;
			var item = items[indexPath.Row];

			_cell.FavoriteClicked -= FavoriteClicked;
			_cell.FavoriteClicked += FavoriteClicked;

			_cell.ConfigureWith(item);
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			tableView.DeselectRow(indexPath, true);
			var item = items[indexPath.Row];
			ItemSelected?.Invoke(this, item);
		}
	}
	#endregion

	#region UISearchResultsUpdating implementation
	public class SearchResultsUpdator : UISearchResultsUpdating
	{
		public event EventHandler<string> UpdateSearchResults;

		public override void UpdateSearchResultsForSearchController(UISearchController searchController)
		{
			UpdateSearchResults?.Invoke(this, searchController.SearchBar.Text);
		}
	}
	#endregion
}
