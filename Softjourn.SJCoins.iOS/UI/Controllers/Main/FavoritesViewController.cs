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
		public List<Product> FavoriteProducts { get; private set; } = new List<Product>();
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

			ConfigurePage();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			//Presenter.OnStartLoadingPage();
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			TableView.RegisterNibForCellReuse(ProductCell.Nib, ProductCell.Key);
		}
		#endregion

		#region Private methods
		private void ConfigurePage()
		{
			//Hide no items label
			NoItemsLabel.Hidden = true;

			// Configure datasource and delegate
			TableView.Source = new FavoritesViewControllerSource(this);
		}

		// Throw CollectionView to parent
		//protected override UIScrollView GetRefreshableScrollView() => TableView;

		//public void OnItemSelected(object sender, Product product)
		//{
		//	// Trigg presenter that user click on some product for showing details controllers
		//	Presenter.OnProductClick(product);
		//}
		#endregion

		#region IFavoritesView implementation
		#endregion

		#region BaseViewController -> IBaseView implementation
		#endregion
	}

	#region UITableViewSource implementation
	public class FavoritesViewControllerSource : UITableViewSource
	{
		private FavoritesViewController parent;

		public FavoritesViewControllerSource(FavoritesViewController parent)
		{
			this.parent = parent;
		}

		public override nint RowsInSection(UITableView tableview, nint section) 
		{
			return 3;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) 
		{
			return tableView.DequeueReusableCell(ProductCell.Key, indexPath);
		}	

		public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
		{
			var _cell = (ProductCell)cell;
			//var category = parent.Categories[indexPath.Row];

		}
	}
	#endregion
}
