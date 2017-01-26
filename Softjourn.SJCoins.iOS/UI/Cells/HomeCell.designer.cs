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
	[Register ("HomeCell")]
	partial class HomeCell
	{
		[Outlet]
		UIKit.UILabel CategoryNameLabel { get; set; }

		[Outlet]
		UIKit.UICollectionView InternalCollectionView { get; set; }

		[Outlet]
		UIKit.UIButton ShowAllButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (CategoryNameLabel != null) {
				CategoryNameLabel.Dispose ();
				CategoryNameLabel = null;
			}

			if (InternalCollectionView != null) {
				InternalCollectionView.Dispose ();
				InternalCollectionView = null;
			}

			if (ShowAllButton != null) {
				ShowAllButton.Dispose ();
				ShowAllButton = null;
			}
		}
	}
}
