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
        public event EventHandler<Product> FavoriteClicked;
		public event EventHandler<Product> BuyClicked;

		public bool Favorite { get; set; } = false;
        public Product Product { get; set; }
        #endregion

        #region Constructor
        protected ProductCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
        #endregion

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            SetUpUI();
        }

        public void ConfigureWith(Product item)
        {
            Product = item;
            NameLabel.Text = item.Name;
            LogoImage.SetImage(url: new NSUrl(item.ImageFullUrl), placeholder: UIImage.FromBundle(ImageConstants.Placeholder));
            ConfigureDescript(item);
            ConfigureFavoriteImage(item.IsProductFavorite);
            BuyButton.SetTitle("SJ " + string.Format("{0}", item.Price), UIControlState.Normal);

            // Attach event
            FavoriteButton.TouchUpInside -= FavoriteButtonClicked;
            FavoriteButton.TouchUpInside += FavoriteButtonClicked;

            BuyButton.TouchUpInside -= BuyButtonClicked;
			BuyButton.TouchUpInside += BuyButtonClicked;
		}

        public void MarkFavorites(Product product)
        {
            if (product.IsProductInCurrentMachine)
                LogoImage.Alpha = 1.0f;
            else
                LogoImage.Alpha = 0.3f;
        }

        public override void PrepareForReuse()
        {
            Product = null;
            NameLabel.Text = "";
            DescriptLabel.Text = "";
            LogoImage.Image = null;
            // Detach event
            FavoriteButton.TouchUpInside -= FavoriteButtonClicked;
			BuyButton.TouchUpInside -= BuyButtonClicked;
			base.PrepareForReuse();
        }

        #region Private methods
        private void SetUpUI()
        {
            LogoImage.Layer.CornerRadius = 22;
            LogoImage.Layer.BorderWidth = 0.7f;
            LogoImage.Layer.BorderColor = UIColorConstants.ProductImageBorderColor.CGColor;
        }

		private void ConfigureFavoriteImage(bool isFavorite)
		{
			if (isFavorite)
                FavoriteButton.SetImage(UIImage.FromBundle(ImageConstants.HeartFilled), forState: UIControlState.Normal);
			else
				FavoriteButton.SetImage(UIImage.FromBundle(ImageConstants.Heart), forState: UIControlState.Normal);
		}

		private void ConfigureDescript(Product product)
		{
            var descriptString = "";

            if (!String.IsNullOrEmpty(product.Description))
                descriptString = "Description";
            
            if (product.NutritionFacts.Count > 0)
                descriptString = descriptString + ", Nutrition Facts";

            DescriptLabel.Text = descriptString;
		}
		#endregion

		private void FavoriteButtonClicked(object sender, EventArgs e)
        {
            FavoriteClicked?.Invoke(this, Product);
        }

		private void BuyButtonClicked(object sender, EventArgs e)
		{
            BuyClicked?.Invoke(this, Product);
		}
    }
}
