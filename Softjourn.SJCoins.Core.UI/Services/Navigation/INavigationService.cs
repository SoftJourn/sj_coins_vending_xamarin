using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Softjourn.SJCoins.Core.API.Model.Products;

namespace Softjourn.SJCoins.Core.UI.Services.Navigation
{
    public interface INavigationService
    {   
        // Navigate to page without closind previous page
        void NavigateTo(NavigationPage page, Object obj=null);

        // Navigate to page with closind previous page
        void NavigateToAsRoot(NavigationPage page);

        //void NavigationToShowAll(string category);

        //void NavigationToDetails(Product product);
    }
}
