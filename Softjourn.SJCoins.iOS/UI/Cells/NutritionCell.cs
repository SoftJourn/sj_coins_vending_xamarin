using System;

using Foundation;
using UIKit;

namespace Softjourn.SJCoins.iOS
{
	public partial class NutritionCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString("NutritionCell");
		public static readonly UINib Nib;

		static NutritionCell()
		{
			Nib = UINib.FromName("NutritionCell", NSBundle.MainBundle);
		}

		protected NutritionCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public void ConfigureWith()
		{
			
		}
	}
}
