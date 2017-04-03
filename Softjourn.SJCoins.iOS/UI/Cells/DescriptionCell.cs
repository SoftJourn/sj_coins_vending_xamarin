using System;

using Foundation;
using Softjourn.SJCoins.iOS.General.Constants;
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
			if (String.IsNullOrEmpty(description))
			{
				DescriptionLabel.TextColor = UIColor.Gray;
				DescriptionLabel.Text = Const.defaultDescription;
			}
			else
				DescriptionLabel.Text = description;
		}
	}
}
