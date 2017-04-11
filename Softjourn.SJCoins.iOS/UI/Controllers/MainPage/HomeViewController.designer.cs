// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Softjourn.SJCoins.iOS.UI.Controllers.Main
{
	partial class HomeViewController
	{
		[Outlet]
		UIKit.UIBarButtonItem AccountButton { get; set; }

		[Outlet]
		UIKit.UICollectionView CollectionView { get; set; }

		[Outlet]
		UIKit.UILabel MachineNameLabel { get; set; }

		[Outlet]
		UIKit.UILabel MyBalanceLabel { get; set; }

		[Outlet]
		UIKit.UILabel NoItemsLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (MachineNameLabel != null) {
				MachineNameLabel.Dispose ();
				MachineNameLabel = null;
			}

			if (MyBalanceLabel != null) {
				MyBalanceLabel.Dispose ();
				MyBalanceLabel = null;
			}

			if (AccountButton != null) {
				AccountButton.Dispose ();
				AccountButton = null;
			}

			if (CollectionView != null) {
				CollectionView.Dispose ();
				CollectionView = null;
			}

			if (NoItemsLabel != null) {
				NoItemsLabel.Dispose ();
				NoItemsLabel = null;
			}
		}
	}
}
