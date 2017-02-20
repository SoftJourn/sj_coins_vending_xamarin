using System;
using System.Threading.Tasks;
using Autofac;
using Softjourn.SJCoins.Core.API.Model;
using Softjourn.SJCoins.Core.Exceptions;
using Softjourn.SJCoins.Core.Managers;
using Softjourn.SJCoins.Core.UI.Bootstrapper;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Core.Utils;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
    public class QrPresenter : BasePresenter<IQrView>
    {
        private QrManager _qrManager;
        public QrPresenter()
        {
            _scope = BaseBootstrapper.Container.BeginLifetimeScope();
            _qrManager = _scope.Resolve<QrManager>();
        }

        public async void ScanCode()
        {
            try
            {
                var code = await _qrManager.GetCodeFromQr();

                if (code != null)
                {
                    var result = await GetMoney(code);
                    View.UpdateBalance(result.Remain.ToString());
                }
            }
            catch (CameraException e)
            {
                AlertService.ShowToastMessage(e.ToString());
            }
        }

        public async Task<DepositeTransaction> GetMoney(Cash scannedCode)
        {
            if (NetworkUtils.IsConnected)
            {
                try
                {
                   return await RestApiServise.GetOfflineCash(scannedCode);
                    
                }
                catch (ApiNotAuthorizedException ex)
                {
                    AlertService.ShowToastMessage(ex.Message);
                    NavigationService.NavigateToAsRoot(NavigationPage.Login);
                }
                catch (Exception ex)
                {
                    AlertService.ShowToastMessage(ex.Message);
                }
            }
            else
            {
                AlertService.ShowToastMessage(Resources.StringResources.internet_turned_off);
            }
            return null;
        }
    }
}
