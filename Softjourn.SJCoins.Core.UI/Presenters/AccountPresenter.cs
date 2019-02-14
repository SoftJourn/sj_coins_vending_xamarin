using System;
using System.Collections.Generic;
using Plugin.DeviceInfo;
using Plugin.DeviceInfo.Abstractions;
using Softjourn.SJCoins.Core.Common;
using Softjourn.SJCoins.Core.Common.Exceptions;
using Softjourn.SJCoins.Core.Common.Utils;
using Softjourn.SJCoins.Core.Models;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
    public class AccountPresenter : BasePresenter<IAccountView>
    {
        private List<AccountOption> OptionsList { get; }

        protected override void AvatarImageAcquired(byte[] receipt)
        {
            View.ImageAcquired(receipt);
        }

        public AccountPresenter()
        {
            OptionsList = new List<AccountOption>
            {
                new AccountOption(Constant.ProfileOptionsPurchase, Constant.ProfileOptionsPurchaseIconName),
                new AccountOption(Constant.ProfileOptionsReports, Constant.ProfileOptionsReportsIconName),
                new AccountOption(Constant.ProfileOptionsShareFunds, Constant.ProfileOptionsShareFundsIconName),
                new AccountOption(Constant.ProfileOptionsLogout, Constant.ProfileOptionsLogoutIconName)
            };

            if (!Settings.OnlyOneVendingMachine)
            {
                var accountOption = new AccountOption(Constant.ProfileOptionsSelectMachine,
                    Constant.ProfileOptionsSelectMachineIconName);
                OptionsList.Add(accountOption);
            }
        }

        /// <summary>
        /// Display account information
        /// </summary>
        public void OnStartLoadingPage() => View.SetAccountInfo(DataManager.Profile);

        public void GetImageFromServer() => GetAvatarImage(DataManager.Profile.Image);

        /// <summary>
        /// Returns List of Options taken from string resources
        /// </summary>
        /// <returns></returns>
        public List<AccountOption> GetOptionsList() => OptionsList;

        /// <summary>
        /// Shows dialog to select photo source (Camera ar Gallery)
        /// </summary>
        public void OnPhotoClicked()
        {
            var items = new List<string>
            {
                Resources.UiMessageResources.select_source_photo_camera,
                Resources.UiMessageResources.select_source_photo_gallery
            };

            var getPhotoFromCameraAction = new Action(GetPhotoFromCamera);
            var getPhotoFromGalleryAction = new Action(GetPhotoFromGallery);
            var getPhotoPathFromCameraAction = new Action(GetPhotoPathFromCamera);
            var getPhotoPathFromGalleryAction = new Action(GetPhotoPathFromGallery);

            if (CrossDeviceInfo.Current.Platform == Platform.Android)
                AlertService.ShowPhotoSelectorDialog(items,
                    getPhotoPathFromCameraAction, getPhotoPathFromGalleryAction);
            else
                AlertService.ShowPhotoSelectorDialog(items,
                    getPhotoFromCameraAction, getPhotoFromGalleryAction);
        }

        /// <summary>
        /// Navigate to given page from OptionsList
        /// </summary>
        /// <param name="item"></param>
        public void OnItemClick(string item)
        {
            switch (item)
            {
                case Constant.ProfileOptionsPurchase:
                    NavigationService.NavigateTo(NavigationPage.Purchase);
                    return;
                case Constant.ProfileOptionsReports:
                    NavigationService.NavigateTo(NavigationPage.Reports);
                    return;
                case Constant.ProfileOptionsPrivacyTerms:
                    NavigationService.NavigateTo(NavigationPage.PrivacyTerms);
                    return;
                case Constant.ProfileOptionsHelp:
                    NavigationService.NavigateTo(NavigationPage.Help);
                    return;
                case Constant.ProfileOptionsShareFunds:
                    ShowDialogForChoosingQrStrategy();
                    return;
                case Constant.ProfileOptionsSelectMachine:
                    NavigationService.NavigateTo(NavigationPage.SelectMachine);
                    return;
                case Constant.ProfileOptionsLogout:
                    LogOut();
                    return;
            }
        }

        /// <summary>
        /// Navigate to given page from OptionsList
        /// </summary>
        /// <param name="itemPosition"></param>
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
                case 2:
                    ShowDialogForChoosingQrStrategy();
                    return;
                case 3:
                    LogOut();
                    return;
                case 4:
                    NavigationService.NavigateTo(NavigationPage.SelectMachine);
                    return;
            }
        }

        /// <summary>
        /// Start call to send image to the server
        /// Is using by android platform only as on Android Presenter have only 
        /// photo path after capturing and actually byte array
        /// </summary>
        /// <param name="image">byte array of image</param>
        public void StoreAvatarOnServer(byte[] image) => SetAvatarImage(image);

        #region Private Methods

        /// <summary>
        /// Create List for Dialog of choosing QR strategies and make Actions from methods
        /// </summary>
        private void ShowDialogForChoosingQrStrategy()
        {
            var items = new List<string>
            {
                Resources.UiMessageResources.select_strategy_scanning_qr,
                Resources.UiMessageResources.select_strategy_generating_qr
            };

            var goToScanningQrAction = new Action(GoToScanningQr);
            var goToGeneratingQrAction = new Action(GoToGeneratingQr);

            AlertService.ShowQrSelectorDialog(items,
                goToScanningQrAction, goToGeneratingQrAction);
        }

        private void GoToScanningQr() =>
            NavigationService.NavigateTo(NavigationPage.ShareFuns, Constant.QrScreenScanningTag);

        private void GoToGeneratingQr() =>
            NavigationService.NavigateTo(NavigationPage.ShareFuns, Constant.QrScreenGeneratingTag);

        /// <summary>
        /// If internet is connected clear user's data
        /// call revoke token and navigate to LoginPage
        /// </summary>
        private async void LogOut()
        {
            if (NetworkUtils.IsConnected)
            {
                View.ShowProgress("Logout...");

                try
                {
                    await RestApiService.RevokeTokenAsync();

                    DataManager.Profile = null;
                    DataManager.Avatar = null;

                    Settings.ClearUserData();

                    NavigationService.NavigateToAsRoot(NavigationPage.Login);
                }
                catch (ApiNotAuthorizedException)
                {
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
                AlertService.ShowToastMessage(Resources.UiMessageResources.internet_turned_off);
            }
        }

        /// <summary>
        /// Gets photo from Camera
        /// For IOS use
        /// </summary>
        private async void GetPhotoFromCamera()
        {
            try
            {
                var photo = await PhotoManager.GetImageFromCameraAsync();
                if (photo != null)
                    View.ImageAcquiredPlugin(photo);
            }
            catch (CameraException e)
            {
                AlertService.ShowToastMessage(e.Message);
            }
        }

        /// <summary>
        /// Gets photo from Gallery
        /// For IOS use
        /// </summary>
        private async void GetPhotoFromGallery()
        {
            try
            {
                var photo = await PhotoManager.GetImageFromGalleryAsync();
                if (photo != null)
                    View.ImageAcquiredPlugin(photo);
            }
            catch (CameraException e)
            {
                AlertService.ShowToastMessage(e.Message);
            }
        }

        /// <summary>
        /// Gets photo path from Camera
        /// For Android use
        /// </summary>
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

        /// <summary>
        /// Gets photo from Gallery
        /// For Android use
        /// </summary>
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

        private async void SetAvatarImage(byte[] image)
        {
            if (NetworkUtils.IsConnected)
            {
                try
                {
                    await RestApiService.SetAvatarImage(image);

                    DataManager.Profile = await RestApiService.GetUserAccountAsync();
                    DataManager.Avatar = image;

                    AlertService.ShowMessageWithUserInteraction(string.Empty,
                        "Image was stored on server", string.Empty, null);
                }
                catch (ApiBadRequestException)
                {
                    AlertService.ShowMessageWithUserInteraction("Server Error", string.Empty,
                        Resources.UiMessageResources.btn_title_ok, null);
                }
                catch (Exception ex)
                {
                    AlertService.ShowToastMessage(ex.Message);
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }
            else
            {
                AlertService.ShowToastMessage(Resources.UiMessageResources.internet_turned_off);
            }
        }

        #endregion 
    }
}
