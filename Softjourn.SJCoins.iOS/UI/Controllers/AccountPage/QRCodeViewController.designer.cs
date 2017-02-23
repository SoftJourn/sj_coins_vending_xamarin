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
	partial class QRCodeViewController
	{
		[Outlet]
		UIKit.UITextField AmountTexfield { get; set; }

		[Outlet]
		UIKit.UILabel BalanceLabel { get; set; }

		[Outlet]
		UIKit.UIBarButtonItem DoneButton { get; set; }

		[Outlet]
		UIKit.UILabel ErrorLabel { get; set; }

		[Outlet]
		UIKit.UIButton GenerateButton { get; set; }

		[Outlet]
		UIKit.UIImageView QRCodeImage { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (AmountTexfield != null) {
				AmountTexfield.Dispose ();
				AmountTexfield = null;
			}

			if (BalanceLabel != null) {
				BalanceLabel.Dispose ();
				BalanceLabel = null;
			}

			if (DoneButton != null) {
				DoneButton.Dispose ();
				DoneButton = null;
			}

			if (ErrorLabel != null) {
				ErrorLabel.Dispose ();
				ErrorLabel = null;
			}

			if (GenerateButton != null) {
				GenerateButton.Dispose ();
				GenerateButton = null;
			}

			if (QRCodeImage != null) {
				QRCodeImage.Dispose ();
				QRCodeImage = null;
			}
		}
	}
}
