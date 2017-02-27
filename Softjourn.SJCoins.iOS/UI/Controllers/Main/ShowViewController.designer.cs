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
	partial class ShowViewController
	{
		[Outlet]
		UIKit.UIBarButtonItem SearchButton { get; set; }

		[Outlet]
		UIKit.UISegmentedControl SegmentControl { get; set; }

		[Outlet]
		UIKit.UITableView TableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (SegmentControl != null) {
				SegmentControl.Dispose ();
				SegmentControl = null;
			}

			if (TableView != null) {
				TableView.Dispose ();
				TableView = null;
			}

			if (SearchButton != null) {
				SearchButton.Dispose ();
				SearchButton = null;
			}
		}
	}
}
