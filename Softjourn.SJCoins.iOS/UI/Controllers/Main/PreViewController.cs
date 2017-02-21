using System;
using Foundation;
using SDWebImage;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.iOS.General.Constants;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers
{
	[Register("PreViewController")]
	public partial class PreViewController : UIViewController
	{
		#region Properties
		private Product currentProduct { get; set; }

		public event EventHandler<Product> BuyActionExecuted;
		public event EventHandler<Product> FavoriteActionExecuted;

		public override IUIPreviewActionItem[] PreviewActionItems
		{
			get { return PreviewActions; }
		}

		IUIPreviewActionItem[] PreviewActions
		{
			get {
				var action1 = PreviewActionForTitle("Buy", UIPreviewActionStyle.Default, new Action(BuyActionClicked));
				var action2 = PreviewActionForTitle("Add to favorite", UIPreviewActionStyle.Default, new Action(FavoriteActionClicked));
				return new IUIPreviewActionItem[] { action1, action2 }; 
			}
		}
		#endregion

		#region Constructor
		public PreViewController(IntPtr handle) : base(handle)
		{
		}

		public void SetItem(Product item)
		{
			this.currentProduct = item;
		}
		#endregion

		#region Controller Life cycle
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			ConfigurePageWith(currentProduct);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
		}
		#endregion

		#region Private methods
		private void ConfigurePageWith(Product product)
		{
			if (product != null)
				NameLabel.Text = product.Name;
				PriceLabel.Text = product.Price.ToString();
				DescriptionLabel.Text = "Descriprion lalala....";
				
				Logo.SetImage(url: new NSUrl(product.ImageFullUrl), placeholder: UIImage.FromBundle(ImageConstants.Placeholder));

				ConfigureFavoriteImage(product.IsProductFavorite);
		}

		private void ConfigureFavoriteImage(bool isFavorite)
		{
			if (isFavorite)
				FavoriteButton.SetImage(UIImage.FromBundle(ImageConstants.FavoriteChecked), forState: UIControlState.Normal);
			else
				FavoriteButton.SetImage(UIImage.FromBundle(ImageConstants.FavoriteUnchecked), forState: UIControlState.Normal);
		}

		// -------------------- Action handlers --------------------
		private void BuyActionClicked()
		{
			BuyActionExecuted?.Invoke(this, currentProduct);
		}

		private void FavoriteActionClicked()
		{
			FavoriteActionExecuted?.Invoke(this, currentProduct);
		}
		// --------------------------------------------------------
		#endregion

		private UIPreviewAction PreviewActionForTitle(string title, UIPreviewActionStyle style = UIPreviewActionStyle.Default, Action handler = null)
		{
			return UIPreviewAction.Create(title, style, (action, previewViewController) =>
			{
				if (handler != null)
				{
					handler();
				}
			});
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			System.Diagnostics.Debug.WriteLine("Softjourn.SJCoins.iOS.UI.Controllers.PreViewController disposed");
		}
	}
}
