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
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
		}

		public void FavoriteChanged()
		{
			throw new NotImplementedException();
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
			TableView.Source = _tableSource;

			TableView.RegisterNibForCellReuse(ProductCell.Nib, ProductCell.Key);
		}

		private void TableSource_ItemSelected(object sender, Product product)
		{
			Presenter.OnProductDetailsClick(product.Id);
		}

		public void FavoriteChanged(bool isFavorite)
		{
			throw new NotImplementedException();
		}
		#endregion
	}

	#region UITableViewSource implementation
	public class ShowAllSource : UITableViewSource
	{
		private List<Product> items;
		public event EventHandler<Product> ItemSelected;

		public ShowAllSource(List<Product> items)
		{
			this.items = items;
		}

		public override nint RowsInSection(UITableView tableview, nint section) => items.Count;

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) => (ProductCell)tableView.DequeueReusableCell(ProductCell.Key, indexPath);

		public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
		{
			var _cell = (ProductCell)cell;
			var item = items[indexPath.Row];
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
