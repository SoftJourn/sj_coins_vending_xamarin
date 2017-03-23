using System;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using UIKit;

namespace Softjourn.SJCoins.iOS
{
	public class DetailViewSource : UITableViewSource
	{
		private const int numberOfSections = 2;
		private const int oneCell = 1;
		private const int descriptionSection = 0;
		private const int nutritionSection = 1;

		private Product product;

		public DetailViewSource(Product product)
		{
			this.product = product;
		}

		public override nint NumberOfSections(UITableView tableView) => numberOfSections;

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			switch (section)
			{
				case descriptionSection: 
					return oneCell;
				case nutritionSection:
					return product.NutritionFacts.Count;
				default:
					return 0;
			}
		}
		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) //=> (AccountCell)tableView.DequeueReusableCell(AccountCell.Key, indexPath);
		{
			switch (indexPath.Section)
			{
				case descriptionSection:
					return (UITableViewCell)tableView.DequeueReusableCell(DescriptionCell.Key, indexPath);
				case nutritionSection:
					return (UITableViewCell)tableView.DequeueReusableCell(NutritionCell.Key, indexPath);
				default:
					return null;
			}
		}
		public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
		{
			switch (indexPath.Section)
			{
				case descriptionSection:
					var descriptionCell = (DescriptionCell)cell;
					descriptionCell.ConfigureWith(product.Description);
					break;
				case nutritionSection:
					var nutritionCell = (NutritionCell)cell;
					nutritionCell.ConfigureWith();
					break;
			}
		}
	}
}
