﻿using System;

using Foundation;
using UIKit;

namespace Softjourn.SJCoins.iOS
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

		public void ConfigureWith(string item)
		{
			TitleLabel.Text = item;
		}
	}
}
