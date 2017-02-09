using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Softjourn.SJCoins.Core.Exceptions;

namespace Softjourn.SJCoins.Core.Managers
{
    public class PhotoManager
    {
        public bool IsCameraAvailable()
        {
            return CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported;
        }

        public async Task<byte[]> GetImageFromCameraAsync()
        {
            await CheckCameraPermissiomAsync();

            var result = await MakePhotoAsync();

            return result == null ? null : GetBytes(result);
        }

        public async Task<byte[]> GetImageFromGalleryAsync()
        {
            await CheckGalleryPermissionAsync();

            var result = await PickPhotoFromGalleryAsync();

            return result == null ? null : GetBytes(result);
        }

        public async Task<string> GetImagePathFromCameraAsync()
        {
            await CheckCameraPermissiomAsync();

            var result = await MakePhotoAsync();

            return result?.Path;
        }

        public async Task<string> GetImagePathFromGalleryAsync()
        {
            await CheckGalleryPermissionAsync();

            var result = await PickPhotoFromGalleryAsync();

            return result?.Path;
        }

        protected async Task<MediaFile> MakePhotoAsync()
        {
            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions());

            return file;
        }

        protected async Task<MediaFile> PickPhotoFromGalleryAsync()
        {
            var file = await CrossMedia.Current.PickPhotoAsync();

            return file;
        }

        private static byte[] GetBytes(MediaFile file)
        {
            using (var stream = file.GetStream())
            {
                var buffer = new byte[stream.Length];
                stream.Read(buffer, 0, (int)stream.Length);
                return buffer;
            }
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
    }
}
