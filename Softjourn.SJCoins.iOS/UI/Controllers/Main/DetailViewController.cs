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
	[Register("DetailViewController")]
	public partial class DetailViewController : BaseViewController<DetailPresenter>, IDetailView
	{
		#region Properties
		public int ProductId { get; set; }

		private Product product;
		#endregion

		#region Constructor
		public DetailViewController(IntPtr handle) : base(handle)
		{
		}
		#endregion

		#region Controller Life cycle
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			product = Presenter.GetProduct(ProductId);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			ConfigurePageWith(product);
			// Attach 
			FavoriteButton.TouchUpInside += FavoriteButtonClickHandler;
			BuyButton.TouchUpInside += BuyButtonClickHandler;
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
		}

		public override void ViewWillDisappear(bool animated)
		{
			// Detach
			FavoriteButton.TouchUpInside -= FavoriteButtonClickHandler;
			BuyButton.TouchUpInside -= BuyButtonClickHandler;
			base.ViewWillDisappear(animated);
		}
		#endregion

		#region IDetailView implementation
		public void FavoriteChanged(bool isFavorite)
		{
			// change button image
			ConfigureFavoriteImage(isFavorite);
			// TODO let know another controllers in this product is favorite
		}
		#endregion

		#region Private methods
		// ---------------- Buttons methods ---------------- 
		private void FavoriteButtonClickHandler(object sender, EventArgs e)
		{
			// Handle clicking on the Favorite button
			Presenter.OnFavoriteClick(product);
		}

		private void BuyButtonClickHandler(object sender, EventArgs e)
		{
			// Handle clicking on the Buy button
			Presenter.OnBuyProductClick(product);
		}
		// ------------------------------------------------- 

		private void ConfigurePageWith(Product product)
		{
			NameLabel.Text = product.Name;
			PriceLabel.Text = product.Price.ToString();
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
		#endregion

		#region BaseViewController -> IBaseView implementation
		#endregion
	}
}
