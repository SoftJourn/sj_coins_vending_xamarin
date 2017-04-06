using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Softjourn.SJCoins.iOS
{
	public class UIImageHelper
	{
		#region Constants
		private const float width = 200;
		private const float height = 200;
		#endregion

		#region Public methods
		public UIImage ScaleImage(UIImage image)
		{
			if (image != null)
			{
				var size = new CGSize(width, height);
				var rect = new CGRect(0, 0, width, height);

				UIGraphics.BeginImageContextWithOptions(size, false, 0);
				var context = UIGraphics.GetCurrentContext();

				// Images upside down so flip them
				var flipVertical = new CGAffineTransform(1, 0, 0, -1, 0, size.Height);
				context.ConcatCTM(flipVertical);

				context.DrawImage(rect, image.CGImage);

				var newImage = UIGraphics.GetImageFromCurrentImageContext();
				UIGraphics.EndImageContext();

				return newImage;
			}
			else
				return null;
		}

		public Byte[] BytesFromImage(UIImage image) => image != null ? new Byte[image.AsJPEG().Length] : null;
		#endregion
	}
}
