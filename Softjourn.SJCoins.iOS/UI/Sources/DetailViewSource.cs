using System;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using UIKit;

namespace Softjourn.SJCoins.iOS
{
	public class DetailViewSource : UITableViewSource
	{
		private Product product;

		public DetailViewSource(Product product)
		{
			this.product = product;
		}

		public override nint NumberOfSections(UITableView tableView) => 2;

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			switch (section)
			{
				case 0:
					return optionsFirstSection.Count;
				case 1:
					return product.NutritionFacts.Count;
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
	}
}
