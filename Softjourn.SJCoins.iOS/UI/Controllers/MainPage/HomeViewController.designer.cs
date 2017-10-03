// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
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
		UIKit.UIImageView CoinLogo { get; set; }

		[Outlet]
		UIKit.UILabel MachineNameLabel { get; set; }

		[Outlet]
		UIKit.UILabel MyBalanceLabel { get; set; }

		[Outlet]
		UIKit.UILabel NoItemsLabel { get; set; }

		[Outlet]
		UIKit.UITableView TableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (AccountButton != null) {
				AccountButton.Dispose ();
				AccountButton = null;
			}

			if (MachineNameLabel != null) {
				MachineNameLabel.Dispose ();
				MachineNameLabel = null;
			}

			if (MyBalanceLabel != null) {
				MyBalanceLabel.Dispose ();
				MyBalanceLabel = null;
			}

			if (NoItemsLabel != null) {
				NoItemsLabel.Dispose ();
				NoItemsLabel = null;
			}

			if (TableView != null) {
				TableView.Dispose ();
				TableView = null;
			}

			if (CoinLogo != null) {
				CoinLogo.Dispose ();
				CoinLogo = null;
			}
		}
	}
}
