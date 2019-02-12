// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//

using Foundation;

namespace Softjourn.SJCoins.iOS.UI.Controllers.HomePage
{
	partial class ShowViewController
	{
		[Outlet]
		UIKit.UISegmentedControl NamePriceSegmentControl { get; set; }

		[Outlet]
		UIKit.UIBarButtonItem SearchButton { get; set; }

		[Outlet]
		UIKit.UITableView TableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (SearchButton != null) {
				SearchButton.Dispose ();
				SearchButton = null;
			}

			if (NamePriceSegmentControl != null) {
				NamePriceSegmentControl.Dispose ();
				NamePriceSegmentControl = null;
			}

			if (TableView != null) {
				TableView.Dispose ();
				TableView = null;
			}
		}
	}
}
