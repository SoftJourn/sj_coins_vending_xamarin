using System;
using System.Threading.Tasks;
using Autofac;
using Softjourn.SJCoins.Core.Common;
using Softjourn.SJCoins.Core.Common.Exceptions;
using Softjourn.SJCoins.Core.Common.Utils;
using Softjourn.SJCoins.Core.Managers;
using Softjourn.SJCoins.Core.Models;
using Softjourn.SJCoins.Core.Models.Products;
using Softjourn.SJCoins.Core.UI.Bootstrapper;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.Core.UI.Utils;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
    public class QrPresenter : BasePresenter<IQrView>
    {
        public int MyBalance => DataManager.Profile.Amount;

        private readonly QrManager QrManager;

        public QrPresenter()
        {
            Scope = BaseBootstrapper.Container.BeginLifetimeScope();
            QrManager = Scope.Resolve<QrManager>();
        }

        /// <summary>
        /// Calls Method of ZXing scanning to get code
        /// and if success call API method to proceed transaction
        /// </summary>
        public async void ScanCode()
        {
            try
            {
                var code = await QrManager.GetCodeFromQr();
                if (code != null)
                    UpdateBalance(await GetMoney(code));
            }
            catch (JsonReaderExceptionCustom)
            {
                AlertService.ShowToastMessage(Resources.UiMessageResources.error_not_valid_qr_code);
            }
            catch (CameraException e)
            {
                AlertService.ShowToastMessage(e.Message);
            }
        }

        /// <summary>
        /// Using only on IOS platform
        /// </summary>
        /// <param name="cashObject"></param>
        public async void ScanCodeIOS(Cash cashObject)
        {
            try
            {
                UpdateBalance(await GetMoney(cashObject));
            }
            catch (CameraException e)
            {
                AlertService.ShowToastMessage(e.ToString());
            }
        }

        /// <summary>
        /// If amount is valid call API method for creating transaction
        /// </summary>
        /// <param name="amount"></param>
        public async void GenerateCode(string amount)
        {
            if (ValidateAmount(amount))
            {
                try
                {
                    await PermissionsUtils.CheckGalleryPermissionAsync();
                    await WithdrawMoney(amount);
                }
                catch (CameraException e)
                {
                    AlertService.ShowToastMessage(e.Message);
                }
            }
        }

        /// <summary>
        /// Return Current Balance from DataManager
        /// </summary>
        /// <returns></returns>
        public int GetBalance()
        {
            return MyBalance;
        }

        public async Task CheckPermission()
        {
            await PermissionsUtils.CheckCameraPermissionAsync();
        }

        /// <summary>
        /// Return true if amount is not empty, is integer and not exceeds user's balance
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool ValidateAmount(string amount)
        {
            if (string.IsNullOrEmpty(amount))
            {
                View.SetEditFieldError(Resources.UiMessageResources.error_field_is_empty);
                return false;
            }

            if (!Validators.IsAmountValid(amount))
            {
                View.SetEditFieldError(Resources.UiMessageResources.error_field_contains_not_digits);
                return false;
            }

            if (amount.Length > 10)
            {
                View.SetEditFieldError(Resources.UiMessageResources.error_field_too_many_characters);
                return false;
            }

            if (Convert.ToInt64(amount) < 1)
            {
                View.SetEditFieldError(Resources.UiMessageResources.error_field_zero_amount);
                return false;
            }

            if (Convert.ToInt64(amount) > MyBalance)
            {
                View.SetEditFieldError(Resources.UiMessageResources.error_field_not_enough_money);
                return false;
            }

            return true;
        }

        #region Private Methods

        private void UpdateBalance(DepositeTransaction result)
        {
            if (result == null) return;
            //Updating Balance in DataManager
            DataManager.Profile.Amount = DataManager.Profile.Amount + result.Amount;
            View.HideProgress();
            AlertService.ShowToastMessage(Resources.UiMessageResources.wallet_was_funded + result.Amount + " coins");

            //Updating balance on View
            View.UpdateBalance(DataManager.Profile.Amount.ToString());
        }

        /// <summary>
        /// API call to get DepositeTransaction Object
        /// </summary>
        /// <param name="scannedCode"></param>
        /// <returns></returns>
        private async Task<DepositeTransaction> GetMoney(Cash scannedCode)
        {
            if (NetworkUtils.IsConnected)
            {
                try
                {
                    View.ShowProgress(Resources.UiMessageResources.progress_loading);

                    return await RestApiService.GetOfflineCash(scannedCode);
                }
                catch (ApiNotAuthorizedException)
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
                View.HideProgress();
                AlertService.ShowToastMessage(Resources.UiMessageResources.internet_turned_off);
            }

            return null;
        }

        /// <summary>
        /// Api call to get Cash Object fro generating QR code based on it
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        private async Task<Cash> WithdrawMoney(string amount)
        {
            if (NetworkUtils.IsConnected)
            {
                try
                {
                    View.ShowProgress(Resources.UiMessageResources.progress_loading);

                    //Creating Object Amount based on Input amount
                    var amountJson = new Amount { Balance = amount };

                    var code = await RestApiService.WithdrawMoney(amountJson);

                    View.HideProgress();
                    //Send string built from received CashObject to View
                    //so it could generate QRCode
                    View.ShowImage(QrManager.ConvertCashObjectToString(code));

                    //Call Updated UserBalance From server and store it to the DataManager
                    DataManager.Profile = await RestApiService.GetUserAccountAsync();

                    //Updates user's balance on View from DataManager.Profile
                    View.UpdateBalance(MyBalance.ToString());

                }
                catch (ApiNotAuthorizedException)
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
                View.HideProgress();
                AlertService.ShowToastMessage(Resources.UiMessageResources.internet_turned_off);
            }

            return null;
        }

        #endregion
    }
}
