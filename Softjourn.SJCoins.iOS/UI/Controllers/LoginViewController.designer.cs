// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Softjourn.SJCoins.iOS.UI.Controllers
{
    [Register ("LoginViewController")]
    partial class LoginViewController
    {
        [Outlet]
        UIKit.UIImageView ImageLogo { get; set; }


        [Outlet]
        UIKit.UIButton LoginButton { get; set; }


        [Outlet]
        UIKit.UILabel LoginErrorLabel { get; set; }


        [Outlet]
        UIKit.UITextField LoginTextField { get; set; }


        [Outlet]
        UIKit.UILabel PasswordErrorLabel { get; set; }


        [Outlet]
        UIKit.UITextField PasswordTextField { get; set; }


        [Outlet]
        UIKit.UIView ScrollView { get; set; }


        [Outlet]
        UIKit.UILabel VersionLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel versionLabel { get; set; }


        [Action ("LoginButtonPressed:")]
        partial void LoginButtonPressed (UIKit.UIButton sender);


        [Action ("LoginTextFieldDidChange:")]
        partial void LoginTextFieldDidChange (UIKit.UITextField sender);

        void ReleaseDesignerOutlets ()
        {
            if (ImageLogo != null) {
                ImageLogo.Dispose ();
                ImageLogo = null;
            }

            if (LoginButton != null) {
                LoginButton.Dispose ();
                LoginButton = null;
            }

            if (LoginErrorLabel != null) {
                LoginErrorLabel.Dispose ();
                LoginErrorLabel = null;
            }

            if (LoginTextField != null) {
                LoginTextField.Dispose ();
                LoginTextField = null;
            }

            if (PasswordErrorLabel != null) {
                PasswordErrorLabel.Dispose ();
                PasswordErrorLabel = null;
            }

            if (PasswordTextField != null) {
                PasswordTextField.Dispose ();
                PasswordTextField = null;
            }

            if (ScrollView != null) {
                ScrollView.Dispose ();
                ScrollView = null;
            }

            if (versionLabel != null) {
                versionLabel.Dispose ();
                versionLabel = null;
            }

            if (VersionLabel != null) {
                VersionLabel.Dispose ();
                VersionLabel = null;
            }
        }
    }
}