using System;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.TransactionReports;
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

		public void ConfigureWith(Transaction item)
		{
			DateLabel.Text = item.PrettyTime;
			SenderLabel.Text = item.Account;
			ReceiverLabel.Text = item.Destination;
			AmountLabel.Text = item.Amount.ToString() + " Coins";
		}

		public override void PrepareForReuse()
		{
			DateLabel.Text = "";
			SenderLabel.Text = "";
			ReceiverLabel.Text = "";
			AmountLabel.Text = "";
		}
	}
}
