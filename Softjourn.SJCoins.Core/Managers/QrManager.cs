using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Softjourn.SJCoins.Core.API.Model;
using Softjourn.SJCoins.Core.Exceptions;

namespace Softjourn.SJCoins.Core.Managers
{
    public class QrManager
    {
        public async Task<Cash> GetCodeFromQr()
        {
            var result = await ScanPhoto();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Cash>(result.Text);
        }

        public async Task<ZXing.Result> ScanPhoto()
        {
            await CheckCameraPermissiomAsync();

            var scanner = new ZXing.Mobile.MobileBarcodeScanner();
            var result = await scanner.Scan();

            return result;
        }

        private async Task CheckCameraPermissiomAsync()
        {
            if (!IsCameraAvailable())
                throw new CameraException("Camera permission is denied");

            var permission = await ValidatePermissionAsync(Permission.Camera);

            if (permission == PermissionStatus.Unknown)
                permission = PermissionStatus.Granted;

            if (permission != PermissionStatus.Granted)
            {
                throw new CameraException("Camera permission is denied");
            }
        }

        private async Task CheckGalleryPermissionAsync()
        {
            var permission = await ValidatePermissionAsync(Permission.Storage);
            if (permission != PermissionStatus.Granted)
            {
                throw new CameraException("Gallery permission is denied");
            }
        }

        private async Task<PermissionStatus> ValidatePermissionAsync(Permission permission)
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
            if (status != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(permission);
                status = results[permission];
            }

            return status;
        }

        private bool IsCameraAvailable()
        {
            return CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported;
        }
    }
}
