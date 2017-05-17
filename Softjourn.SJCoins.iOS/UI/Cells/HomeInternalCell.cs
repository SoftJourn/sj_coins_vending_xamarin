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

		private UIImageView Logo { get; set; }
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

		public void ConfigureWith(Product product)
		{
			this.Product = product;
			this.Layer.CornerRadius = 16;

			if (Logo == null)
			{
				Logo = new UIImageView
				{
					Frame = new CGRect(5, 5, 80, 80),
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

			// Register for preview
			previewing = currentApplication.VisibleViewController.RegisterForPreviewingWithDelegate(this, this);
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
