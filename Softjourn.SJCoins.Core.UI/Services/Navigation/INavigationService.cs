using System;

namespace Softjourn.SJCoins.Core.UI.Services.Navigation
{
    public interface INavigationService
    {
        /// <summary>
        /// Navigate to page without closing previous page
        /// </summary>
        /// <param name="page"></param>
        /// <param name="obj"></param>
        void NavigateTo(NavigationPage page, Object obj=null);

        /// <summary>
        /// Navigate to page with closing previous page
        /// </summary>
        /// <param name="page"></param>
        void NavigateToAsRoot(NavigationPage page);
    }
}
