using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Softjourn.SJCoins.Core.Common.Exceptions;

namespace Softjourn.SJCoins.Core.Common.Utils
{
    public static class PermissionsUtils
    {
        public static async Task CheckCameraPermissionAsync()
        {
            if (!IsCameraAvailable())
                throw new CameraException("Camera permission is denied");

            var permission = await ValidatePermissionAsync(Permission.Camera);

            if (permission == PermissionStatus.Unknown)
                permission = PermissionStatus.Granted;

            if (permission != PermissionStatus.Granted)
                throw new CameraException("Camera permission is denied");
        }

        public static async Task CheckGalleryPermissionAsync()
        {
            var permission = await ValidatePermissionAsync(Permission.Storage);
            if (permission != PermissionStatus.Granted)
                throw new CameraException("Gallery permission is denied");
        }

        private static async Task<PermissionStatus> ValidatePermissionAsync(Permission permission)
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
            if (status != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(permission);
                status = results[permission];
            }

            return status;
        }

        private static bool IsCameraAvailable()
        {
            return CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported;
        }
    }
}
