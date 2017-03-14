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
		Softjourn.SJCoins.iOS.CustomSegmentControl DateAmountSegmentControl { get; set; }

		[Outlet]
		Softjourn.SJCoins.iOS.CustomSegmentControl InputOutputSegmentControl { get; set; }

		[Outlet]
		UIKit.UILabel NoItemsLabel { get; set; }

		[Outlet]
		UIKit.UITableView TableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (NoItemsLabel != null) {
				NoItemsLabel.Dispose ();
				NoItemsLabel = null;
			}

			if (InputOutputSegmentControl != null) {
				InputOutputSegmentControl.Dispose ();
				InputOutputSegmentControl = null;
			}

			if (DateAmountSegmentControl != null) {
				DateAmountSegmentControl.Dispose ();
				DateAmountSegmentControl = null;
			}

			if (TableView != null) {
				TableView.Dispose ();
				TableView = null;
			}
		}
	}
}
