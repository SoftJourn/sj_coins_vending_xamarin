using System;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using UIKit;
using CoreGraphics;
using SDWebImage;
using Softjourn.SJCoins.iOS.General.Constants;
using Softjourn.SJCoins.iOS.UI.Controllers;

namespace Softjourn.SJCoins.iOS
{
	public partial class HomeInternalCell : UICollectionViewCell, IUIViewControllerPreviewingDelegate
	{
		#region Properties
		public static readonly NSString Key = new NSString("HomeInternalCell");
		public static readonly UINib Nib;

		public event EventHandler<Product> BuyAction;
		public event EventHandler<Product> FavoriteAction;

        private UIFont nameLabelFont = UIFont.SystemFontOfSize(12);
		private UIImageView ProductImage { get; set; }
		private UILabel NameLabel { get; set; }
		private UILabel PriceLabel { get; set; }
        private UIImageView CoinImage { get; set; }
		private Product Product { get; set; }
		private PreViewController previewController;
		private IUIViewControllerPreviewing previewing;
		private AppDelegate currentApplication
		{
			get { return (AppDelegate)UIApplication.SharedApplication.Delegate; }
		}

		static HomeInternalCell()
		{
			Nib = UINib.FromName("HomeInternalCell", NSBundle.MainBundle);
		}
        #endregion

		protected HomeInternalCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            SetUpUI();
        }

		#region Private methods
		private void SetUpUI()
        {
            ProductImage = new UIImageView {
			    BackgroundColor = UIColor.White,
                ContentMode = UIViewContentMode.ScaleAspectFit,
                ClipsToBounds = true                   
            };
            ProductImage.Opaque = true;
            ProductImage.Layer.CornerRadius = 24;
            ProductImage.Layer.BorderWidth = 1;
            ProductImage.Layer.BorderColor = UIColorConstants.ProductImageBorderColor.CGColor;
            AddSubview(ProductImage);

            NameLabel = new UILabel
            {
                Font = nameLabelFont,
                Lines = 2,
                TextColor = UIColorConstants.ProductNameColor,
                BackgroundColor = UIColor.White,
                LineBreakMode = UILineBreakMode.TailTruncation
            };
            AddSubview(NameLabel);

            PriceLabel = new UILabel {
                Font = nameLabelFont,
                Lines = 1,
                BackgroundColor = UIColor.White,
                TextColor = UIColorConstants.ProductPriceColor
            };
            AddSubview(PriceLabel);

            CoinImage = new UIImageView {
				Image = UIImage.FromBundle(ImageConstants.Bitcoin),
                BackgroundColor = UIColor.White,
				ContentMode = UIViewContentMode.ScaleAspectFit,
				ClipsToBounds = true
			};
			AddSubview(CoinImage);
            BringSubviewToFront(CoinImage);
        }

        public void LayoutUI(Product product)
        {
            if (product != null)
            {
                ProductImage.Frame = new CGRect(0, 0, this.Frame.Width, this.Frame.Width);
				var expectedNameSize = NameLabel.SizeThatFits(new CGSize(this.Frame.Width, 28));
				NameLabel.Frame = new CGRect(2, ProductImage.Frame.Height + 8, this.Frame.Width, expectedNameSize.Height);

				var expectedPriceSize = PriceLabel.SizeThatFits(new CGSize(this.Frame.Width, 14));
				PriceLabel.Frame = new CGRect(2, ProductImage.Frame.Height + 6 + NameLabel.Frame.Height + 2, expectedPriceSize.Width, 14);

				CoinImage.Frame = new CGRect(PriceLabel.Frame.Width + 2, ProductImage.Frame.Height + 6 + NameLabel.Frame.Height + 2, 14, 14);   
            }
        }
		#endregion

		#region Public methods
		public void ConfigureWith(Product product)
		{
			this.Product = product;
			//this.Layer.CornerRadius = 24;

			NameLabel.Text = product.Name;
			PriceLabel.Text = product.Price.ToString();
            // On server image size 200*200
			ProductImage.SetImage(url: new NSUrl(product.ImageFullUrl), placeholder: UIImage.FromBundle(ImageConstants.Placeholder));

			LayoutUI(Product);

			// Register for preview
			previewing = currentApplication.VisibleViewController.RegisterForPreviewingWithDelegate(this, this);
		}

		public override void PrepareForReuse()
		{
			// Reset outlets
			ProductImage.Image = null;
			ProductImage.Alpha = 1.0f;
			NameLabel.Text = "";
            NameLabel.Frame = new CGRect(0, ProductImage.Frame.Height + 8, 0, 0);
            NameLabel.Alpha = 1.0f;
			PriceLabel.Text = "";
            PriceLabel.Alpha = 1.0f;

			// Dettach
			if (previewController != null)
			{
                previewController.PreViewController_BuyActionExecuted -= BuyAction;
                previewController.PreViewController_FavoriteActionExecuted -= FavoriteAction;
				previewController = null;
			}
            if (previewing != null)
            {
				// Unregister for preview
				currentApplication.VisibleViewController.UnregisterForPreviewingWithContext(previewing);
            }

			//Layer.ShouldRasterize = true;
			//Layer.RasterizationScale = UIScreen.MainScreen.Scale;

			base.PrepareForReuse();
		}

		public void MarkFavorites(Product product)
		{
			if (product.IsProductInCurrentMachine)
            {
                ProductImage.Alpha = 1.0f;
                NameLabel.Alpha = 1.0f;
                PriceLabel.Alpha = 1.0f;
                CoinImage.Alpha = 1.0f;
            }
			else
            {
				ProductImage.Alpha = 0.3f;
                NameLabel.Alpha = 0.3f;
                PriceLabel.Alpha = 0.3f;
                CoinImage.Alpha = 0.3f;
			}
		}
		#endregion

		#region IUIViewControllerPreviewingDelegate implementation
		public UIViewController GetViewControllerForPreview(IUIViewControllerPreviewing previewingContext, CGPoint location)
		{
			// Create a preview controller and set its properties.
			previewController = (PreViewController)UIStoryboard.FromName(StoryboardConstants.StoryboardMain, null).InstantiateViewController(StoryboardConstants.PreViewController);
			if (previewController == null)
				return null;

			previewController.SetItem(Product);
			previewController.PreferredContentSize = new CGSize(0, 420);
            previewingContext.SourceRect = this.Bounds;

			// Attach
            previewController.PreViewController_BuyActionExecuted += BuyAction;
            previewController.PreViewController_FavoriteActionExecuted += FavoriteAction;

			return previewController;
		}

		public void CommitViewController(IUIViewControllerPreviewing previewingContext, UIViewController viewControllerToCommit)
		{
			currentApplication.VisibleViewController.ShowViewController(viewControllerToCommit, this);
		}
		#endregion
	}
}
