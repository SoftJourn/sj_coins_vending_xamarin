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
		#region Properties
		public static readonly NSString Key = new NSString("ProductCell");
		public static readonly UINib Nib;

		static ProductCell()
		{
			Nib = UINib.FromName("ProductCell", NSBundle.MainBundle);
		}

		public event EventHandler<Product> FavoriteClicked;
		public bool Favorite { get; set; } = false;
		private Product product;
		#endregion

		#region Constructor
		protected ProductCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}
		#endregion

		public void ConfigureWith(Product item)
		{
			product = item;
			NameLabel.Text = item.Name;
			PriceLabel.Text = item.Price.ToString() + " Coins";
			Favorite = item.IsProductFavorite;
			ImageLogo.SetImage(url: new NSUrl(item.ImageFullUrl), placeholder: UIImage.FromBundle(ImageConstants.Placeholder));

			if (item.IsProductFavorite)
				FavoriteButton.SetImage(UIImage.FromBundle(ImageConstants.FavoriteChecked), forState: UIControlState.Normal);
			else
				FavoriteButton.SetImage(UIImage.FromBundle(ImageConstants.FavoriteUnchecked), forState: UIControlState.Normal);

			FavoriteButton.TouchUpInside -= FavoriteButtonClicked;
			FavoriteButton.TouchUpInside += FavoriteButtonClicked;
		}

		private void FavoriteButtonClicked(object sender, EventArgs e)
		{
			var handler = FavoriteClicked;
			if (handler != null)
			{
				handler(this, product);
			}
		}
	}
}
