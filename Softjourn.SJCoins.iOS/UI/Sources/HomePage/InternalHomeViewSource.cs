using System;
using System.Collections.Generic;
using System.Diagnostics;
using Foundation;
using Softjourn.SJCoins.Core.Models.Products;
using Softjourn.SJCoins.iOS.UI.Cells;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Sources.HomePage
{
    public class InternalHomeViewSource : UICollectionViewSource
    {
        // Horizontal CollectionView flowLayout delegate object.

        public List<Product> Products { get; set; } = new List<Product>();

        public override nint GetItemsCount(UICollectionView collectionView, nint section) => Products.Count;

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath) =>
            (HomeInternalCell) collectionView.DequeueReusableCell(HomeInternalCell.Key, indexPath);

        protected override void Dispose(bool disposing)
        {
            Debug.WriteLine($"{GetType()} disposed");
            base.Dispose(disposing);
        }
    }
}
