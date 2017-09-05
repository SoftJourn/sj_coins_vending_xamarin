using System;
using System.Collections.Generic;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.iOS.General.Constants;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Sources
{
	public class InternalHomeViewSource : UICollectionViewSource, IDisposable
	{
		// Horizontal CollectionView flowlayout delegate object.

		public List<Product> Products { get; set; } = new List<Product>();
		//public string CategoryName { get; set; }

		public override nint GetItemsCount(UICollectionView collectionView, nint section) => Products.Count;

		public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
		{
			//var cell = (HomeInternalCell)collectionView.DequeueReusableCell(HomeInternalCell.Key, indexPath);
			//var item = Products[indexPath.Row];
			//cell.ConfigureWith(item);

			//if (CategoryName == Const.FavoritesCategory)
				//cell.MarkFavorites(item);

            return (HomeInternalCell)collectionView.DequeueReusableCell(HomeInternalCell.Key, indexPath);
		}

		protected override void Dispose(bool disposing)
		{
			System.Diagnostics.Debug.WriteLine(String.Format("{0} disposed", this.GetType()));
			base.Dispose(disposing);
		}
	}
}
