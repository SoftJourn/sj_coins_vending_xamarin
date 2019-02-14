using System;
using System.Collections.Generic;
using System.Diagnostics;
using Foundation;
using Softjourn.SJCoins.Core.Models.Products;
using Softjourn.SJCoins.iOS.General.Constants;
using Softjourn.SJCoins.iOS.UI.Cells;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Sources
{
    public class SeeAllViewSource : UITableViewSource
    {
        private List<Product> items = new List<Product>();
        private readonly string categoryName;

        public event EventHandler<Product> ItemSelected;
        public event EventHandler<Product> FavoriteClicked;
        public event EventHandler<Product> BuyClicked;

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

            _cell.BuyClicked -= BuyClicked;
            _cell.BuyClicked += BuyClicked;

            _cell.ConfigureWith(item);

            if (categoryName == Const.FavoritesCategory)
                _cell.MarkFavorites(item);
        }

        public override void CellDisplayingEnded(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
        {
            var _cell = (ProductCell)cell;
            // Detach event
            _cell.FavoriteClicked -= FavoriteClicked;
            _cell.BuyClicked -= BuyClicked;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            tableView.DeselectRow(indexPath, true);
            ItemSelected?.Invoke(this, items[indexPath.Row]);
        }

        protected override void Dispose(bool disposing)
        {
            Debug.WriteLine($"{GetType()} disposed");
            base.Dispose(disposing);
        }
    }
}
