using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.DeviceInfo;
using Plugin.DeviceInfo.Abstractions;
using Softjourn.SJCoins.Core.API.Model;
using Softjourn.SJCoins.Core.Exceptions;
using Softjourn.SJCoins.Core.Helpers;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Core.Utils;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
    public class AccountPresenter : BasePresenter<IAccountView>
    {
		private const int oneMb = 1048576;

        private List<AccountOption> OptionsList { get; set; }

        public AccountPresenter()
        {
            OptionsList = new List<AccountOption>();
            OptionsList.Add(new AccountOption(Const.ProfileOptionsPurchase, Const.ProfileOptionsPurchaseIconName));
            OptionsList.Add(new AccountOption(Const.ProfileOptionsReports, Const.ProfileOptionsReportsIconName));
            //OptionsList.Add(new AccountOption(Const.ProfileOptionsPrivacyTerms, Const.ProfileOptionsPrivacyTermsIconName));
            //OptionsList.Add(new AccountOption(Const.ProfileOptionsHelp, Const.ProfileOptionsHelpIconName));
            OptionsList.Add(new AccountOption(Const.ProfileOptionsShareFunds, Const.ProfileOptionsShareFundsIconName));
            OptionsList.Add(new AccountOption(Const.ProfileOptionsSelectMachine,Const.ProfileOptionsSelectMachineIconName));
            OptionsList.Add(new AccountOption(Const.ProfileOptionsLogout, Const.ProfileOptionsLogoutIconName));
        }

        #region Public Methods
        public void OnStartLoadingPage()
        {
            // Display account information
            View.SetAccountInfo(DataManager.Profile);            
        }

        public void GetImageFromServer()
        {
            GetAvatarImage(DataManager.Profile.Image);
        }

        //Returns List of Options taken from string resources
        public List<AccountOption> GetOptionsList()
        {
            return OptionsList;
        }

        //Shows dialog to select photo source (Camera ar Gallery)
        public async Task OnPhotoClicked()
        {
            var items = new List<string>
            {
                Resources.StringResources.select_source_photo_camera,
                Resources.StringResources.select_source_photo_gallery
            };

            var getPhotoFromCameraAction = new Action(GetPhotoFromCamera);
            var getPhotoFromGalleryAction = new Action(GetPhotoFromGallery);

            var getPhotoPathFromCameraAction = new Action(GetPhotoPathFromCamera);
            var getPhotoPathFromGalleryAction = new Action(GetPhotoPathFromGallery);

            if (CrossDeviceInfo.Current.Platform == Platform.Android)
            {
                AlertService.ShowPhotoSelectorDialog(items, getPhotoPathFromCameraAction, getPhotoPathFromGalleryAction);
            }
            else
            {
                AlertService.ShowPhotoSelectorDialog(items, getPhotoFromCameraAction, getPhotoFromGalleryAction);
            }
        }

        //Navigate to given page from OptionsList
        public void OnItemClick(string item)
        {
            switch (item)
            {
                case Const.ProfileOptionsPurchase:
                    NavigationService.NavigateTo(NavigationPage.Purchase);
                    return;
                case Const.ProfileOptionsReports:
                    NavigationService.NavigateTo(NavigationPage.Reports);
                    return;
                case Const.ProfileOptionsPrivacyTerms:
                    NavigationService.NavigateTo(NavigationPage.PrivacyTerms);
                    return;
                case Const.ProfileOptionsHelp:
                    NavigationService.NavigateTo(NavigationPage.Help);
                    return;
                case Const.ProfileOptionsShareFunds:
                    ShowDialogForChoosingQrStrategy();
                    return;
                case Const.ProfileOptionsSelectMachine:
                    NavigationService.NavigateTo(NavigationPage.SelectMachine);
                    return;
                case Const.ProfileOptionsLogout:
                    LogOut();
                    return;
            }
        }

        //Navigate to given page from OptionsList
        public void OnItemClick(int itemPosition)
        {
            switch (itemPosition)
            {
                case 0:
                    NavigationService.NavigateTo(NavigationPage.Purchase);
                    return;
                case 1:
                    NavigationService.NavigateTo(NavigationPage.Reports);
                    return;
                //case 2:
                //    NavigationService.NavigateTo(NavigationPage.PrivacyTerms);
                //    return;
                //case 3:
                //    NavigationService.NavigateTo(NavigationPage.Help);
                //    return;
                case 2:
                    ShowDialogForChoosingQrStrategy();
                    return;
                case 3:
                    NavigationService.NavigateTo(NavigationPage.SelectMachine);
                    return;
                case 4:
                    LogOut();
                    return;
            }
        }

        /// <summary>
        /// Start call to send image to the server
        /// Is using by android platform only as on Android Presenter have only 
        /// photo path after capturing and actually byte array
        /// </summary>
        /// <param name="image">byte array of image</param>
        public void StoreAvatarOnServer(byte[] image)
        {
            SetAvatarImage(image);
        }
        #endregion

        #region Private Methods

        //Create List for Dialog of choosing QR strategies and make Actions from methods
        private void ShowDialogForChoosingQrStrategy()
        {
            var items = new List<string>
            {
                Resources.StringResources.select_strategy_scanning_qr,
                Resources.StringResources.select_strategy_generating_qr
            };

            var goToScanningQrAction = new Action(GoToScanningQr);
            var goToGeneratingQrAction = new Action(GoToGeneratingQr);

            AlertService.ShowQrSelectorDialog(items, goToScanningQrAction, goToGeneratingQrAction);
        }

        private void GoToScanningQr()
        {
            NavigationService.NavigateTo(NavigationPage.ShareFuns, Const.QrScreenScanningTag);
        }

        private void GoToGeneratingQr()
        {
            NavigationService.NavigateTo(NavigationPage.ShareFuns, Const.QrScreenGeneratingTag);
        }

        //If internet is connected clear user's data
        //call revoke token and navigate to LoginPage
        private async void LogOut()
        {
            if (NetworkUtils.IsConnected)
            {
                try
                {
                    await RestApiServise.RevokeTokenAsync();
                    DataManager.Profile = null;
                    Settings.ClearUserData();
                    NavigationService.NavigateToAsRoot(NavigationPage.Login);
                }
                catch (ApiNotAuthorizedException ex)
                {
                    AlertService.ShowToastMessage(ex.Message);
                    DataManager.Profile = null;
                    Settings.ClearUserData();
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
        }

        //Gets photo from Camera
        private async void GetPhotoFromCamera()
        {
            try
            {
                var photo = await PhotoManager.GetImageFromCameraAsync();

                if (photo != null)
                    View.ImageAcquired(photo);
                    SetAvatarImage(photo);
            }
            catch (CameraException e)
            {
                AlertService.ShowToastMessage(e.Message);
            }
        }

        //Gets photo from Gallery
        private async void GetPhotoFromGallery()
        {
            try
            {
                var photo = await PhotoManager.GetImageFromGalleryAsync();

                if (photo != null)
                    View.ImageAcquired(photo);
                    SetAvatarImage(photo);
            }
            catch (CameraException e)
            {
                AlertService.ShowToastMessage(e.Message);
            }
        }

        //Gets photo path from Camera
        //For Android use
        private async void GetPhotoPathFromCamera()
        {
            try
            {
                var photo = await PhotoManager.GetImagePathFromCameraAsync();

                if (photo != null)
                    View.ImageAcquired(photo);
            }
            catch (CameraException e)
            {
                AlertService.ShowToastMessage(e.Message);
            }
        }

        //Gets photo from Gallery
        //For Android use
        private async void GetPhotoPathFromGallery()
        {
            try
            {
                var photo = await PhotoManager.GetImagePathFromGalleryAsync();

                if (photo != null)
                    View.ImageAcquired(photo);
            }
            catch (CameraException e)
            {
                AlertService.ShowToastMessage(e.Message);
            }
        }

        private async void GetAvatarImage(string endpoint)
        {
            if (NetworkUtils.IsConnected)
            {
                try
                {
					byte[] image;
					if (DataManager.Avatar == null)
					{
						// Avatar not stored
						image = await RestApiServise.GetAvatarImage(endpoint.Substring(1));

						if (image.Length < oneMb)
						{
							// Store in data manager
							DataManager.Avatar = image;
						}
					}
					else
						// Avatar stored
						image = DataManager.Avatar;

					View.ImageAcquired(image);
                }
                catch (ApiBadRequestException ex)
                {
                    AlertService.ShowMessageWithUserInteraction("", Resources.StringResources.server_error_bad_username_or_password, Resources.StringResources.btn_title_ok, null);
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
        }

        private async void SetAvatarImage(byte[] image)
        {
            if (NetworkUtils.IsConnected)
            {

                try
                {
                    await RestApiServise.SetAvatarImage(image);
                    DataManager.Profile = await RestApiServise.GetUserAccountAsync();
					DataManager.Avatar = image;
                    AlertService.ShowMessageWithUserInteraction("","Image was stored on server","",null);                  
                }
                catch (ApiBadRequestException ex)
                {
                    AlertService.ShowMessageWithUserInteraction("Server Error", "", Resources.StringResources.btn_title_ok, null);
                }
                catch (Exception ex)
                {
                    AlertService.ShowToastMessage(ex.Message);
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }
            else
            {
                AlertService.ShowToastMessage(Resources.StringResources.internet_turned_off);
            }
        }
        #endregion
    }
}
