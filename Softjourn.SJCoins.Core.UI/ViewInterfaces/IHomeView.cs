using System.Collections.Generic;
using Softjourn.SJCoins.Core.Models.AccountInfo;
using Softjourn.SJCoins.Core.Models.Products;

namespace Softjourn.SJCoins.Core.UI.ViewInterfaces
{
    public interface IHomeView : IBaseProductView
    {        
        void SetAccountInfo(Account account);

        void SetUserBalance(string balance);

        void SetMachineName(string name);

        void ShowProducts(List<Categories> listCategories); //have to add Favorites to the first place to list

        void ServiceNotAvailable();

        void ImageAcquired(byte[] receipt);
    }
}
