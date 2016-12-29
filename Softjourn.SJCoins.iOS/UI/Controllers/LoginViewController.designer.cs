// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Softjourn.SJCoins.iOS
{
    [Register ("LoginViewController")]
    partial class LoginViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imageLogo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Softjourn.SJCoins.iOS.CustomLoginUIButton loginButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel loginErrorLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField loginTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel passwordErrorLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField passwordTexField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView scrollView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel versionLabel { get; set; }

        [Action ("LoginButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void LoginButton_TouchUpInside (Softjourn.SJCoins.iOS.CustomLoginUIButton sender);

        [Action ("loginTextFieldDidChange:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void loginTextFieldDidChange (UIKit.UITextField sender);

        [Action ("passwordTextFieldDidChange:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void passwordTextFieldDidChange (UIKit.UITextField sender);

        void ReleaseDesignerOutlets ()
        {
            if (imageLogo != null) {
                imageLogo.Dispose ();
                imageLogo = null;
            }

            if (loginButton != null) {
                loginButton.Dispose ();
                loginButton = null;
            }

            if (loginErrorLabel != null) {
                loginErrorLabel.Dispose ();
                loginErrorLabel = null;
            }

            if (loginTextField != null) {
                loginTextField.Dispose ();
                loginTextField = null;
            }

            if (passwordErrorLabel != null) {
                passwordErrorLabel.Dispose ();
                passwordErrorLabel = null;
            }

            if (passwordTexField != null) {
                passwordTexField.Dispose ();
                passwordTexField = null;
            }

            if (scrollView != null) {
                scrollView.Dispose ();
                scrollView = null;
            }

            if (versionLabel != null) {
                versionLabel.Dispose ();
                versionLabel = null;
            }
        }
    }
}