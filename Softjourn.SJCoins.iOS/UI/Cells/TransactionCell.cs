using System;

using Foundation;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Cells
{
	public partial class TransactionCell : UITableViewCell
	{
		#region Properties
		public static readonly NSString Key = new NSString("TransactionCell");
		public static readonly UINib Nib;

		static TransactionCell()
		{
			Nib = UINib.FromName("TransactionCell", NSBundle.MainBundle);
		}
		#endregion

		#region Constructor
		protected TransactionCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}
		#endregion

		public void ConfigureWith() //Product item)
		{
			
		}

		public override void PrepareForReuse()
		{
			
		}
	}
}
