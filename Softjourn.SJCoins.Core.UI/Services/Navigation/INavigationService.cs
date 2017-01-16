using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softjourn.SJCoins.Core.UI.Services.Navigation
{
    public interface INavigationService
    {
        void NavigateTo(NavigationPage page);
        void NavigateToAsRoot(NavigationPage page);
    }
}
