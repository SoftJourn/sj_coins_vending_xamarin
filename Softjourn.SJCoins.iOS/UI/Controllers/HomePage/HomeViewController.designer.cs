// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Softjourn.SJCoins.iOS.UI.Controllers.HomePage
{
	partial class HomeViewController
	{
		[Outlet]
		UIKit.UIView AccountView { get; set; }

		[Outlet]
		UIKit.UIImageView AvatarImage { get; set; }

		[Outlet]
		UIKit.UIImageView CoinLogo { get; set; }

		[Outlet]
		UIKit.UILabel MachineNameLabel { get; set; }

		[Outlet]
		UIKit.UILabel MyBalanceLabel { get; set; }

		[Outlet]
		UIKit.UILabel NoItemsLabel { get; set; }

		[Outlet]
		UIKit.UIBarButtonItem SearchButton { get; set; }

		[Outlet]
		UIKit.UITableView TableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (AccountView != null) {
				AccountView.Dispose ();
				AccountView = null;
			}

			if (AvatarImage != null) {
				AvatarImage.Dispose ();
				AvatarImage = null;
			}

			if (CoinLogo != null) {
				CoinLogo.Dispose ();
				CoinLogo = null;
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

			if (SearchButton != null) {
				SearchButton.Dispose ();
				SearchButton = null;
			}

			if (TableView != null) {
				TableView.Dispose ();
				TableView = null;
			}
		}
	}
}
