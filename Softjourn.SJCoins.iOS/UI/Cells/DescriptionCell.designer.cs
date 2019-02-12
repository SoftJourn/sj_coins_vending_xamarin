// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//

using Foundation;

namespace Softjourn.SJCoins.iOS.UI.Cells
{
	[Register ("DescriptionCell")]
	partial class DescriptionCell
	{
		[Outlet]
		UIKit.UILabel DescriptionLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (DescriptionLabel != null) {
				DescriptionLabel.Dispose ();
				DescriptionLabel = null;
			}
		}
	}
}
