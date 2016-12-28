// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Softjourn.SJCoins.iOS
{
	[Register ("LoginViewController")]
	partial class LoginViewController
	{
		[Outlet]
		UIKit.UIImageView ImageLogo { get; set; }

		[Outlet]
		UIKit.UILabel LoginErrorLabel { get; set; }

		[Outlet]
		UIKit.UITextField LoginTextField { get; set; }

		[Outlet]
		UIKit.UILabel PasswordErrorLabel { get; set; }

		[Outlet]
		UIKit.UITextField PasswordTextField { get; set; }

		[Outlet]
		UIKit.UILabel VersionLabel { get; set; }

		[Action ("LoginButtonPressed:")]
		partial void LoginButtonPressed (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ImageLogo != null) {
				ImageLogo.Dispose ();
				ImageLogo = null;
			}

			if (LoginTextField != null) {
				LoginTextField.Dispose ();
				LoginTextField = null;
			}

			if (PasswordTextField != null) {
				PasswordTextField.Dispose ();
				PasswordTextField = null;
			}

			if (VersionLabel != null) {
				VersionLabel.Dispose ();
				VersionLabel = null;
			}

			if (LoginErrorLabel != null) {
				LoginErrorLabel.Dispose ();
				LoginErrorLabel = null;
			}

			if (PasswordErrorLabel != null) {
				PasswordErrorLabel.Dispose ();
				PasswordErrorLabel = null;
			}
		}
	}
}
