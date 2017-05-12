using System;
using System.Collections.Generic;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.iOS.General.Constants;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Sources
{
	public class NewInternalHomeViewSource : UICollectionViewSource, IDisposable
	{
		public List<Product> Products { get; set; } = new List<Product>();
		public string CategoryName { get; set; }
		public event EventHandler<Product> HomeCellDelegate_ItemSelected;

		public override nint GetItemsCount(UICollectionView collectionView, nint section) => Products.Count;

		public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
		{
			var cell = (HomeInternalCell)collectionView.DequeueReusableCell(HomeInternalCell.Key, indexPath);
			var item = Products[indexPath.Row];
			cell.ConfigureWith(item);

			//if (CategoryName == Const.FavoritesCategory)
			//	cell.MarkFavorites(item);

			return cell;
		}
		public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
		{
			HomeCellDelegate_ItemSelected?.Invoke(this, Products[indexPath.Row]);
		}

		protected override void Dispose(bool disposing)
		{
			System.Diagnostics.Debug.WriteLine(String.Format("{0} object disposed", this.GetType()));
			base.Dispose(disposing);
		}
	}
}
