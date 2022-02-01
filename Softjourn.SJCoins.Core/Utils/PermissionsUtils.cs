
using System.Text;
using System.Threading.Tasks;
using Softjourn.SJCoins.Core.Exceptions;
using Xamarin.Essentials;

namespace Softjourn.SJCoins.Core.Utils
{
    public class PermissionsUtils
    {
        public static async Task CheckCameraPermissiomAsync()
        {
            if (!IsCameraAvailable())
                throw new CameraException("Camera permission is denied");
            
            var permission = await ValidatePermissionAsync<Permissions.Camera>();

            if (permission == PermissionStatus.Unknown)
                permission = PermissionStatus.Granted;

            if (permission != PermissionStatus.Granted)
            {
                throw new CameraException("Camera permission is denied");
            }
        }

        public static async Task CheckGalleryPermissionAsync()
        {
            var permission = await ValidatePermissionAsync<Permissions.StorageRead>();
            if (permission != PermissionStatus.Granted)
            {
                throw new CameraException("Gallery permission is denied");
            }
        }

        private static async Task<PermissionStatus> ValidatePermissionAsync<T>() where T : Permissions.BasePermission, new()
        {
            
            var status = await Permissions.CheckStatusAsync<T>();
            if (status != PermissionStatus.Granted)
                status = await Permissions.RequestAsync<T>();

            return status;
        }

        private static bool IsCameraAvailable()
        {
            return MediaPicker.IsCaptureSupported;
        }
    }
}
