// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//

using Foundation;

namespace Softjourn.SJCoins.iOS.UI.Cells
{
	[Register ("HomeCell")]
	partial class HomeCell
	{
		[Outlet]
		UIKit.UICollectionView CollectionView { get; set; }

		[Outlet]
		UIKit.UILabel NameLabel { get; set; }

		[Outlet]
		UIKit.UIButton ShowAllButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (CollectionView != null) {
				CollectionView.Dispose ();
				CollectionView = null;
			}

			if (NameLabel != null) {
				NameLabel.Dispose ();
				NameLabel = null;
			}

			if (ShowAllButton != null) {
				ShowAllButton.Dispose ();
				ShowAllButton = null;
			}
		}
	}
}
