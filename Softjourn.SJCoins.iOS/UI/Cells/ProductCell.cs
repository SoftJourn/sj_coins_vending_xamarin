using System;
using Foundation;
using SDWebImage;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.iOS.General.Constants;
using Softjourn.SJCoins.iOS.UI.Services;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Cells
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
		public event EventHandler<Product> ProductCell_FavoriteClicked;
		public bool Favorite { get; set; } = false;
		public Product Product { get; set; }
		public bool FavoriteAnimated { get; set; } = false;

		private Lazy<AnimationService> lazyAnimationService = new Lazy<AnimationService>(() => { return new AnimationService(); });
		private AnimationService animationService { get { return lazyAnimationService.Value; } }
		#endregion

		#region Constructor
		protected ProductCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}
		#endregion

		public void ConfigureWith(Product item)
		{
			Product = item;
			NameLabel.Text = item.Name;
			PriceLabel.Text = item.Price.ToString() + " Coins";
			Favorite = item.IsProductFavorite;
			ImageLogo.SetImage(url: new NSUrl(item.ImageFullUrl), placeholder: UIImage.FromBundle(ImageConstants.Placeholder));

			if (item.IsHeartAnimationRunning)
			{
				// Final animation with complition
				animationService.CompleteRotation(FavoriteButton);
				animationService.ScaleEffect(FavoriteButton);
				item.IsHeartAnimationRunning = false;
			}

			if (item.IsProductFavorite)
				FavoriteButton.SetImage(UIImage.FromBundle(ImageConstants.FavoriteChecked), forState: UIControlState.Normal);
			else
				FavoriteButton.SetImage(UIImage.FromBundle(ImageConstants.FavoriteUnchecked), forState: UIControlState.Normal);

			// Attach event
			FavoriteButton.TouchUpInside -= FavoriteButtonClicked;
			FavoriteButton.TouchUpInside += FavoriteButtonClicked;
		}

		public void MarkFavorites(Product product)
		{
			if (product.IsProductInCurrentMachine)
				ImageLogo.Alpha = 1.0f;
			else
				ImageLogo.Alpha = 0.3f;
		}

		public override void PrepareForReuse()
		{
			Product = null;
			NameLabel.Text = "";
			PriceLabel.Text = "";
			Favorite = false;
			ImageLogo.Image = null; 
			// Detach event
			FavoriteButton.TouchUpInside -= FavoriteButtonClicked;
			base.PrepareForReuse();
		}

		private void FavoriteButtonClicked(object sender, EventArgs e)
		{
			// Start animation
			if (!Product.IsHeartAnimationRunning)
			{
				animationService.StartRotation(FavoriteButton);
				Product.IsHeartAnimationRunning = true;
				ProductCell_FavoriteClicked?.Invoke(this, Product);
			}
		}
	}
}
