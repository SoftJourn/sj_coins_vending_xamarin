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
	partial class AccountViewController
	{
		[Outlet]
		UIKit.UILabel AmountLabel { get; set; }

		[Outlet]
		UIKit.UIBarButtonItem LogOutButton { get; set; }

		[Outlet]
		UIKit.UILabel NameLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (LogOutButton != null) {
				LogOutButton.Dispose ();
				LogOutButton = null;
			}

			if (NameLabel != null) {
				NameLabel.Dispose ();
				NameLabel = null;
			}

			if (AmountLabel != null) {
				AmountLabel.Dispose ();
				AmountLabel = null;
			}
		}
	}
}
