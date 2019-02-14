using System.Collections.Generic;
using Softjourn.SJCoins.Core.Models.Products;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
    public class ShowAllPresenter : BaseProductPresenter<IShowAllView>
    {
        public ShowAllPresenter()
        {
            MyBalance = DataManager.Profile.Amount;
        }

        private bool IsSortingNameForward { get; set; }
        private bool IsSortingPriceForward { get; set; }

        public override void ChangeUserBalance(string balance)
        {
        }

        public List<Product> GetProductList(string category) => DataManager.GetSortedByNameProductsList(category, true);

        public void OnSortByNameClicked(string category)
        {
            View.ShowSortedList(DataManager.GetSortedByNameProductsList(category, IsSortingNameForward));
            IsSortingPriceForward = true;
            IsSortingNameForward = !IsSortingNameForward;
            View.SetCompoundDrawableName(IsSortingNameForward);
            View.SetCompoundDrawablePrice(null);
        }

        public void OnSortByPriceClicked(string category)
        {
            View.ShowSortedList(DataManager.GetSortedByPriceProductsList(category, IsSortingPriceForward));
            IsSortingNameForward = true;
            IsSortingPriceForward = !IsSortingPriceForward;
            View.SetCompoundDrawablePrice(IsSortingPriceForward);
            View.SetCompoundDrawableName(null);
        }
    }
}
