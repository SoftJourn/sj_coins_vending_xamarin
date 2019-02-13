using System;
using System.Collections.Generic;
using Foundation;
using Softjourn.SJCoins.Core.Models;
using Softjourn.SJCoins.iOS.UI.Cells;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Sources.AccountPage
{
    public class PurchaseViewSource : UITableViewSource
    {
        private readonly List<History> items = new List<History>();

        public PurchaseViewSource(List<History> items)
        {
            this.items = items;
        }

        public override nint RowsInSection(UITableView tableview, nint section) => items.Count;

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) => tableView.DequeueReusableCell(PurchaseCell.Key, indexPath);

        public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
        {
            var _cell = (PurchaseCell)cell;
            var item = items[indexPath.Row];
            _cell.ConfigureWith(item);
        }
    }
}
