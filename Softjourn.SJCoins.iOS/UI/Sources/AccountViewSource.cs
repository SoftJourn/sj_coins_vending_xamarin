﻿﻿using System;
using System.Collections.Generic;
using Foundation;
using Softjourn.SJCoins.Core.API.Model;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Sources
{
	public class AccountViewSource : UITableViewSource
	{
		private List<AccountOption> options = new List<AccountOption>();

		public event EventHandler<AccountOption> ItemSelected;

		public AccountViewSource(List<AccountOption> options)
		{
            this.options = options;
        }

        public override nint RowsInSection(UITableView tableview, nint section) => options.Count;

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) => (AccountCell)tableView.DequeueReusableCell(AccountCell.Key, indexPath);

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
