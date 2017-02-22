using System;

using Foundation;
using Softjourn.SJCoins.Core.API.Model;
using UIKit;

namespace Softjourn.SJCoins.iOS
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
			PriceLabel.Text = purchaseObj.Price.ToString() + " coins";
		}

		public override void PrepareForReuse()
		{
			DateLabel.Text = "";
			NameLabel.Text = "";
			PriceLabel.Text = "";
			base.PrepareForReuse();
		}
	}
}
