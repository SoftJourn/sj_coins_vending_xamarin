using System;
using Foundation;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers
{
	[Register("DetailViewController")]
	public partial class DetailViewController : UIViewController
	{
		#region Properties
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
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			//Presenter.OnStartLoadingPage();
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
