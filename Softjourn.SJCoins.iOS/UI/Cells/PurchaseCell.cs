using System;
using Foundation;
using Softjourn.SJCoins.Core.Models;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Cells
{
	public partial class PurchaseCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString("PurchaseCell");
		public static readonly UINib Nib;

		static PurchaseCell()
		{
			Nib = UINib.FromName("PurchaseCell", NSBundle.MainBundle);
		}

		protected PurchaseCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public void ConfigureWith(History purchaseObj)
		{
			DateLabel.Text = purchaseObj.PrettyTime;
			NameLabel.Text = purchaseObj.Name;
			PriceLabel.Text = purchaseObj.Price.ToString();
		}

		public override void PrepareForReuse()
		{
			DateLabel.Text = string.Empty;
			NameLabel.Text = string.Empty;
			PriceLabel.Text = string.Empty;

			base.PrepareForReuse();
		}
	}
}
