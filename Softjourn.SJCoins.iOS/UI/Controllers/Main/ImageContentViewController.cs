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
		//Constructor
		public ImageContentViewController(IntPtr handle) : base(handle)
		{
		}

		public void SetImage(string imageUrl)
		{
			Image.SetImage(url: new NSUrl(imageUrl), placeholder: UIImage.FromBundle(ImageConstants.Placeholder));
		}
	}
}
