using System;

using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using UIKit;
using SDWebImage;
using Softjourn.SJCoins.Core.Utils;
using Softjourn.SJCoins.iOS.General.Constants;

namespace Softjourn.SJCoins.iOS
{
	public partial class HomeInternalCell : UICollectionViewCell
	{
		public static readonly NSString Key = new NSString("HomeInternalCell");
		public static readonly UINib Nib;

		static HomeInternalCell()
		{
			Nib = UINib.FromName("HomeInternalCell", NSBundle.MainBundle);
		}

		protected HomeInternalCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public void ConfigureWith(Product product)
		{
			// Set outlets
			NameLabel.Text = product.Name;
			PriceLabel.Text = product.Price.ToString() + " coins";
			Logo.SetImage(url: new NSUrl(product.ImageFullUrl), placeholder: UIImage.FromBundle(ImageConstants.Placeholder));
		}

		public override void PrepareForReuse()
		{
			// Reset outlets
			NameLabel.Text = "";
			PriceLabel.Text = "";
			Logo.Image = null;
			base.PrepareForReuse();
		}
	}
}
