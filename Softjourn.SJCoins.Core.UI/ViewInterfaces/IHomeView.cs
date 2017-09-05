using Softjourn.SJCoins.Core.API.Model.AccountInfo;
using Softjourn.SJCoins.Core.API.Model.Products;
using System;
using System.Collections.Generic;
using Softjourn.SJCoins.Core.UI.Interfaces;

namespace Softjourn.SJCoins.Core.UI.ViewInterfaces
{
    public interface IHomeView : IBaseProductView
    {        
        void SetAccountInfo(Account account);
        void SetUserBalance(String balance);
        void SetMachineName(String name);
        void ShowProducts(List<Categories> listCategories); //have to add Favorites to the first place to list
        void ServiceNotAvailable();
    }
}
