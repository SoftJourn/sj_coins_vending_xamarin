using System;
using System.Collections.Generic;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.iOS.UI.Cells;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Sources.HomePage
{
    public class InternalHomeViewSource : UICollectionViewSource
    {
        // Horizontal CollectionView flowlayout delegate object.

        public List<Product> Products { get; set; } = new List<Product>();

        public override nint GetItemsCount(UICollectionView collectionView, nint section) => Products.Count;

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            return (HomeInternalCell)collectionView.DequeueReusableCell(HomeInternalCell.Key, indexPath);
        }

        protected override void Dispose(bool disposing)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("{0} disposed", this.GetType()));
            base.Dispose(disposing);
        }
    }
}
