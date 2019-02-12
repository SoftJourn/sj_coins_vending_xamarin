using System;
using Foundation;
using SDWebImage;
using Softjourn.SJCoins.iOS.General.Constants;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Cells
{
    public partial class ImageDetailCell : UICollectionViewCell
    {
        public static readonly NSString Key = new NSString("ImageDetailCell");
        public static readonly UINib Nib;

        static ImageDetailCell()
        {
            Nib = UINib.FromName("ImageDetailCell", NSBundle.MainBundle);
        }

        protected ImageDetailCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

		public void ConfigureWith(string link)
		{
			ProductImage.SetImage(url: new NSUrl(link), placeholder: UIImage.FromBundle(ImageConstants.Placeholder));
		}

		public override void PrepareForReuse()
		{
			base.PrepareForReuse();
			ProductImage.Image = null;
		}
    }
}
