using System;
using Foundation;
using Softjourn.SJCoins.Core.Models;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Cells
{
	public partial class AccountCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString("AccountCell");
		public static readonly UINib Nib;

		static AccountCell()
		{
			Nib = UINib.FromName("AccountCell", NSBundle.MainBundle);
		}

		protected AccountCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public void ConfigureWith(AccountOption item)
		{
			TitleLabel.Text = item.OptionName;
			ImageLogo.Image = UIImage.FromBundle(item.OptionIconName);
		}
	}
}
