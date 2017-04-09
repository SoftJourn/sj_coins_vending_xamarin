using System;
using System.Drawing;
using System.IO;
using CoreGraphics;
using UIKit;

namespace Softjourn.SJCoins.iOS
{
	public class UIImageHelper
	{
		#region Constants
		private const int kMaxResolution = 200;
		//private const float width = 200;
		//private const float height = 200;
		#endregion

		#region Public methods
		//public UIImage ScaleImage(UIImage image)
		//{
		//	if (image != null)
		//	{
		//		var size = new CGSize(width, height);
		//		var rect = new CGRect(0, 0, width, height);

		//		UIGraphics.BeginImageContextWithOptions(size, false, 0);
		//		var context = UIGraphics.GetCurrentContext();

		//		// Images upside down so flip them
		//		var flipVertical = new CGAffineTransform(1, 0, 0, -1, 0, size.Height);
		//		context.ConcatCTM(flipVertical);

		//		context.DrawImage(rect, image.CGImage);

		//		var newImage = UIGraphics.GetImageFromCurrentImageContext();
		//		UIGraphics.EndImageContext();

		//		return newImage;
		//	}
		//	else
		//		return null;
		//}

		public Byte[] BytesFromImage(UIImage image)
		{
			if (image != null)
			{
				var imageStream = image.AsJPEG(0).AsStream();
				using (MemoryStream memStream = new MemoryStream())
				{
					imageStream.CopyTo(memStream);
					return memStream.ToArray();
				}
			}
			else
				return null;
		}

		public UIImage ScaleAndRotateImage(UIImage imageIn, UIImageOrientation orIn)
		{
			if (imageIn != null)
			{
				CGImage imgRef = imageIn.CGImage;
				float width = imgRef.Width;
				float height = imgRef.Height;
				CGAffineTransform transform = CGAffineTransform.MakeIdentity();
				RectangleF bounds = PrepareRectangle(width, height);

				float scaleRatio = bounds.Width / width;
				SizeF imageSize = new SizeF(width, height);

				UIImageOrientation orient = orIn;
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
						break;
				}

				UIGraphics.BeginImageContext(bounds.Size);

				CGContext context = UIGraphics.GetCurrentContext();

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

				UIImage imageCopy = UIGraphics.GetImageFromCurrentImageContext();
				UIGraphics.EndImageContext();

				return imageCopy;
			}
			else
				return null;
		}

		private RectangleF PrepareRectangle(float width, float height)
		{
			var bounds = new RectangleF(0, 0, width, height);

				if (width > kMaxResolution || height > kMaxResolution)
				{
					float ratio = width / height;

					if (ratio > 1)
					{
						bounds.Width = kMaxResolution;
						bounds.Height = bounds.Width / ratio;
					}
					else
					{
						bounds.Height = kMaxResolution;
						bounds.Width = bounds.Height* ratio;
					}
				}
			return bounds;
		}
		#endregion
	}
}
