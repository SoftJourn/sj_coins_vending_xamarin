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
		private ShowAllSource _tableSource;
		private NSIndexPath _favoriteCellIndex;

		public string CategoryName { get; set; }
		public List<Product> filteredItems;
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
			// Configure table view with source and events.
			ConfigureTableView();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			Title = CategoryName;
			// Attach 
			SegmentControl.TouchUpInside += SameButtonClickHandler;
			SegmentControl.ValueChanged += AnotherButtonClickHandler;
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
		}

		public override void ViewWillDisappear(bool animated)
		{
			// Dettach 
			SegmentControl.TouchUpInside += SameButtonClickHandler;
			SegmentControl.ValueChanged += AnotherButtonClickHandler;
			base.ViewWillDisappear(animated);
		}
		#endregion

		#region BaseViewController -> IBaseView implementation
		#endregion

		#region Private methods 
		private void ConfigureTableView()
		{
			_tableSource = new ShowAllSource(filteredItems);
			_tableSource.ItemSelected -= TableSource_ItemSelected;
			_tableSource.ItemSelected += TableSource_ItemSelected;

			_tableSource.FavoriteClicked -= TableSource_FavoriteClicked;
			_tableSource.FavoriteClicked += TableSource_FavoriteClicked;
			TableView.Source = _tableSource;

			TableView.RegisterNibForCellReuse(ProductCell.Nib, ProductCell.Key);
		}

		private void TableSource_ItemSelected(object sender, Product product)
		{
			// Trigg presenter that user click on some product to see details
			Presenter.OnProductDetailsClick(product.Id);
		}

		public void TableSource_FavoriteClicked(object sender, ProductCell cell)
		{
			// Trigg presenter that user click on some product for adding it to favorite
			_favoriteCellIndex = TableView.IndexPathForCell(cell);
			Presenter.OnFavoriteClick(cell.Product);
		}

		// ---------------- SegmentControl methods ---------------- 
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

		private void SortItems(string categoryName)
		{
			switch (SegmentControl.SelectedSegment)
			{
				case 0: // Name button
					Presenter.OnSortByNameClicked(categoryName);
					break;
				case 1:	// Price button
					Presenter.OnSortByPriceClicked(categoryName);
					break;
				default:
					break;
			}
		}
		// -------------------------------------------------------- 
		#endregion

		#region IAccountView implementation
		public void FavoriteChanged(bool isFavorite)
		{
			// table reload row at index
			if (_favoriteCellIndex != null)
			{
				var index = new NSIndexPath[] { _favoriteCellIndex };
				if (CategoryName == "Favorites")
				{
					// Set new items to table source
					_tableSource.SetItems(Presenter.GetProductList(CategoryName));
					// Delete row
					TableView.DeleteRows(atIndexPaths: index, withRowAnimation: UITableViewRowAnimation.Fade);
				}
				else {
					TableView.ReloadRows(atIndexPaths: index, withRowAnimation: UITableViewRowAnimation.Fade);
				}
			}
		}

		public void ShowSortedList(List<Product> products)
		{
			_tableSource.SetItems(products);
			TableView.ReloadData();
		}
		#endregion
	}

	#region UITableViewSource implementation
	public class ShowAllSource : UITableViewSource
	{
		private List<Product> items;
		public event EventHandler<Product> ItemSelected;
		public event EventHandler<ProductCell> FavoriteClicked;

		public ShowAllSource(List<Product> items)
		{
			this.items = items;
		}

		public void SetItems(List<Product> items)
		{
			this.items = items;
		}

		public override nint RowsInSection(UITableView tableview, nint section) => items.Count;

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) => (ProductCell)tableView.DequeueReusableCell(ProductCell.Key, indexPath);

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
			if (ItemSelected != null)
			{
				ItemSelected(this, item);
			}
		}
	}
	#endregion
}
