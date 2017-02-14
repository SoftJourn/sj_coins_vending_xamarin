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
	[Register ("ProductCell")]
	partial class ProductCell
	{
		[Outlet]
		UIKit.UIButton FavoriteButton { get; set; }

		[Outlet]
		UIKit.UIImageView ImageLogo { get; set; }

		[Outlet]
		UIKit.UILabel NameLabel { get; set; }

		[Outlet]
		UIKit.UILabel PriceLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (FavoriteButton != null) {
				FavoriteButton.Dispose ();
				FavoriteButton = null;
			}

			if (ImageLogo != null) {
				ImageLogo.Dispose ();
				ImageLogo = null;
			}

			if (NameLabel != null) {
				NameLabel.Dispose ();
				NameLabel = null;
			}

			if (PriceLabel != null) {
				PriceLabel.Dispose ();
				PriceLabel = null;
			}
		}
	}
}
