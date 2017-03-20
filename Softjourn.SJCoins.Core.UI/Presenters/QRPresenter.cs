using System;
using System.Threading.Tasks;
using Autofac;
using Softjourn.SJCoins.Core.API.Model;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Core.Exceptions;
using Softjourn.SJCoins.Core.Helpers;
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
            set { }
        }

        private QrManager QrManager;

        #region Constructor
        public QrPresenter()
        {
            Scope = BaseBootstrapper.Container.BeginLifetimeScope();
            QrManager = Scope.Resolve<QrManager>();
        }
        #endregion

        #region Public Methods
        //Calls Method of ZXing scanning to get code
        //and if success call API method to proceed transaction
        public async void ScanCode()
        {
            try
            {
                var code = await QrManager.GetCodeFromQr();
                if (code != null)
                    UpdateBalance(await GetMoney(code));
            }
            catch (JsonReaderExceptionCustom e)
            {
                AlertService.ShowToastMessage(Resources.StringResources.error_not_valid_qr_code);
            }
            catch (CameraException e)
            {
                AlertService.ShowToastMessage(e.ToString());
            }
        }

		// Using only on IOS platform
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

        //If amount is valid call API method for creating transaction
        public async void GenerateCode(string amount)
        {
            if (ValidateAmount(amount))
            {
                await WithdrawMoney(amount);
            }
        }

        //Return Current Balance from DataManager
        public int GetBalance()
        {
            return MyBalance;
        }

		public async Task CheckPermission()
		{
			await PermissionsUtils.CheckCameraPermissiomAsync();
		}

		//Return true if amount is not empty, is integer and not exceeds user's balance
		public bool ValidateAmount(string amount)
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

            if (amount.Length > 10)
            {
                View.SetEditFieldError(Resources.StringResources.error_field_too_many_characters);
                return false;
            }

            if (Convert.ToInt64(amount) > MyBalance)
		    {
		        View.SetEditFieldError(Resources.StringResources.error_field_not_enough_money);
		        return false;
		    }
            
            return true;
		}
		#endregion

		#region Private Methods
		private void UpdateBalance(DepositeTransaction result)
		{
		    if (result == null) return;
		    //Updating Balance in DataManager
		    DataManager.Profile.Amount = DataManager.Profile.Amount + result.Amount;
		    View.HideProgress();
		    AlertService.ShowToastMessage(Resources.StringResources.wallet_was_funded + result.Amount + " coins");

		    //Updating balance on View
		    View.UpdateBalance(DataManager.Profile.Amount.ToString());
		}

        //API call to get DepositeTransaction Object
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
                AlertService.ShowToastMessage(Resources.StringResources.internet_turned_off);
            }
            return null;
        }

        //Api call to get Cash Object fro generating QR code based on it
        private async Task<Cash> WithdrawMoney(string amount)
        {
            if (NetworkUtils.IsConnected)
            {
                try
                {
                    View.ShowProgress(Resources.StringResources.progress_loading);

                    //Creating Object Amount based on Input amount
                    var amountJson = new Amount { Balance = amount };

                    var code = await RestApiServise.WithdrawMoney(amountJson);

                    View.HideProgress();
                    //Send string built from received CashObject to View
                    //so it could generate QRCode
                    View.ShowImage(QrManager.ConvertCashObjectToString(code));

                    //Call Updated UserBalance From server and store it to the DataManager
                    DataManager.Profile = await RestApiServise.GetUserAccountAsync();

                    //Updates user's balance on View from DataManager.Profile
                    View.UpdateBalance(MyBalance.ToString());

                }
                catch (ApiNotAuthorizedException ex)
                {
                    View.HideProgress();
                    AlertService.ShowToastMessage(ex.Message);
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
                AlertService.ShowToastMessage(Resources.StringResources.internet_turned_off);
            }
            return null;
        }
        #endregion
    }
}
