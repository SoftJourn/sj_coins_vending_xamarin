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

		private Product selectedProduct;
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

			selectedProduct = Presenter.GetProduct(ProductId);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			NameLabel.Text = selectedProduct.Name; 
			PriceLabel.Text = selectedProduct.Price.ToString();
			Logo.SetImage(url: new NSUrl(selectedProduct.ImageFullUrl), placeholder: UIImage.FromBundle(ImageConstants.Placeholder));
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
		}

		public void FavoriteChanged()
		{
			throw new NotImplementedException();
		}
		#endregion

		#region IDetailView implementation
		#endregion

		#region Private methods
		#endregion

		#region BaseViewController -> IBaseView implementation
		#endregion
	}
}
