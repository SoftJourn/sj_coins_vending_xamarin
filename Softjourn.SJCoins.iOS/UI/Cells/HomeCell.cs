using System;
using System.Collections.Generic;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using UIKit;

namespace Softjourn.SJCoins.iOS
{
	public partial class HomeCell : UICollectionViewCell
	{
		public static readonly NSString Key = new NSString("HomeCell");
		public static readonly UINib Nib;

		public event EventHandler<Product> ItemSelected;

		private string categoryName; 
		private List<Product> categoryProducts;

		static HomeCell()
		{
			Nib = UINib.FromName("HomeCell", NSBundle.MainBundle);
		}

		protected HomeCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public void ConfigureWith(Categories category)
		{
			// Save and set category name
			categoryName = category.Name;
			CategoryNameLabel.Text = category.Name;
			// Save list of products
			categoryProducts = category.Products;
		}

		#region UICollectionViewSource implementation
		private class HomeCellDataSource : UICollectionViewSource
		{
			private HomeCell parent;

			public HomeCellDataSource(HomeCell parent)
			{
				this.parent = parent;
			}

			public override nint NumberOfSections(UICollectionView collectionView) => parent.categoryProducts == null ? 0 : parent.categoryProducts.Count;

			public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath) => collectionView.DequeueReusableCell(HomeInternalCell.Key, indexPath) as UICollectionViewCell;
		}

		#endregion

		#region UICollectionViewDelegate implementation
		private class HomeCellDelegate : UICollectionViewDelegate
		{
			private HomeCell parent;

			public HomeCellDelegate(HomeCell parent)
			{
				this.parent = parent;
			}

			public override void WillDisplayCell(UICollectionView collectionView, UICollectionViewCell cell, NSIndexPath indexPath)
			{
				var _cell = cell as HomeInternalCell;
				var item = parent.categoryProducts[indexPath.Row];
				_cell.ConfigureWith(item)
			}

			public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
			{
				var selectedItem = parent.categoryProducts[indexPath.Row];
				var handler = ItemSelected;
				if (handler != null)
				{
					handler(this, selectedItem);
				}
			}
		}
		#endregion

	}
}
