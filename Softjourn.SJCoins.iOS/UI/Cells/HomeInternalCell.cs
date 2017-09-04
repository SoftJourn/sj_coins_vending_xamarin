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

		public event EventHandler<Product> HomeInternalCell_BuyActionExecuted;
		public event EventHandler<Product> HomeInternalCell_FavoriteActionExecuted;

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
            SetUp();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
			
            ProductImage.Frame = new CGRect(0, 0, this.Frame.Width, this.Frame.Width);

            var expectedNameSize = NameLabel.SizeThatFits(new CGSize(this.Frame.Width, 28));
            NameLabel.Frame = new CGRect(0, ProductImage.Frame.Height + 8, this.Frame.Width, expectedNameSize.Height);

            var expectedPriceSize = PriceLabel.SizeThatFits(new CGSize(this.Frame.Width, 14));
			PriceLabel.Frame = new CGRect(0, ProductImage.Frame.Height + 6 + NameLabel.Frame.Height + 2, expectedPriceSize.Width, 14);

            CoinImage.Frame = new CGRect(PriceLabel.Frame.Width, ProductImage.Frame.Height + 6 + NameLabel.Frame.Height + 2, 14, 14);
		}

		#region Private methods
		private void SetUp()
        {
            ProductImage = new UIImageView {
                BackgroundColor = UIColor.White,
                ContentMode = UIViewContentMode.ScaleAspectFit,
                ClipsToBounds = true                   
            };
            ProductImage.Layer.CornerRadius = 24;
            ProductImage.Layer.BorderWidth = 1;
            ProductImage.Layer.BorderColor = UIColor.FromRGB(220, 220, 220).ColorWithAlpha(1.0f).CGColor; 
            AddSubview(ProductImage);

            NameLabel = new UILabel {
                Font = UIFont.SystemFontOfSize(11),
                Lines = 2,
                TextColor = UIColor.FromRGB(81, 69, 62).ColorWithAlpha(1.0f),
                BackgroundColor = UIColor.White,
                LineBreakMode = UILineBreakMode.TailTruncation
            };
            AddSubview(NameLabel);

            PriceLabel = new UILabel {
                Font = UIFont.SystemFontOfSize(11),
                Lines = 1,
                BackgroundColor = UIColor.White,
                TextColor = UIColor.Gray
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
		#endregion

		#region Public methods
		public void ConfigureWith(Product product)
		{
			this.Product = product;
			this.Layer.CornerRadius = 24;

			NameLabel.Text = product.Name;
			PriceLabel.Text = product.Price.ToString();
			ProductImage.SetImage(url: new NSUrl(product.ImageFullUrl), placeholder: UIImage.FromBundle(ImageConstants.Placeholder));

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
			PriceLabel.Text = "";

			// Dettach
			if (previewController != null)
			{
				previewController.PreViewController_BuyActionExecuted -= HomeInternalCell_BuyActionExecuted;
				previewController.PreViewController_FavoriteActionExecuted -= HomeInternalCell_FavoriteActionExecuted;
				previewController = null;
			}
			// Unregister for preview
			currentApplication.VisibleViewController.UnregisterForPreviewingWithContext(previewing);
			base.PrepareForReuse();
		}

		public void MarkFavorites(Product product)
		{
			if (product.IsProductInCurrentMachine)
				ProductImage.Alpha = 1.0f;
			else
				ProductImage.Alpha = 0.3f;
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
			previewController.PreViewController_BuyActionExecuted += HomeInternalCell_BuyActionExecuted;
			previewController.PreViewController_FavoriteActionExecuted += HomeInternalCell_FavoriteActionExecuted;

			return previewController;
		}

		public void CommitViewController(IUIViewControllerPreviewing previewingContext, UIViewController viewControllerToCommit)
		{
			currentApplication.VisibleViewController.ShowViewController(viewControllerToCommit, this);
		}
		#endregion
	}
}
