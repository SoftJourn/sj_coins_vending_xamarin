using System;

using Foundation;
using UIKit;

namespace Softjourn.SJCoins.iOS
{
	public partial class ProductCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString("ProductCell");
		public static readonly UINib Nib;

		static ProductCell()
		{
			Nib = UINib.FromName("ProductCell", NSBundle.MainBundle);
		}

		protected ProductCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}
	}
}
