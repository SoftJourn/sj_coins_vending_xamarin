using System;
using System.Collections.Generic;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Sources
{
	public class NewHomeViewSource : UITableViewSource
	{
		public List<Categories> Categories { get; set; } = new List<Categories>();

		public event EventHandler<Product> NewHomeViewSource_ItemSelected;
		public event EventHandler<string> NewHomeViewSource_SeeAllClicked;
		public event EventHandler<Product> NewHomeViewSource_BuyExecuted;
		public event EventHandler<Product> NewHomeViewSource_AddDeleteFavoriteExecuted;

		public override nint RowsInSection(UITableView tableview, nint section) => Categories.Count;

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) => tableView.DequeueReusableCell(NewHomeCell.Key, indexPath);

		public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
		{
			if (Categories.Count > 0)
			{
				var _cell = (NewHomeCell)cell;
				_cell.ConfigureWith(Categories[indexPath.Row]);

				_cell.NewHomeCell_ItemSelected -= NewHomeViewSource_ItemSelected;
				_cell.NewHomeCell_ItemSelected += NewHomeViewSource_ItemSelected;

				_cell.NewHomeCell_SeeAllClicked -= NewHomeViewSource_SeeAllClicked;
				_cell.NewHomeCell_SeeAllClicked += NewHomeViewSource_SeeAllClicked;

				_cell.NewHomeCell_BuyActionExecuted -= NewHomeViewSource_BuyExecuted;
				_cell.NewHomeCell_BuyActionExecuted += NewHomeViewSource_BuyExecuted;

				_cell.NewHomeCell_FavoriteActionExecuted -= NewHomeViewSource_AddDeleteFavoriteExecuted;
				_cell.NewHomeCell_FavoriteActionExecuted += NewHomeViewSource_AddDeleteFavoriteExecuted;
			}
		}

		public override void CellDisplayingEnded(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
		{
			var _cell = (NewHomeCell)cell;
			_cell.NewHomeCell_ItemSelected -= NewHomeViewSource_ItemSelected;
			_cell.NewHomeCell_SeeAllClicked -= NewHomeViewSource_SeeAllClicked;
			_cell.NewHomeCell_BuyActionExecuted -= NewHomeViewSource_BuyExecuted;
			_cell.NewHomeCell_FavoriteActionExecuted -= NewHomeViewSource_AddDeleteFavoriteExecuted;
		}
	}
}
