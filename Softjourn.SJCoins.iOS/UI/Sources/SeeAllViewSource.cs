using System;
using UIKit;
using System.Collections.Generic;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.iOS.UI.Cells;
using Softjourn.SJCoins.iOS.General.Constants;

namespace Softjourn.SJCoins.iOS
{
    public class SeeAllViewSource: UITableViewSource, IDisposable
    {
		private List<Product> items = new List<Product>();
		private string categoryName;

		public event EventHandler<Product> ItemSelected;
		public event EventHandler<Product> FavoriteClicked;

		public SeeAllViewSource(string categoryName)
		{
			this.categoryName = categoryName;
		}

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
			_cell.FavoriteClicked -= FavoriteClicked;
			_cell.FavoriteClicked += FavoriteClicked;

			_cell.ConfigureWith(item);

			if (categoryName == Const.FavoritesCategory)
				_cell.MarkFavorites(item);
		}

		public override void CellDisplayingEnded(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
		{
			var _cell = (ProductCell)cell;
			// Detach event
			_cell.FavoriteClicked -= FavoriteClicked;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			tableView.DeselectRow(indexPath, true);
			var item = items[indexPath.Row];
			ItemSelected?.Invoke(this, item);
		}

		protected override void Dispose(bool disposing)
		{
			System.Diagnostics.Debug.WriteLine(String.Format("{0} disposed", this.GetType()));
			base.Dispose(disposing);
		}
	}
}
