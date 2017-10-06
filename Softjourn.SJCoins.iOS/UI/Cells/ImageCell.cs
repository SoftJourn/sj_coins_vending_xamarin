using System;

using Foundation;
using SDWebImage;
using Softjourn.SJCoins.iOS.General.Constants;
using UIKit;

namespace Softjourn.SJCoins.iOS
{
    public partial class ImageCell : UICollectionViewCell
    {
        public static readonly NSString Key = new NSString("ImageCell");
        public static readonly UINib Nib;

        static ImageCell()
        {
            Nib = UINib.FromName("ImageCell", NSBundle.MainBundle);
        }

        protected ImageCell(IntPtr handle) : base(handle)
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
