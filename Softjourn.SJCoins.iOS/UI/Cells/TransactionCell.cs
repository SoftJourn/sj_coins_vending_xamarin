using System;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.TransactionReports;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Cells
{
	public partial class TransactionCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString("TransactionCell");
		public static readonly UINib Nib;

		static TransactionCell()
		{
			Nib = UINib.FromName("TransactionCell", NSBundle.MainBundle);
		}

		protected TransactionCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public void ConfigureWith(Transaction item)
		{
			DateLabel.Text = item.PrettyTime;
			SenderLabel.Text = item.Account;
			ReceiverLabel.Text = item.Destination;
			AmountLabel.Text = item.Amount.ToString();
		}

		public override void PrepareForReuse()
		{
			DateLabel.Text = string.Empty;
			SenderLabel.Text = string.Empty;
			ReceiverLabel.Text = string.Empty;
			AmountLabel.Text = string.Empty;
		}
	}
}
