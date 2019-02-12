// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//

using Foundation;

namespace Softjourn.SJCoins.iOS.UI.Cells
{
	[Register ("AccountCell")]
	partial class AccountCell
	{
		[Outlet]
		UIKit.UIImageView ImageLogo { get; set; }

		[Outlet]
		UIKit.UILabel TitleLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (TitleLabel != null) {
				TitleLabel.Dispose ();
				TitleLabel = null;
			}

			if (ImageLogo != null) {
				ImageLogo.Dispose ();
				ImageLogo = null;
			}
		}
	}
}
