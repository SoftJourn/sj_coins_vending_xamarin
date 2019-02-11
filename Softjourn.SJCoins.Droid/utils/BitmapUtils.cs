
using Android.Graphics;
using Android.Media;

namespace Softjourn.SJCoins.Droid.Utils
{
    public class BitmapUtils
    {
        public static int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            var height = options.OutHeight;
            var width = options.OutWidth;
            var inSampleSize = 1;

            if (height <= reqHeight && width <= reqWidth) return inSampleSize;

            var halfHeight = height / 2;
            var halfWidth = width / 2;

            while ((halfHeight / inSampleSize) > reqHeight
                   && (halfWidth / inSampleSize) > reqWidth)
            {
                inSampleSize *= 2;
            }

            return inSampleSize;
        }

        public static Bitmap RotateIfNeeded(Bitmap bmp, string path)
        {
            var ei = new ExifInterface(path);
            var orientation = ei.GetAttributeInt(ExifInterface.TagOrientation, -1);

            switch (orientation)
            {
                case 6: //portrait
                    return RotateImage(bmp, 90);
                case 3: //Landscape
                    return RotateImage(bmp, 180);
                case 8: //Selfie ORIENTATION_ROTATE_270 - might need to flip horizontally too...
                    return RotateImage(bmp, 270);
                default:
                    return bmp;
            }
        }

        private static Bitmap RotateImage(Bitmap img, int degree)
        {
            var matrix = new Matrix();
            matrix.PostRotate(degree);
            var rotatedImg = Bitmap.CreateBitmap(img, 0, 0, img.Width, img.Height, matrix, true);

            return rotatedImg;
        }
    }
}