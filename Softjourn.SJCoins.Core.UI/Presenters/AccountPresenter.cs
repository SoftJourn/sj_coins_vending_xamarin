using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Plugin.DeviceInfo;
using Plugin.DeviceInfo.Abstractions;
using Softjourn.SJCoins.Core.Exceptions;
using Softjourn.SJCoins.Core.Managers;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Core.Utils;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
    public class AccountPresenter : BasePresenter<IAccountView>
    {
        private int _balance;

        public AccountPresenter()
        {
        }

        public void OnStartLoadingPage()
        {
            // Display account information
            View.SetAccountInfo(DataManager.Profile);
        }

        public List<string> GetOptionsList()
        {
            var items = Resources.StringResources.profile_options_array.Split("|".ToCharArray());
            return items.ToList();
        }

        public async Task OnPhotoClicked()
        {
            var items = new List<string>();
            items.Add(Resources.StringResources.select_source_photo_camera);
            items.Add(Resources.StringResources.select_source_photo_gallery);

            var getPhotoFromCameraAction = new Action(GetPhotoFromCamera);
            var getPhotoFromGalleryAction = new Action(GetPhotoFromGallery);

            AlertService.ShowPhotoSelectorDialog(items, getPhotoFromCameraAction, getPhotoFromGalleryAction);
        }

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
    }
}
