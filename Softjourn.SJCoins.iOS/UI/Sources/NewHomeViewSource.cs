using System;
using System.Collections.Generic;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Sources
{
	public class NewHomeViewSource : UITableViewSource
	{
		public List<Categories> Categories { get; set; }

		public override nint RowsInSection(UITableView tableview, nint section) => Categories.Count;

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)  => tableView.DequeueReusableCell(NewHomeCell.Key, indexPath);

		public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
		{
			var _cell = (NewHomeCell)cell;
			_cell.ConfigureWith(Categories[indexPath.Row]);
			//_cell.setCollectionViewDataSourceAndDelegate(this, indexPath.Row);
		}
	}
}
