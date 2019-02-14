using System;
using System.Collections.Generic;
using Foundation;
using Softjourn.SJCoins.Core.Models;
using Softjourn.SJCoins.iOS.UI.Cells;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Sources.AccountPage
{
    public class AccountViewSource : UITableViewSource
    {
        private readonly List<AccountOption> options;

        public event EventHandler<AccountOption> ItemSelected;

        public AccountViewSource(List<AccountOption> options)
        {
            this.options = options;
        }

        public override nint RowsInSection(UITableView tableview, nint section) => options.Count;

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) =>
            (AccountCell) tableView.DequeueReusableCell(AccountCell.Key, indexPath);

        public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
        {
            var _cell = (AccountCell)cell;
            _cell.ConfigureWith(options[indexPath.Row]);
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            tableView.DeselectRow(indexPath, true);
            ItemSelected?.Invoke(this, options[indexPath.Row]);
        }
    }
}
