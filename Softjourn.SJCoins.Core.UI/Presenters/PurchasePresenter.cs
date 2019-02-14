using System;
using Softjourn.SJCoins.Core.Common;
using Softjourn.SJCoins.Core.Common.Exceptions;
using Softjourn.SJCoins.Core.Common.Utils;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;

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
                    View.ShowProgress(Resources.UiMessageResources.progress_loading);
                    var purchaseList = await RestApiService.GetPurchaseHistory();

                    //Converting DateTime in appropriate string for UI
                    foreach (var item in purchaseList)
                    {
                        item.PrettyTime = TimeUtils.GetPrettyTime(item.Time);
                    }

                    View.HideProgress();

                    if (purchaseList.Count == Constant.Zero)
                        View.ShowEmptyView();
                    else
                        View.SetData(purchaseList);
                }
                catch (ApiNotAuthorizedException)
                {
                    View.HideProgress();
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
                AlertService.ShowToastMessage(Resources.UiMessageResources.internet_turned_off);
            }
        }
    }
}
