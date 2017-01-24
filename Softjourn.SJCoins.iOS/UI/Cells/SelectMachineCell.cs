using System;

using Foundation;
using UIKit;

namespace Softjourn.SJCoins.iOS
{
	public partial class SelectMachineCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString("SelectMachineCell");
		public static readonly UINib Nib;

		static SelectMachineCell()
		{
			Nib = UINib.FromName("SelectMachineCell", NSBundle.MainBundle);
		}

		protected SelectMachineCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}
	}
}
