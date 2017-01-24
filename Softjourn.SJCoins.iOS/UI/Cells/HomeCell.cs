using System;

using Foundation;
using UIKit;

namespace Softjourn.SJCoins.iOS
{
	public partial class HomeCell : UICollectionViewCell
	{
		public static readonly NSString Key = new NSString("HomeCell");
		public static readonly UINib Nib;

		static HomeCell()
		{
			Nib = UINib.FromName("HomeCell", NSBundle.MainBundle);
		}

		protected HomeCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}
	}
}
