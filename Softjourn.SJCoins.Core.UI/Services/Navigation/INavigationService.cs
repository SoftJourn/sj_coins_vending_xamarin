using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softjourn.SJCoins.Core.UI.Services.Navigation
{
    public interface INavigationService
    {   
        // Navigate to page without closind previous page
        void NavigateTo(NavigationPage page);

        // Navigate to page with closind previous page
        void NavigateToAsRoot(NavigationPage page);
    }
}
