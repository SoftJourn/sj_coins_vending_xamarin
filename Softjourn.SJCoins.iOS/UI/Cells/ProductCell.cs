using System;

using Foundation;
using SDWebImage;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Core.Utils;
using Softjourn.SJCoins.iOS.General.Constants;
using UIKit;

namespace Softjourn.SJCoins.iOS
{
	public partial class ProductCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString("ProductCell");
		public static readonly UINib Nib;

		static ProductCell()
		{
			Nib = UINib.FromName("ProductCell", NSBundle.MainBundle);
		}

		protected ProductCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public void ConfigureWith(Product item)
		{
			NameLabel.Text = item.Name;
			PriceLabel.Text = item.Price.ToString() + " Coins";
			ImageLogo.SetImage(url: new NSUrl(item.ImageFullUrl), placeholder: UIImage.FromBundle(ImageConstants.Placeholder));

		}
	}
}
