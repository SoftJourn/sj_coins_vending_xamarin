using System;
using System.Linq;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using UIKit;

namespace Softjourn.SJCoins.iOS
{
	public class DetailTableViewSource : UITableViewSource
	{
        //public event EventHandler DidScroll;

		private const int oneCell = 1;
		private const int descriptionSection = 0;
		private const int nutritionSection = 1;

		private Product product;
		private int numberOfSections = 2;

		public DetailTableViewSource(Product product)
		{
			this.product = product;

            if (String.IsNullOrEmpty(product.Description) && product.NutritionFacts.Count == 0)
            {
                numberOfSections = 0;  
            }
		}

		public override nint NumberOfSections(UITableView tableView) => numberOfSections;

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

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) 
		{
			switch (indexPath.Section)
			{
				case descriptionSection:
					var descriptionCell = (DescriptionCell)tableView.DequeueReusableCell(DescriptionCell.Key, indexPath);;
					descriptionCell.ConfigureWith(product.Description);
					return descriptionCell;
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
					break;
				case nutritionSection:
					var nutritionCell = (NutritionCell)cell;
					var key = product.NutritionFacts.Keys.ElementAt(indexPath.Row);
					var value = product.NutritionFacts.Values.ElementAt(indexPath.Row);
					nutritionCell.ConfigureWith(key, value);
					break;
			}
		}

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            return 70f;
        }

        //public override void Scrolled(UIScrollView scrollView)
        //{
        //    DidScroll?.Invoke(this, null);
        //}
	}
}
