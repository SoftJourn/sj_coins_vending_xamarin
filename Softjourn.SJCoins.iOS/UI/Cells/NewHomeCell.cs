using System;

using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.iOS.UI.Sources;
using UIKit;

namespace Softjourn.SJCoins.iOS
{
	public partial class NewHomeCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString("NewHomeCell");
		public static readonly UINib Nib;

		static NewHomeCell()
		{
			Nib = UINib.FromName("NewHomeCell", NSBundle.MainBundle);
		}

		protected NewHomeCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}
		public void ConfigureWith(Categories category)
		{
			NameLabel.Text = category.Name;
		}

		public void setCollectionViewDataSourceAndDelegate(NewHomeViewSource source, int row)
		{
			//CollectionView.Source = source;
			CollectionView.Tag = row;
			CollectionView.ReloadData();
		}

		public override void PrepareForReuse()
		{
			
		}
	}
}
