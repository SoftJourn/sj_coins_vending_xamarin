using System;
using System.Collections.Generic;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.iOS.UI.Cells;
using UIKit;

namespace Softjourn.SJCoins.iOS
{
    public class ShowAllSource: UICollectionViewSource, IDisposable
    {
		public List<Product> Products { get; set; } = new List<Product>();

        public override nint GetItemsCount(UICollectionView collectionView, nint section) //=> Products.Count;
        {
            return Products.Count;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, Foundation.NSIndexPath indexPath) 
        {
            return (ProductCell)collectionView.DequeueReusableCell(ProductCell.Key, indexPath);
        }

		protected override void Dispose(bool disposing)
		{
			System.Diagnostics.Debug.WriteLine(String.Format("{0} disposed", this.GetType()));
			base.Dispose(disposing);
		}
    }
}
