using System;
using Foundation;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers.Main
{
	[Register("FavoritesViewController")]
	public class FavoritesViewController : UIViewController
	{
		#region Properties
		//private List<Categories> categories;
		#endregion

		#region Constructor
		public FavoritesViewController(IntPtr handle) : base(handle)
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
		private class FavoritesViewControllerDataSource : UITableViewSource
		{
			private FavoritesViewController parent;

			public FavoritesViewControllerDataSource(FavoritesViewController parent)
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
		private class FavoritesViewControllerDelegate : UITableViewDelegate
		{
			private FavoritesViewController parent;

			public FavoritesViewControllerDelegate(FavoritesViewController parent)
			{
				this.parent = parent;
			}

		}
		#endregion
	}
}
