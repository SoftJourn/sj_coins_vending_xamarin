using System;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers
{
	[Register("DetailViewController")]
	public partial class DetailViewController : BaseViewController<DetailPresenter>, IDetailView
	{
		#region Properties
		public int ProductId { get; set; }
		public Product SelectedProduct;
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

			SelectedProduct = Presenter.GetProduct(ProductId);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

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
