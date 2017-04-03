using System;
using System.Collections.Generic;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.iOS.General.Constants;
using Softjourn.SJCoins.iOS.General.Helper;
using Softjourn.SJCoins.iOS.UI.Cells;
using Softjourn.SJCoins.iOS.UI.Services;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers.Main
{
	[Register("ShowViewController")]
	public partial class ShowViewController : BaseViewController<ShowAllPresenter>, IShowAllView, IUISearchControllerDelegate
	{
		#region Constants
		private const string NameTitle = "Name";
		private const string PriceTitle = "Price";
		private const int NameSegment = 0;
		private const int PriceSegment = 1;
		private const int tableSection = 0;
		#endregion

		#region Properties
		private string categoryName { get; set; }

		private ShowAllSource _tableSource;
		private UISearchController searchController;
		private SearchResultsUpdator searchResultsUpdator;
		private SegmentControlHelper _segmentControlHelper;
		private List<Product> searchData;
		private List<Product> filteredItems;
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
			ConfigureSegmentControl();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			Title = categoryName;
			// Throw to presenter category name what needs to be displayed and take products.
			filteredItems = Presenter.GetProductList(categoryName);
			_tableSource.SetItems(filteredItems);
			TableView.ReloadData();

			NamePriceSegmentControl.Alpha = 1.0f;
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
			NamePriceSegmentControl.TouchUpInside += SameButtonClickHandler;
			NamePriceSegmentControl.ValueChanged += AnotherButtonClickHandler;

			_tableSource.ShowAllSource_ItemSelected += TableSource_ItemSelected;
			_tableSource.ShowAllSource_FavoriteClicked += TableSource_FavoriteClicked;

			SearchButton.Clicked += SearchButtonClickHandler;

			searchResultsUpdator.UpdateSearchResults += SearchResultsUpdator_Search;
		}

		public override void DetachEvents()
		{
			NamePriceSegmentControl.TouchUpInside -= SameButtonClickHandler;
			NamePriceSegmentControl.ValueChanged -= AnotherButtonClickHandler;

			_tableSource.ShowAllSource_ItemSelected -= TableSource_ItemSelected;
			_tableSource.ShowAllSource_FavoriteClicked -= TableSource_FavoriteClicked;

			SearchButton.Clicked -= SearchButtonClickHandler;

			searchResultsUpdator.UpdateSearchResults -= SearchResultsUpdator_Search;
			base.DetachEvents();
		}
		#endregion

		#region IShowAllView implementation
		public void FavoriteChanged(Product product)
		{
			ChangeFavorite(product);
		}

		public void LastUnavailableFavoriteRemoved(Product product)
		{
			ChangeFavorite(product);
		}

		public void ShowSortedList(List<Product> products)
		{
			_tableSource.SetItems(products);
			TableView.ReloadData();
		}

		public void SetCompoundDrawableName(bool? isAsc)
		{
			SetCompoundDrawableSegment(isAsc, NameTitle, NameSegment);
		}

		public void SetCompoundDrawablePrice(bool? isAsc)
		{
			SetCompoundDrawableSegment(isAsc, PriceTitle, PriceSegment);
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

			if (categoryName == Const.FavoritesCategory)
			{
				NavigationItem.RightBarButtonItem = null;
			}
		}

		private void ConfigureTableView()
		{
			_tableSource = new ShowAllSource();

			TableView.Source = _tableSource;
			TableView.RegisterNibForCellReuse(ProductCell.Nib, ProductCell.Key);
		}

		private void ConfigureSegmentControl()
		{
			_segmentControlHelper = new SegmentControlHelper();
			// Configure 0 segment
			ConfigureSegment(NameTitle, NameSegment, ImageConstants.ArrowUpward);
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
				UIView.Animate(0.5, 0, UIViewAnimationOptions.CurveLinear, () => { NamePriceSegmentControl.Alpha = 0.0f; }, null);
			else
				UIView.Animate(0.5, 0, UIViewAnimationOptions.CurveLinear, () => { NamePriceSegmentControl.Alpha = 1.0f; }, null);

			TableView.ReloadData();
		}

		private void SetCompoundDrawableSegment(bool? isAsc, string title, int segment)
		{
			if (isAsc == true)
				ConfigureSegment(title, segment, ImageConstants.ArrowDownward);
			else if (isAsc == false)
				ConfigureSegment(title, segment, ImageConstants.ArrowUpward);
			else
				ConfigureSegment(title, segment, null);
		}

		private void ConfigureSegment(string title, int segment, string imageName = null)
		{
			// Configure segment depending on whether the picture is present or not 
			if (imageName == null)
			{
				NamePriceSegmentControl.SetTitle(title, segment);
			}
			else
			{
				var inputImage = UIImage.FromBundle(imageName);
				var mergedImage = _segmentControlHelper.ImageFromImageAndText(inputImage, title, UIColor.Black);
				NamePriceSegmentControl.SetImage(mergedImage, segment);
			}
		}

		private void ChangeFavorite(Product product)
		{
			var indexPaths = new List<NSIndexPath>();
			if (filteredItems.Contains(product))
			{
				var index = filteredItems.IndexOf(product);
				var indexPath = NSIndexPath.FromRowSection(index, tableSection);
				indexPaths.Add(indexPath);
			}

			if (categoryName == Const.FavoritesCategory)
			{
				// Set new items to table source
				filteredItems = Presenter.GetProductList(categoryName);
				_tableSource.SetItems(filteredItems);
				TableView.DeleteRows(atIndexPaths: indexPaths.ToArray(), withRowAnimation: UITableViewRowAnimation.Fade);
			}
			else
			{
				TableView.ReloadRows(atIndexPaths: indexPaths.ToArray(), withRowAnimation: UITableViewRowAnimation.Fade);
			}
		}

		// -------------------- Event handlers --------------------
		private void TableSource_ItemSelected(object sender, Product product)
		{
			// Trigg presenter that user click on some product to see details
			Presenter.OnProductDetailsClick(product.Id);
			if (searchController.Active)
				searchController.DismissViewController(true, null);
		}

		public void TableSource_FavoriteClicked(object sender, Product product)
		{
			// Trigg presenter that user click on some product for adding it to favorite
			Presenter.OnFavoriteClick(product);
		}

		private void SearchButtonClickHandler(object sender, EventArgs e)
		{
			// Handle clicking on the Search button
			searchController.SearchBar.Text = "";
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
			switch (NamePriceSegmentControl.SelectedSegment)
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
	}

	#region UITableViewSource implementation
	public class ShowAllSource : UITableViewSource, IDisposable
	{
		private List<Product> items = new List<Product>();

		public event EventHandler<Product> ShowAllSource_ItemSelected;
		public event EventHandler<Product> ShowAllSource_FavoriteClicked;

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
			// Attach event
			_cell.ProductCell_FavoriteClicked -= ShowAllSource_FavoriteClicked;
			_cell.ProductCell_FavoriteClicked += ShowAllSource_FavoriteClicked;

			_cell.ConfigureWith(item);
		}

		public override void CellDisplayingEnded(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
		{
			var _cell = (ProductCell)cell;
			// Detach event
			_cell.ProductCell_FavoriteClicked -= ShowAllSource_FavoriteClicked;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			tableView.DeselectRow(indexPath, true);
			var item = items[indexPath.Row];
			ShowAllSource_ItemSelected?.Invoke(this, item);
		}

		protected override void Dispose(bool disposing)
		{
			System.Diagnostics.Debug.WriteLine(String.Format("{0} object disposed", this.GetType()));
			base.Dispose(disposing);
		}
	}
	#endregion

	#region UISearchResultsUpdating implementation
	public class SearchResultsUpdator : UISearchResultsUpdating, IDisposable
	{
		public event EventHandler<string> UpdateSearchResults;

		public override void UpdateSearchResultsForSearchController(UISearchController searchController)
		{
			UpdateSearchResults?.Invoke(this, searchController.SearchBar.Text);
		}

		protected override void Dispose(bool disposing)
		{
			System.Diagnostics.Debug.WriteLine(String.Format("{0} object disposed", this.GetType()));
			base.Dispose(disposing);
		}
	}
	#endregion
}
