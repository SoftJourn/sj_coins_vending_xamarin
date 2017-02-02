using System;

using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using UIKit;
using SDWebImage;
using Softjourn.SJCoins.Core.Utils;

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
			Logo.SetImage(url: new NSUrl(Const.BaseUrl+Const.UrlVendingService+product.ImageUrl), placeholder: UIImage.FromBundle("Placeholder.png"));
		}
	}
}
