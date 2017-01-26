using System;
using System.Collections.Generic;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers.Main
{
	[Register("FavoritesViewController")]
	public partial class FavoritesViewController : UIViewController
	{
		#region Properties
		private List<Product> favorites;
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

			//Working with Presenter (fetch products etc) 
			//Presenter.LoadProducts(); 
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
				if (parent.favorites != null || parent.favorites.Count != 0)
				{
					parent.NoItemsLabel.Hidden = true;
					parent.NoItemsLabel.Text = "";
					return 0;
				}
				else {
					parent.NoItemsLabel.Hidden = false;
					return parent.favorites.Count;
				}
			}

			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) => tableView.DequeueReusableCell(ProductCell.Key, indexPath);
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
