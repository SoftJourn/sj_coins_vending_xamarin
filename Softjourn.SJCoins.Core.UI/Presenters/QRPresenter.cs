using System;
using System.Threading.Tasks;
using Autofac;
using Softjourn.SJCoins.Core.API.Model;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Core.Exceptions;
using Softjourn.SJCoins.Core.Managers;
using Softjourn.SJCoins.Core.UI.Bootstrapper;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.Core.UI.Utils;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Core.Utils;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
    public class QrPresenter : BasePresenter<IQrView>
    {
        public int MyBalance
        {
            get { return DataManager.Profile.Amount; }
            set {}
        }

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

        public async void GenerateCode(string amount)
        {
            if (ValidateAmount(amount))
            {
                await WithdrawMoney(amount);
            }
        }

        public int GetBalance()
        {
            return MyBalance;
        }

        private bool ValidateAmount(string amount)
        {
            if (string.IsNullOrEmpty(amount))
            {
                View.SetEditFieldError(Resources.StringResources.error_field_is_empty);
                return false;
            }
            if (!Validators.IsAmountValid(amount))
            {
                View.SetEditFieldError(Resources.StringResources.error_field_contains_not_digits);
                return false;
            }

            if (Convert.ToInt32(amount) > MyBalance)
            {
                View.SetEditFieldError(Resources.StringResources.error_field_not_enough_money);
                return false;
            }
            return true;
        }

        private async Task<DepositeTransaction> GetMoney(Cash scannedCode)
        {
            if (NetworkUtils.IsConnected)
            {
                try
                {
                    View.ShowProgress(Resources.StringResources.progress_loading);
                    return await RestApiServise.GetOfflineCash(scannedCode);
                }
                catch (ApiNotAuthorizedException ex)
                {
                    View.HideProgress();
                    AlertService.ShowToastMessage(ex.Message);
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
                View.HideProgress();
                AlertService.ShowToastMessage(Resources.StringResources.internet_turned_off);
            }
            return null;
        }

        private async Task<Cash> WithdrawMoney(string amount)
        {
            if (NetworkUtils.IsConnected)
            {
                try
                {
                    View.ShowProgress(Resources.StringResources.progress_loading);
                    var amountJson = new Amount {Balance = amount};

                    var code = await RestApiServise.WithdrawMoney(amountJson);

                    View.HideProgress();
                    View.ShowImage(_qrManager.ConvertCashObjectToString(code));

                    DataManager.Profile = await RestApiServise.GetUserAccountAsync();

                    View.UpdateBalance(MyBalance.ToString());

                }
                catch (ApiNotAuthorizedException ex)
                {
                    View.HideProgress();
                    AlertService.ShowToastMessage(ex.Message);
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
                View.HideProgress();
                AlertService.ShowToastMessage(Resources.StringResources.internet_turned_off);
            }
            return null;
        }
    }
}
