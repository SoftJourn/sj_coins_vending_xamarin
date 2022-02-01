using System.Threading.Tasks;
using Softjourn.SJCoins.Core.Utils;
using Xamarin.Essentials;

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

            return result == null ? null : await GetBytes(result);
        }

        //Returns byte array of selected from gallery photo
        public async Task<byte[]> GetImageFromGalleryAsync()
        {
            await PermissionsUtils.CheckGalleryPermissionAsync();

            var result = await PickPhotoFromGalleryAsync();

            return result == null ? null : await GetBytes(result);
        }

        //Returns Path to captured Photo
        public async Task<string> GetImagePathFromCameraAsync()
        {
            await PermissionsUtils.CheckCameraPermissiomAsync();

            var result = await MakePhotoAsync();

            return result?.FullPath;
        }

        //Returns path to selected from gallery photo
        public async Task<string> GetImagePathFromGalleryAsync()
        {
            await PermissionsUtils.CheckGalleryPermissionAsync();

            var result = await PickPhotoFromGalleryAsync();

            return result?.FullPath;
        }
        #endregion

        #region Private Methods
        //Starts Standard Camera and returns MediaFile of captured photo
        private async Task<FileResult> MakePhotoAsync()
        {
            
            var file = await MediaPicker.CapturePhotoAsync();

            return file;
        }

        //Starts Gallery and returns MediaFile of selected photo
        private async Task<FileResult> PickPhotoFromGalleryAsync()
        {
            var file = await MediaPicker.PickPhotoAsync();

            return file;
        }

        //Creating byte array based on MediaFile (Captured or selected in gallery photo)
        private static async Task<byte[]> GetBytes(FileResult file)
        {
            await using var stream = await file.OpenReadAsync();
            var buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);
            return buffer;
        }
        #endregion
    }
}
