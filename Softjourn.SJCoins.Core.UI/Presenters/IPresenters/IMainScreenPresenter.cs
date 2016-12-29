using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Softjourn.SJCoins.Core.API.Model.Products;

namespace Softjourn.SJCoins.Core.UI.Presenters.IPresenters
{
    public interface IMainScreenPresenter
    {
        /**
         * Checks if Machine was chosen.
         * If machine was not chosen no products will appear
         * and calls related to products will be blocked till machine will not be chosen.
         */
        void IsMachineSet();

        void GetFeaturedProductsList();

        void GetFavoritesList();

        void GetMachinesList();

        void GetBalance();

        List<Categories> GetCategories();

        /**
         * Get all categories loaded from server for dynamic creating views
         */
        void GetCategoriesFromDb();
    }
}
