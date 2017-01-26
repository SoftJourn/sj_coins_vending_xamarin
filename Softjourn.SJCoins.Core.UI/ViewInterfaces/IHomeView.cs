using Softjourn.SJCoins.Core.API.Model.AccountInfo;
using Softjourn.SJCoins.Core.API.Model.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softjourn.SJCoins.Core.UI.ViewInterfaces
{
    public interface IHomeView : IBaseView
    {        
        void SetAccountInfo(Account account);
        void SetUserBalance(String balance);
        void SetMachineName(String name);

        void ShowProducts(List<Categories> listCategories); //have to add Favorites to the first place to list
        void showPurchaseConfirmationDialog(Product product); 
    }
}
