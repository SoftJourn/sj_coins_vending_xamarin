using System;

using Foundation;
using UIKit;

namespace Softjourn.SJCoins.iOS
{
	public partial class DescriptionCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString("DescriptionCell");
		public static readonly UINib Nib;

		static DescriptionCell()
		{
			Nib = UINib.FromName("DescriptionCell", NSBundle.MainBundle);
		}

		protected DescriptionCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public void ConfigureWith(string description)
		{
			DescriptionLabel.Text = description;
		}
	}
}
