using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Softjourn.SJCoins.Core.Utils;

namespace Softjourn.SJCoins.Core.Managers
{
    public sealed class PhotoManager
    {
        /// <summary>
        /// Returns byte array of captured photo
        /// </summary>
        /// <returns></returns>
        public async Task<byte[]> GetImageFromCameraAsync()
        {
            await PermissionsUtils.CheckCameraPermissiomAsync();

            var result = await MakePhotoAsync();

            return result == null
                ? null
                : GetBytes(result);
        }

        /// <summary>
        /// Returns byte array of selected from gallery photo
        /// </summary>
        /// <returns></returns>
        public async Task<byte[]> GetImageFromGalleryAsync()
        {
            await PermissionsUtils.CheckGalleryPermissionAsync();

            var result = await PickPhotoFromGalleryAsync();

            return result == null
                ? null
                : GetBytes(result);
        }

        /// <summary>
        /// Returns Path to captured Photo
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetImagePathFromCameraAsync()
        {
            await PermissionsUtils.CheckCameraPermissiomAsync();

            var result = await MakePhotoAsync();

            return result?.Path;
        }

        /// <summary>
        /// Returns path to selected from gallery photo
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetImagePathFromGalleryAsync()
        {
            await PermissionsUtils.CheckGalleryPermissionAsync();

            var result = await PickPhotoFromGalleryAsync();

            return result?.Path;
        }

        #region Private Methods

        /// <summary>
        /// Starts Standard Camera and returns MediaFile of captured photo
        /// </summary>
        /// <returns></returns>
        private static async Task<MediaFile> MakePhotoAsync()
        {
            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions());

            return file;
        }

        /// <summary>
        /// Starts Gallery and returns MediaFile of selected photo
        /// </summary>
        /// <returns></returns>
        private static async Task<MediaFile> PickPhotoFromGalleryAsync()
        {
            var file = await CrossMedia.Current.PickPhotoAsync();

            return file;
        }

        /// <summary>
        /// Creating byte array based on MediaFile (Captured or selected in gallery photo)
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
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
