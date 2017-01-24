using System;
using Foundation;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers.Main
{
	[Register("AccountViewController")]
	public class AccountViewController : UIViewController
	{
		#region Properties
		//private List<Categories> categories;
		#endregion

		#region Constructor
		public AccountViewController(IntPtr handle) : base(handle)
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
		private class AccountViewControllerDataSource : UITableViewSource
		{
			private AccountViewController parent;

			public AccountViewControllerDataSource(AccountViewController parent)
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
		private class AccountViewControllerDelegate : UITableViewDelegate
		{
			private AccountViewController parent;

			public AccountViewControllerDelegate(AccountViewController parent)
			{
				this.parent = parent;
			}

		}
		#endregion
	}
}
