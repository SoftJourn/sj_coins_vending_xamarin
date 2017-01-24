using System;
using Foundation;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers.Main
{
	[Register("AllItemsViewController")]
	public class AllItemsViewController : UIViewController
	{
		#region Properties
		//private List<Categories> categories;
		#endregion

		#region Constructor
		public AllItemsViewController(IntPtr handle) : base(handle)
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
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
		}
		#endregion

		#region BaseViewController -> IBaseView implementation
		#endregion

		#region UITableViewSource implementation
		private class AllItemsViewControllerDataSource : UITableViewSource
		{
			private AllItemsViewController parent;

			public AllItemsViewControllerDataSource(AllItemsViewController parent)
			{
				this.parent = parent;
			}

			public override nint RowsInSection(UITableView tableview, nint section)
			{
				throw new NotImplementedException();
			}

			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				throw new NotImplementedException();
			}
		}

		#endregion

		#region UITableViewDelegate implementation
		private class AllItemsViewControllerDelegate : UITableViewDelegate
		{
			private AllItemsViewController parent;

			public AllItemsViewControllerDelegate(AllItemsViewController parent)
			{
				this.parent = parent;
			}

		}
		#endregion
	}
}
