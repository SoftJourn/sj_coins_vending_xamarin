using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Softjourn.SJCoins.iOS.General.Helper
{
	public class SegmentControlHelper
	{
		private const float defaultFontSize = 16.0f;
		private const int distanceBetweenElements = 5;
		private const int half = 2;

        /// <summary>
        /// Create a UIImage from an icon file and string for use in a UISegmentControl. 
        /// </summary>
        /// <param name="image"></param>
        /// <param name="title"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public UIImage ImageFromImageAndText(UIImage image, string title, UIColor color)
		{
			var font = UIFont.SystemFontOfSize(defaultFontSize);

			var nsString = new NSString(title);
			var attribs = new UIStringAttributes { Font = font };

			var expectedTextSize = nsString.GetSizeUsingAttributes(attribs);

			var width = expectedTextSize.Width + image.Size.Width + distanceBetweenElements;
			var height = (nfloat)Math.Max(expectedTextSize.Height, image.Size.Width);

			var size = new CGSize(width + 28, height);

			UIGraphics.BeginImageContextWithOptions(size, false, 0);
			var context = UIGraphics.GetCurrentContext();
			context.SetFillColor(color.CGColor);

			nfloat fontTopPosition = (height - expectedTextSize.Height) / half;
			var textPoint = new CGPoint(image.Size.Width + distanceBetweenElements, fontTopPosition);

			title.DrawString(textPoint, font);

			// Images upside down so flip them
			var flipVertical = new CGAffineTransform(1, 0, 0, -1, 0, size.Height);
			context.ConcatCTM(flipVertical);
			var rect = new CGRect(0, (height - image.Size.Height) / half, image.Size.Width, image.Size.Height);

			context.DrawImage(rect, image.CGImage);

			var newImage = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();

			return newImage;
		}
	}
}
