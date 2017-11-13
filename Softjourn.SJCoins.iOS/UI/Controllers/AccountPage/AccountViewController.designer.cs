// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Softjourn.SJCoins.iOS.UI.Controllers.AccountPage
{
    partial class AccountViewController
    {
        [Outlet]
        UIKit.UILabel AmountLabel { get; set; }

        [Outlet]
        UIKit.UIImageView AvatarImage { get; set; }

        [Outlet]
        UIKit.UIButton DoneButton { get; set; }

        [Outlet]
        UIKit.UIView HeaderView { get; set; }

        [Outlet]
        UIKit.UILabel NameLabel { get; set; }

        [Outlet]
        UIKit.UITableView TableView { get; set; }
        
        void ReleaseDesignerOutlets ()
        {
            if (AmountLabel != null) {
                AmountLabel.Dispose ();
                AmountLabel = null;
            }

            if (AvatarImage != null) {
                AvatarImage.Dispose ();
                AvatarImage = null;
            }

            if (DoneButton != null) {
                DoneButton.Dispose ();
                DoneButton = null;
            }

            if (HeaderView != null) {
                HeaderView.Dispose ();
                HeaderView = null;
            }

            if (NameLabel != null) {
                NameLabel.Dispose ();
                NameLabel = null;
            }

            if (TableView != null) {
                TableView.Dispose ();
                TableView = null;
            }
        }
    }
}
