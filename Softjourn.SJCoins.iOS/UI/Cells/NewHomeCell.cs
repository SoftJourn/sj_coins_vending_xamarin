using System;
using UIKit;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.iOS.UI.Sources;

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

		public void ConfigureWith(Categories category, NewInternalHomeViewSource source, int row)
		{
			// Set category name
			NameLabel.Text = category.Name;
			// Set products which need to be displayed 
			source.Products = category.Products;
			// Configure CollectionView
			CollectionView.Source = source;
			CollectionView.Tag = row;
			CollectionView.ReloadData();
		}

		public override void PrepareForReuse()
		{
			NameLabel.Text = "";
			CollectionView.Source = null;
		}
	}
}
