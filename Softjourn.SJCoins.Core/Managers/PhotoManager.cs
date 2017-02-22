using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Softjourn.SJCoins.Core.Utils;

namespace Softjourn.SJCoins.Core.Managers
{
    public class PhotoManager
    {

        #region Public Methods
        //Returns byte array of captured photo
        public async Task<byte[]> GetImageFromCameraAsync()
        {
            await PermissionsUtils.CheckCameraPermissiomAsync();

            var result = await MakePhotoAsync();

            return result == null ? null : GetBytes(result);
        }

        //Returns byte array of selected from gallery photo
        public async Task<byte[]> GetImageFromGalleryAsync()
        {
            await PermissionsUtils.CheckGalleryPermissionAsync();

            var result = await PickPhotoFromGalleryAsync();

            return result == null ? null : GetBytes(result);
        }

        //Returns Path to captured Photo
        public async Task<string> GetImagePathFromCameraAsync()
        {
            await PermissionsUtils.CheckCameraPermissiomAsync();

            var result = await MakePhotoAsync();

            return result?.Path;
        }

        //Returns path to selected from gallery photo
        public async Task<string> GetImagePathFromGalleryAsync()
        {
            await PermissionsUtils.CheckGalleryPermissionAsync();

            var result = await PickPhotoFromGalleryAsync();

            return result?.Path;
        }
        #endregion

        #region Private Methods
        //Starts Standard Camera and returns MediaFile of captured photo
        private async Task<MediaFile> MakePhotoAsync()
        {
            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions());

            return file;
        }

        //Starts Gallery and returns MediaFile of selected photo
        private async Task<MediaFile> PickPhotoFromGalleryAsync()
        {
            var file = await CrossMedia.Current.PickPhotoAsync();

            return file;
        }

        //Creating byte array based on MediaFile (Captured or selected in gallery photo)
        private static byte[] GetBytes(MediaFile file)
        {
            using (var stream = file.GetStream())
            {
                var buffer = new byte[stream.Length];
                stream.Read(buffer, 0, (int)stream.Length);
                return buffer;
            }
        }
        #endregion
    }
}
