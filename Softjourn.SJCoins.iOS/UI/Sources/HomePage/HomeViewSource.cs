using System;
using System.Collections.Generic;
using Foundation;
using Softjourn.SJCoins.Core.Models.Products;
using Softjourn.SJCoins.iOS.General.Helper;
using Softjourn.SJCoins.iOS.UI.Cells;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Sources.HomePage
{
    public class HomeViewSource : UITableViewSource
    {
        public List<Categories> Categories { get; set; } = new List<Categories>();

        public event EventHandler<Product> HomeViewSource_ItemSelected;
        public event EventHandler<string> HomeViewSource_SeeAllClicked;
        public event EventHandler<Product> HomeViewSource_BuyExecuted;
        public event EventHandler<Product> HomeViewSource_AddDeleteFavoriteExecuted;

        public override nint RowsInSection(UITableView tableview, nint section) => Categories.Count;

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) => tableView.DequeueReusableCell(HomeCell.Key);

        public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
        {
            if (Categories.Count > 0)
            {
                var _cell = (HomeCell)cell;
                _cell.ConfigureWith(Categories[indexPath.Row]);

                _cell.HomeCell_ItemSelected -= HomeViewSource_ItemSelected;
                _cell.HomeCell_ItemSelected += HomeViewSource_ItemSelected;

                _cell.HomeCell_SeeAllClicked -= HomeViewSource_SeeAllClicked;
                _cell.HomeCell_SeeAllClicked += HomeViewSource_SeeAllClicked;

                _cell.HomeCell_BuyActionExecuted -= HomeViewSource_BuyExecuted;
                _cell.HomeCell_BuyActionExecuted += HomeViewSource_BuyExecuted;

                _cell.HomeCell_FavoriteActionExecuted -= HomeViewSource_AddDeleteFavoriteExecuted;
                _cell.HomeCell_FavoriteActionExecuted += HomeViewSource_AddDeleteFavoriteExecuted;
            }
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return SizeHelper.VerticalCellHeight();
        }
    }
}
