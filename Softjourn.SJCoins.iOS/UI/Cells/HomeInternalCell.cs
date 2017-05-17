using System;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using UIKit;
using CoreGraphics;
using SDWebImage;
using Softjourn.SJCoins.iOS.General.Constants;

namespace Softjourn.SJCoins.iOS
{
	public partial class HomeInternalCell : UICollectionViewCell
	{
		public static readonly NSString Key = new NSString("HomeInternalCell");
		public static readonly UINib Nib;

		private UIImageView Logo { get; set; }
		private UILabel NameLabel { get; set; }
		private UILabel PriceLabel { get; set; }

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
			if (Logo == null)
			{
				Logo = new UIImageView
				{
					Frame = new CGRect(5, 0, 80, 80),
					BackgroundColor = UIColor.White,
					ContentMode = UIViewContentMode.ScaleAspectFit,
					ClipsToBounds = true           
				};
				Logo.Layer.CornerRadius = 16;
				Logo.Layer.BorderWidth = 0.1f;
				AddSubview(Logo);
			}

			if (NameLabel == null)
			{
				NameLabel = new UILabel
				{
					Frame = new CGRect(5, Logo.Frame.Height + 5, 80, 28),
					Font = UIFont.SystemFontOfSize(11),
					Lines = 2,
					BackgroundColor = UIColor.White
				};
				AddSubview(NameLabel);
			}

			if (PriceLabel == null)
			{
				PriceLabel = new UILabel
				{
					Frame = new CGRect(5, Logo.Frame.Height + 5 + NameLabel.Frame.Height + 2, 80, 14),
					Font = UIFont.SystemFontOfSize(11),
					Lines = 1,
					BackgroundColor = UIColor.White,
					TextColor = UIColor.Gray
				};
				AddSubview(PriceLabel);
			}

			NameLabel.Text = product.Name;
			PriceLabel.Text = product.Price.ToString() + " coins";
			Logo.SetImage(url: new NSUrl(product.ImageFullUrl), placeholder: UIImage.FromBundle(ImageConstants.Placeholder));
		}

		public void MarkFavorites(Product product)
		{
			if (product.IsProductInCurrentMachine)
			{
				Logo.Alpha = 1.0f;
				NameLabel.Alpha = 1.0f;
				PriceLabel.Alpha = 1.0f;
			}
			else
			{
				Logo.Alpha = 0.3f;
				NameLabel.Alpha = 0.3f;
				PriceLabel.Alpha = 0.3f;
			}
		}

		public override void PrepareForReuse()
		{
			// Reset outlets
			NameLabel.Text = "";
			PriceLabel.Text = "";
			Logo.Image = null;
			Logo.Alpha = 1.0f;
			NameLabel.Alpha = 1.0f;
			PriceLabel.Alpha = 1.0f;
			base.PrepareForReuse();
		}
	}
}
