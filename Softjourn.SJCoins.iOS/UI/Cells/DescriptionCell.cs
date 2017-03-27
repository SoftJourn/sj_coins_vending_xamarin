using System;

using Foundation;
using UIKit;

namespace Softjourn.SJCoins.iOS
{
	public partial class DescriptionCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString("DescriptionCell");
		public static readonly UINib Nib;

		private const string defaultDescription = "No description to current product."; 

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
				DescriptionLabel.Text = defaultDescription;
			}
			else
				DescriptionLabel.Text = description;
		}
	}
}
