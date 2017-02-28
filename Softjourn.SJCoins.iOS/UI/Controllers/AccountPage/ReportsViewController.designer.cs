// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Softjourn.SJCoins.iOS.UI.Controllers.AccountPage
{
	partial class ReportsViewController
	{
		[Outlet]
		UIKit.UIButton FilterButton { get; set; }

		[Outlet]
		UIKit.UILabel NoItemsLabel { get; set; }

		[Outlet]
		UIKit.UISegmentedControl SegmentControl { get; set; }

		[Outlet]
		UIKit.UITableView TableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (NoItemsLabel != null) {
				NoItemsLabel.Dispose ();
				NoItemsLabel = null;
			}

			if (TableView != null) {
				TableView.Dispose ();
				TableView = null;
			}

			if (FilterButton != null) {
				FilterButton.Dispose ();
				FilterButton = null;
			}

			if (SegmentControl != null) {
				SegmentControl.Dispose ();
				SegmentControl = null;
			}
		}
	}
}
