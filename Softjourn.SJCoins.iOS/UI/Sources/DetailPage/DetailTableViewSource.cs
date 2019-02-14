using System;
using System.Linq;
using Foundation;
using Softjourn.SJCoins.Core.Common;
using Softjourn.SJCoins.Core.Models.Products;
using Softjourn.SJCoins.iOS.UI.Cells;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Sources.DetailPage
{
    public class DetailTableViewSource : UITableViewSource
    {
        private const int oneCell = 1;
        private const int descriptionSection = 0;
        private const int nutritionSection = 1;

        private readonly Product product;
        private readonly int numberOfSections = 2;

        public DetailTableViewSource(Product product)
        {
            this.product = product;

            if (string.IsNullOrEmpty(product.Description) && !product.NutritionFacts.Any())
                numberOfSections = Constant.Zero;
        }

        public override nint NumberOfSections(UITableView tableView) => numberOfSections;

        public override string TitleForHeader(UITableView tableView, nint section)
        {
            switch (section)
            {
                case descriptionSection:
                    return "Descriptions";
                case nutritionSection:
                    return product.NutritionFacts.Any()
                        ? "Nutrition Facts"
                        : string.Empty;
                default:
                    return string.Empty;
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
                    return Constant.Zero;
            }
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            switch (indexPath.Section)
            {
                case descriptionSection:
                    var descriptionCell = (DescriptionCell)tableView.DequeueReusableCell(DescriptionCell.Key, indexPath);
                    descriptionCell.ConfigureWith(product.Description);
                    return descriptionCell;
                case nutritionSection:
                    return tableView.DequeueReusableCell(NutritionCell.Key, indexPath);
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

        public override nfloat GetHeightForHeader(UITableView tableView, nint section) => 70f;
    }
}
