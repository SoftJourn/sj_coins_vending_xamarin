// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Softjourn.SJCoins.iOS.UI.Controllers
{
	partial class DetailViewController
	{
		[Outlet]
		UIKit.UIButton BuyButton { get; set; }

		[Outlet]
		UIKit.UILabel DescriptionLabel { get; set; }

		[Outlet]
		UIKit.UIButton FavoriteButton { get; set; }

		[Outlet]
		UIKit.UIView LogoView { get; set; }

		[Outlet]
		UIKit.UILabel NameLabel { get; set; }

		[Outlet]
		UIKit.UIPageControl PageControl { get; set; }

		[Outlet]
		UIKit.UILabel PriceLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (BuyButton != null) {
				BuyButton.Dispose ();
				BuyButton = null;
			}

			if (DescriptionLabel != null) {
				DescriptionLabel.Dispose ();
				DescriptionLabel = null;
			}

			if (FavoriteButton != null) {
				FavoriteButton.Dispose ();
				FavoriteButton = null;
			}

			if (LogoView != null) {
				LogoView.Dispose ();
				LogoView = null;
			}

			if (NameLabel != null) {
				NameLabel.Dispose ();
				NameLabel = null;
			}

			if (PriceLabel != null) {
				PriceLabel.Dispose ();
				PriceLabel = null;
			}

			if (PageControl != null) {
				PageControl.Dispose ();
				PageControl = null;
			}
		}
	}
}
