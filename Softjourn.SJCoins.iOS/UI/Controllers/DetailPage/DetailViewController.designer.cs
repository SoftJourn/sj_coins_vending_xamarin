// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Softjourn.SJCoins.iOS.UI.Controllers
{
	partial class DetailViewController
	{
		[Outlet]
		UIKit.UIButton BuyButton { get; set; }

		[Outlet]
		UIKit.UIBarButtonItem FavoriteButton { get; set; }

		[Outlet]
		UIKit.UIView HeaderView { get; set; }

		[Outlet]
		UIKit.UICollectionView ImageCollectionView { get; set; }

		[Outlet]
		UIKit.UIPageControl PageControl { get; set; }

		[Outlet]
		UIKit.UILabel PriceLabel { get; set; }

		[Outlet]
		UIKit.UITableView TableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (BuyButton != null) {
				BuyButton.Dispose ();
				BuyButton = null;
			}

			if (FavoriteButton != null) {
				FavoriteButton.Dispose ();
				FavoriteButton = null;
			}

			if (HeaderView != null) {
				HeaderView.Dispose ();
				HeaderView = null;
			}

			if (PageControl != null) {
				PageControl.Dispose ();
				PageControl = null;
			}

			if (PriceLabel != null) {
				PriceLabel.Dispose ();
				PriceLabel = null;
			}

			if (TableView != null) {
				TableView.Dispose ();
				TableView = null;
			}

			if (ImageCollectionView != null) {
				ImageCollectionView.Dispose ();
				ImageCollectionView = null;
			}
		}
	}
}
