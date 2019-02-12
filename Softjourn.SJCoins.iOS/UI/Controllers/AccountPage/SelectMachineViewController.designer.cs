// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//

using Foundation;

namespace Softjourn.SJCoins.iOS.UI.Controllers.AccountPage
{
	partial class SelectMachineViewController
	{
		[Outlet]
		UIKit.UILabel NoMachinesLabel { get; set; }

		[Outlet]
		UIKit.UITableView TableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (TableView != null) {
				TableView.Dispose ();
				TableView = null;
			}

			if (NoMachinesLabel != null) {
				NoMachinesLabel.Dispose ();
				NoMachinesLabel = null;
			}
		}
	}
}
