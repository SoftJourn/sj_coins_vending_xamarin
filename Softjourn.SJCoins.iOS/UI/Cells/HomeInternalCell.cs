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
			//NameLabel.Text = product.Name;
			//PriceLabel.Text = product.Price.ToString() + " coins";
			Logo.Layer.CornerRadius = 16;
			////Logo.Layer.BorderWidth = 0.1f;
			Logo.SetImage(url: new NSUrl(product.ImageFullUrl), placeholder: UIImage.FromBundle(ImageConstants.Placeholder));
		}

		public void MarkFavorites(Product product)
		{
			if (product.IsProductInCurrentMachine)
				Logo.Alpha = 1.0f;
			else
				Logo.Alpha = 0.3f;
		}

		public override void PrepareForReuse()
		{
			// Reset outlets
			//NameLabel.Text = "";
			//PriceLabel.Text = "";
			Logo.Image = null;

			//Layer.ShouldRasterize = true;
			//Layer.RasterizationScale = UIScreen.MainScreen.Scale;

			//Logo.Alpha = 1.0f;

			base.PrepareForReuse();
		}
	}
}
