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
        public static readonly NSString Key = new NSString("HomeInternalCell");
        public static readonly UINib Nib;

        public event EventHandler<Product> HomeInternalCell_BuyActionExecuted;
        public event EventHandler<Product> HomeInternalCell_FavoriteActionExecuted;

		private UIFont nameLabelFont = UIFont.SystemFontOfSize(12);
		private UIImageView LogoImage { get; set; }
		private UILabel NameLabel { get; set; }
		private UILabel PriceLabel { get; set; }
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

        protected HomeInternalCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
			SetUpUI();
        }

		public override void LayoutSubviews()
		{
		    base.LayoutSubviews();
			LayoutUI(Product);
		}

        public void Fill(Product product)
        {
			this.Product = product;

			NameLabel.Text = product.Name;
            PriceLabel.Text = product.Price.ToString() + " coins";
            LogoImage.SetImage(url: new NSUrl(product.ImageFullUrl), placeholder: UIImage.FromBundle(ImageConstants.Placeholder));

			// Register for preview
			previewing = currentApplication.VisibleViewController.RegisterForPreviewingWithDelegate(this, this);
        }

        public void MarkFavorites(Product product)
		{
            if (product.IsProductInCurrentMachine)
            {
				LogoImage.Alpha = 1.0f;
                NameLabel.Alpha = 1.0f;
                PriceLabel.Alpha = 1.0f;
            }
            else
            {
                LogoImage.Alpha = 0.3f;
                NameLabel.Alpha = 0.3f;
                PriceLabel.Alpha = 0.3f;
            }
        }

        public override void PrepareForReuse()
        {
            // Reset outlets
            NameLabel.Text = "";
            PriceLabel.Text = "";
			LogoImage.Image = null;
            LogoImage.Alpha = 1.0f;
            NameLabel.Alpha = 1.0f;
            PriceLabel.Alpha = 1.0f;

            // Dettach
            if (previewController != null)
            {
                previewController.PreViewController_BuyActionExecuted -= HomeInternalCell_BuyActionExecuted;
                previewController.PreViewController_FavoriteActionExecuted -= HomeInternalCell_FavoriteActionExecuted;
                previewController = null;
            }
			if (previewing != null)
			{
				// Unregister for preview
				currentApplication.VisibleViewController.UnregisterForPreviewingWithContext(previewing);
			}
            base.PrepareForReuse();
        }

		#region Private methods
		private void SetUpUI()
		{
			LogoImage = new UIImageView
			{
				BackgroundColor = UIColor.White,
				ContentMode = UIViewContentMode.ScaleAspectFit,
				ClipsToBounds = true
			};
			LogoImage.Layer.CornerRadius = 16;
			LogoImage.Layer.BorderWidth = 1f / UIScreen.MainScreen.Scale;
			LogoImage.Layer.BorderColor = UIColor.LightGray.CGColor;
			AddSubview(LogoImage);

			NameLabel = new UILabel
			{
				Font = nameLabelFont,
				Lines = 2,
				BackgroundColor = UIColor.Clear,
				TextColor = UIColor.Black
			};
			AddSubview(NameLabel);

			PriceLabel = new UILabel
			{
				Font = UIFont.BoldSystemFontOfSize(11),
				Lines = 1,
				BackgroundColor = UIColor.Clear,
				TextColor = UIColor.DarkGray //UIColor.FromRGBA(red: 80, green: 80, blue: 80, alpha: 1)
			};
            AddSubview(PriceLabel);	
		}

		private void LayoutUI(Product product)
		{
			if (product != null)
			{
				var leftInset = 0;
				var cellWidht = 100;
				var nameLabelTopRetreat = 5;
				var nameLabelBottomRetreat = 5;

				// Calculate size of product name text.
				var nameString = new NSString(product.Name);
				CGSize nameStringSize = nameString.GetSizeUsingAttributes(new UIStringAttributes { Font = nameLabelFont });

				// Set frames to UI elements.
				LogoImage.Frame = new CGRect(leftInset, 1, cellWidht, 100);

				if (nameStringSize.Width < cellWidht - 10)
				{
					NameLabel.Frame = new CGRect(leftInset, LogoImage.Frame.Height + nameLabelTopRetreat, cellWidht, 15);
				}
				else
				{
					NameLabel.Frame = new CGRect(leftInset, LogoImage.Frame.Height + nameLabelTopRetreat, cellWidht, 30);
				}

				PriceLabel.Frame = new CGRect(leftInset, LogoImage.Frame.Height + nameLabelBottomRetreat + NameLabel.Frame.Height, cellWidht, 15);
			}
		}

		#endregion

		#region IUIViewControllerPreviewingDelegate implementation
		public override void TraitCollectionDidChange(UITraitCollection previousTraitCollection)
        {
            // Must call base method
            base.TraitCollectionDidChange(previousTraitCollection);

            if (TraitCollection.ForceTouchCapability != UIForceTouchCapability.Available)
            {
                // TODO Need move fom here !!!
                //UIAlertController alertController = UIAlertController.Create("3D Touch Not Available", "Unsupported device.", UIAlertControllerStyle.Alert);
                //currentApplication.VisibleViewController.PresentViewController(alertController, true, null);
            }
        }

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
