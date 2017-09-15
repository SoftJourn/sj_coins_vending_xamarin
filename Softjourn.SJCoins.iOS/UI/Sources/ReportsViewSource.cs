﻿using System;
using System.Collections.Generic;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.TransactionReports;
using Softjourn.SJCoins.iOS.UI.Cells;
using UIKit;

namespace Softjourn.SJCoins.iOS
{
	public class ReportsViewSource : UITableViewSource
	{
		private const int tableSection = 0;
		private const int rowBeforeEnd = 15;
		private const int numberOfItemsOnOnePage = 50;

		private List<Transaction> items = new List<Transaction>();

		public event EventHandler GetNexPage;

		public void SetItems(List<Transaction> items)
		{
			this.items = items ?? new List<Transaction>();
		}

		public void AddItems(List<Transaction> items, UITableView tableView)
		{
			// Add new items to existing list
			items.AddRange(items);

			// Create empty list
			var indexPaths = new List<NSIndexPath>();

			// Add elements to list
			foreach (Transaction item in items)
			{
				if (items.Contains(item))
				{
					var index = items.IndexOf(item);
					var indexPath = NSIndexPath.FromRowSection(index, tableSection);
					indexPaths.Add(indexPath);
				}
			}

			// Insert into table
			tableView.InsertRows(atIndexPaths: indexPaths.ToArray(), withRowAnimation: UITableViewRowAnimation.Fade);
		}

		public override nint RowsInSection(UITableView tableview, nint section) => items.Count;

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) => tableView.DequeueReusableCell(TransactionCell.Key, indexPath);

		public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
		{
			var _cell = (TransactionCell)cell;
			_cell.ConfigureWith(items[indexPath.Row]);

			if (indexPath.Row == items.Count - rowBeforeEnd && items.Count >= numberOfItemsOnOnePage)
			{
				// trigg presenter to give the next page.
				GetNexPage?.Invoke(this, null);
			}
		}
	}
}
