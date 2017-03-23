using System;
using System.Collections.Generic;
using Foundation;
using Softjourn.SJCoins.Core.API.Model;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Sources
{
	public class AccountViewSource : UITableViewSource
	{
		private List<AccountOption> optionsFirstSection = new List<AccountOption>();
		private List<AccountOption> optionsSecondSection = new List<AccountOption>();

		public event EventHandler<AccountOption> ItemSelected;

		public AccountViewSource(List<AccountOption> first, List<AccountOption> second)
		{
			optionsFirstSection = first;
			optionsSecondSection = second;
		}

		public override nint NumberOfSections(UITableView tableView) => 2;

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			switch (section)
			{
				case 0:
					return optionsFirstSection.Count;
				case 1:
					return optionsSecondSection.Count;
				default:
					return 0;
			}
		}
		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) => (AccountCell)tableView.DequeueReusableCell(AccountCell.Key, indexPath);

		public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
		{
			var _cell = (AccountCell)cell;
			switch (indexPath.Section)
			{
				case 0:
					_cell.ConfigureWith(optionsFirstSection[indexPath.Row]);
					break;
				case 1:
					_cell.ConfigureWith(optionsSecondSection[indexPath.Row]);
					break;
			}
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			tableView.DeselectRow(indexPath, true);
			switch (indexPath.Section)
			{
				case 0:
					ItemSelected?.Invoke(this, optionsFirstSection[indexPath.Row]);
					break;
				case 1:
					ItemSelected?.Invoke(this, optionsSecondSection[indexPath.Row]);
					break;
			}
		}
	}
}
