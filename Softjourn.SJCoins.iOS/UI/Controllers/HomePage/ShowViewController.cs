using System;
using System.Collections.Generic;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.iOS.General.Constants;
using Softjourn.SJCoins.iOS.General.Helper;
using Softjourn.SJCoins.iOS.UI.Cells;
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
		private string CategoryName { get; set; }

        private SeeAllViewSource tableSource;
		
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
				this.CategoryName = (string)categoryName;
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
			Title = CategoryName;
			// Throw to presenter category name what needs to be displayed and take products.
			filteredItems = Presenter.GetProductList(CategoryName);
			tableSource.SetItems(filteredItems);
            ReloadTable();

			NamePriceSegmentControl.Alpha = 1.0f;
		}

		[Export("willDismissSearchController:")]
		public void WillDismissSearchController(UISearchController searchController)
		{
			tableSource.SetItems(filteredItems);
			ReloadTable();
		}
		#endregion

		#region BaseViewController
		public override void AttachEvents()
		{
			base.AttachEvents();
			NamePriceSegmentControl.TouchUpInside += SameButtonClickHandler;
			NamePriceSegmentControl.ValueChanged += AnotherButtonClickHandler;

			tableSource.ItemSelected += TableSource_ItemSelected;
			tableSource.FavoriteClicked += TableSource_FavoriteClicked;
            tableSource.BuyClicked += TableSource_BuyClicked;

			//SearchButton.Clicked += SearchButtonClickHandler;

			searchResultsUpdator.UpdateSearchResults += SearchResultsUpdator_Search;
		}

		public override void DetachEvents()
		{
			NamePriceSegmentControl.TouchUpInside -= SameButtonClickHandler;
			NamePriceSegmentControl.ValueChanged -= AnotherButtonClickHandler;

			tableSource.ItemSelected -= TableSource_ItemSelected;
			tableSource.FavoriteClicked -= TableSource_FavoriteClicked;
			tableSource.BuyClicked -= TableSource_BuyClicked;

			//SearchButton.Clicked -= SearchButtonClickHandler;

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
			filteredItems = products;
			tableSource.SetItems(filteredItems);
            ReloadTable();
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

			if (CategoryName == Const.FavoritesCategory)
			{
				NavigationItem.RightBarButtonItem = null;
			}
		}

		private void ConfigureTableView()
		{
            tableSource = new SeeAllViewSource(CategoryName);
			TableView.Source = tableSource;
		}

		private void ConfigureSegmentControl()
		{
			_segmentControlHelper = new SegmentControlHelper();
			// Configure 0 segment
			ConfigureSegment(NameTitle, NameSegment, ImageConstants.ArrowUpward);
		}

		private void ReloadTable()
		{
			TableView.ReloadSections(new NSIndexSet(0), UITableViewRowAnimation.Automatic);
		}

		private void Search(string searchString)
		{
			var search = searchString.Trim();
			searchData.Clear();

			if (searchController.SearchBar.Text != "")
			{
				searchData = filteredItems.FindAll(item => item.Name.ToLower().Contains(search.ToLower()));
				tableSource.SetItems(searchData);
			}
			else
			{
				tableSource.SetItems(filteredItems);
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

			if (CategoryName == Const.FavoritesCategory)
			{
				// Set new items to table source
				filteredItems = Presenter.GetProductList(CategoryName);
				tableSource.SetItems(filteredItems);
                TableView.DeleteRows(atIndexPaths: indexPaths.ToArray(), withRowAnimation: UITableViewRowAnimation.Automatic);
			}
			else
			{
                TableView.ReloadRows(atIndexPaths: indexPaths.ToArray(), withRowAnimation: UITableViewRowAnimation.Automatic);
			}
		}
		#endregion

		#region Event handlers
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

		public void TableSource_BuyClicked(object sender, Product product)
		{
			// Trigg presenter that user click on some product for buying
            Presenter.OnBuyProductClick(product);
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
			SortItems(CategoryName);
		}

		private void AnotherButtonClickHandler(object sender, EventArgs e)
		{
			// Handle clicking on the another button of segment control
			SortItems(CategoryName);
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
		#endregion
	}

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
			System.Diagnostics.Debug.WriteLine(String.Format("{0} disposed", this.GetType()));
			base.Dispose(disposing);
		}
	}
	#endregion
}
