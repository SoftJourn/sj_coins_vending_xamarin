﻿using System;

using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using UIKit;

namespace Softjourn.SJCoins.iOS
{
	public partial class HomeInternalCell : UICollectionViewCell
	{
		public static readonly NSString Key = new NSString("HomeInternalCell");
		public static readonly UINib Nib;

		static HomeInternalCell()
		{
			Nib = UINib.FromName("HomeInternalCell", NSBundle.MainBundle);
		}

		protected HomeInternalCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public void ConfigureWith(Product product)
		{
			
		}
	}
}
