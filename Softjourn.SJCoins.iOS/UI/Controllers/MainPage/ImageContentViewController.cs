using System;
using Foundation;
using Softjourn.SJCoins.iOS.General.Constants;
using SDWebImage;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers.Main
{
	[Register("ImageContentViewController")]
	public partial class ImageContentViewController : UIViewController
	{
		private string imageUrl;

		//Constructor
		public ImageContentViewController(IntPtr handle) : base(handle)
		{
		}

		public void SetImage(string imageUrl)
		{
			this.imageUrl = imageUrl;
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			Logo.Layer.CornerRadius = 24;
            Logo.Layer.BorderWidth = 1;
            Logo.Layer.BorderColor = UIColorConstants.ProductImageBorderColor.CGColor;
			Logo.Layer.MasksToBounds = true;
			Logo.SetImage(url: new NSUrl(imageUrl), placeholder: UIImage.FromBundle(ImageConstants.Placeholder));
		}
	}
}
