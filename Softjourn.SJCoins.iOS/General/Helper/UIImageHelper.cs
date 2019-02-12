using System;
using System.Drawing;
using System.IO;
using CoreGraphics;
using UIKit;

namespace Softjourn.SJCoins.iOS.General.Helper
{
    public class UIImageHelper
    {
        private const int KMaxResolution = 1024;

        public byte[] BytesFromImage(UIImage image)
        {
            if (image != null)
            {
                var imageStream = image.AsJPEG(0).AsStream();
                using (var memStream = new MemoryStream())
                {
                    imageStream.CopyTo(memStream);
                    return memStream.ToArray();
                }
            }

            return null;
        }

        public UIImage ScaleAndRotateImage(UIImage imageIn, UIImageOrientation orIn)
        {
            if (imageIn != null)
            {
                var imgRef = imageIn.CGImage;
                float width = imgRef.Width;
                float height = imgRef.Height;
                var transform = CGAffineTransform.MakeIdentity();
                var bounds = PrepareRectangle(width, height);

                var scaleRatio = bounds.Width / width;
                var imageSize = new SizeF(width, height);

                var orient = orIn;
                float boundHeight;

                switch (orient)
                {
                    case UIImageOrientation.Up:                                        //EXIF = 1
                        transform = CGAffineTransform.MakeIdentity();
                        break;

                    case UIImageOrientation.UpMirrored:                                //EXIF = 2
                        transform = CGAffineTransform.MakeTranslation(imageSize.Width, 0f);
                        transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
                        break;

                    case UIImageOrientation.Down:                                      //EXIF = 3
                        transform = CGAffineTransform.MakeTranslation(imageSize.Width, imageSize.Height);
                        transform = CGAffineTransform.Rotate(transform, (float)Math.PI);
                        break;

                    case UIImageOrientation.DownMirrored:                              //EXIF = 4
                        transform = CGAffineTransform.MakeTranslation(0f, imageSize.Height);
                        transform = CGAffineTransform.MakeScale(1.0f, -1.0f);
                        break;

                    case UIImageOrientation.LeftMirrored:                              //EXIF = 5
                        boundHeight = bounds.Height;
                        bounds.Height = bounds.Width;
                        bounds.Width = boundHeight;
                        transform = CGAffineTransform.MakeTranslation(imageSize.Height, imageSize.Width);
                        transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
                        transform = CGAffineTransform.Rotate(transform, 3.0f * (float)Math.PI / 2.0f);
                        break;

                    case UIImageOrientation.Left:                                      //EXIF = 6
                        boundHeight = bounds.Height;
                        bounds.Height = bounds.Width;
                        bounds.Width = boundHeight;
                        transform = CGAffineTransform.MakeTranslation(0.0f, imageSize.Width);
                        transform = CGAffineTransform.Rotate(transform, 3.0f * (float)Math.PI / 2.0f);
                        break;

                    case UIImageOrientation.RightMirrored:                             //EXIF = 7
                        boundHeight = bounds.Height;
                        bounds.Height = bounds.Width;
                        bounds.Width = boundHeight;
                        transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
                        transform = CGAffineTransform.Rotate(transform, (float)Math.PI / 2.0f);
                        break;

                    case UIImageOrientation.Right:                                     //EXIF = 8
                        boundHeight = bounds.Height;
                        bounds.Height = bounds.Width;
                        bounds.Width = boundHeight;
                        transform = CGAffineTransform.MakeTranslation(imageSize.Height, 0.0f);
                        transform = CGAffineTransform.Rotate(transform, (float)Math.PI / 2.0f);
                        break;

                    default:
                        throw new Exception("Invalid image orientation");
                }

                UIGraphics.BeginImageContext(bounds.Size);

                var context = UIGraphics.GetCurrentContext();

                if (orient == UIImageOrientation.Right || orient == UIImageOrientation.Left)
                {
                    context.ScaleCTM(-scaleRatio, scaleRatio);
                    context.TranslateCTM(-height, 0);
                }
                else
                {
                    context.ScaleCTM(scaleRatio, -scaleRatio);
                    context.TranslateCTM(0, -height);
                }

                context.ConcatCTM(transform);
                context.DrawImage(new RectangleF(0, 0, width, height), imgRef);

                var imageCopy = UIGraphics.GetImageFromCurrentImageContext();
                UIGraphics.EndImageContext();

                return imageCopy;
            }

            return null;
        }

        public UIImage optimizedImage(UIImage imageIn)
        {
            if (imageIn != null)
            {
                var imgRef = imageIn.CGImage;
                float width = imgRef.Width;
                float height = imgRef.Height;
                var bounds = PrepareRectangle(width, height);

                var imageSize = new SizeF(width, height);

                UIGraphics.BeginImageContextWithOptions(new CGSize(width, height), true, 1.0f); //BeginImageContext(bounds.Size);
                var context = UIGraphics.GetCurrentContext();
                context.DrawImage(new RectangleF(0, 0, width, height), imgRef);
                var imageCopy = UIGraphics.GetImageFromCurrentImageContext();
                UIGraphics.EndImageContext();

                return imageCopy;
            }

            return null;
        }

        public UIImage GetColoredImage(UIColor color)
        {
            var rect = new RectangleF(0, 0, 1.0f, 1.0f);
            UIGraphics.BeginImageContext(rect.Size);
            var context = UIGraphics.GetCurrentContext();

            context.SetFillColor(color.CGColor);
            context.FillRect(rect);

            var coloredImage = UIGraphics.GetImageFromCurrentImageContext();

            UIGraphics.EndImageContext();

            return coloredImage;
        }

        #region Private methods

        private static RectangleF PrepareRectangle(float width, float height)
        {
            var bounds = new RectangleF(0, 0, width, height);

            if (width > KMaxResolution || height > KMaxResolution)
            {
                var ratio = width / height;

                if (ratio > 1)
                {
                    bounds.Width = KMaxResolution;
                    bounds.Height = bounds.Width / ratio;
                }
                else
                {
                    bounds.Height = KMaxResolution;
                    bounds.Width = bounds.Height * ratio;
                }
            }

            return bounds;
        }

        #endregion
    }
}
