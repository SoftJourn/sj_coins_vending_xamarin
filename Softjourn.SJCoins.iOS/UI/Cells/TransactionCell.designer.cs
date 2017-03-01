// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Softjourn.SJCoins.iOS.UI.Cells
{
	[Register ("TransactionCell")]
	partial class TransactionCell
	{
		[Outlet]
		UIKit.UILabel AmountLabel { get; set; }

		[Outlet]
		UIKit.UILabel DateLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (DateLabel != null) {
				DateLabel.Dispose ();
				DateLabel = null;
			}

			if (AmountLabel != null) {
				AmountLabel.Dispose ();
				AmountLabel = null;
			}
		}
	}
}
