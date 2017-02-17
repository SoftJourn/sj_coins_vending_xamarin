using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Plugin.DeviceInfo;
using Plugin.DeviceInfo.Abstractions;
using Softjourn.SJCoins.Core.Exceptions;
using Softjourn.SJCoins.Core.Helpers;
using Softjourn.SJCoins.Core.Managers;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Core.Utils;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
    public class AccountPresenter : BasePresenter<IAccountView>
    {
        private List<string> OptionsList { get; set; }

        public AccountPresenter()
        {
            OptionsList = new List<string>
            {
                Const.ProfileOptionsPurchase,
                Const.ProfileOptionsReports,
                Const.ProfileOptionsPrivacyTerms,
                Const.ProfileOptionsHelp,
                Const.ProfileOptionsShareFuns,
                Const.ProfileOptionsSelectMachine,
                Const.ProfileOptionsLogout
            };
        }

        #region Public Methods
        public void OnStartLoadingPage()
        {
            // Display account information
            View.SetAccountInfo(DataManager.Profile);
        }

        //Returns List of Options taken from string resources
        public List<string> GetOptionsList()
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

            AlertService.ShowPhotoSelectorDialog(items, getPhotoFromCameraAction, getPhotoFromGalleryAction);
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
                case Const.ProfileOptionsShareFuns:
                    NavigationService.NavigateTo(NavigationPage.ShareFuns);
                    return;
                case Const.ProfileOptionsSelectMachine:
                    NavigationService.NavigateTo(NavigationPage.SelectMachine);
                    return;
                case Const.ProfileOptionsLogout:
                    LogOut();
                    return;
            }
        }
        #endregion

        #region Private Methods

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
            }
            catch (CameraException e)
            {
                AlertService.ShowToastMessage(e.ToString());
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
            }
            catch (CameraException e)
            {
                AlertService.ShowToastMessage(e.ToString());
            }
        }
        #endregion
    }
}
