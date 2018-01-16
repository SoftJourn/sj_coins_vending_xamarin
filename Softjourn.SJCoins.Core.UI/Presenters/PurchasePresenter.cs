using System;
using Softjourn.SJCoins.Core.Exceptions;
using Softjourn.SJCoins.Core.Helpers;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Core.Utils;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
    public class PurchasePresenter : BasePresenter<IPurchaseView>
    {

        public async void OnStartLoadingPage()
        {
            if (NetworkUtils.IsConnected)
            {
                try
                {
                    View.ShowProgress(Resources.StringResources.progress_loading);
                    var purchaseList = await RestApiServise.GetPurchaseHistory();

                    //Converting DateTime in appropriate string for UI
                    foreach (var item in purchaseList)
                    {
                        item.PrettyTime = TimeUtils.GetPrettyTime(item.Time);
                    }
                    View.HideProgress();

                    //If list is Empty Show empty View
                    //else show data
                    if (purchaseList.Count == 0)
                    {
                        View.ShowEmptyView();
                    }
                    else
                    {
                        View.SetData(purchaseList);
                    }
                }
                catch (ApiNotAuthorizedException ex)
                {
                    View.HideProgress();
                    //AlertService.ShowToastMessage(ex.Message);
                    DataManager.Profile = null;
                    Settings.ClearUserData();
                    NavigationService.NavigateToAsRoot(NavigationPage.Login);
                }
                catch (Exception ex)
                {
                    View.HideProgress();
                    AlertService.ShowToastMessage(ex.Message);
                }
            }
            else
            {
                AlertService.ShowToastMessage(Resources.StringResources.internet_turned_off);
            }
        }
    }
}
