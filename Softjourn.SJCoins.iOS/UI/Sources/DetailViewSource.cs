using System;
using System.Linq;
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

		public override nint NumberOfSections(UITableView tableView)// => numberOfSections;
		{
			return numberOfSections;
		}

		public override string TitleForHeader(UITableView tableView, nint section)
		{
			switch (section)
			{
				case descriptionSection:
					return "Descriptions";
				case nutritionSection:
					return product.NutritionFacts.Count > 0 ? "Nutrition Facts" : "";
				default:
					return "";
			}
		}

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

		//public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		//{
		//	switch (indexPath.Section)
		//	{
		//		case descriptionSection:
		//			return 150.0f;
		//		case nutritionSection:
		//			return 50.0f;
		//		default:
		//			return 0;
		//	}
		//}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) 
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
					var key = product.NutritionFacts.Keys.ElementAt(indexPath.Row);
					var value = product.NutritionFacts.Values.ElementAt(indexPath.Row);
					nutritionCell.ConfigureWith(key, value);
					break;
			}
		}
	}
}
