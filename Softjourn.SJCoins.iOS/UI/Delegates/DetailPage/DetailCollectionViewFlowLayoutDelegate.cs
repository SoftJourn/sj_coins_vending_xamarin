using System.Diagnostics;
using Softjourn.SJCoins.iOS.UI.Cells;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Delegates.DetailPage
{
    public class DetailCollectionViewFlowLayoutDelegate : BaseFlowLayoutDelegate
    {
        // Main screen Horizontal CollectionView flowLayout delegate object.

        public override void WillDisplayCell(UICollectionView collectionView, UICollectionViewCell cell, Foundation.NSIndexPath indexPath)
        {
            var _cell = (ImageCell)cell;
            _cell.ConfigureWith(Images[indexPath.Row]);
        }

        protected override void Dispose(bool disposing)
        {
            Debug.WriteLine($"{GetType()} disposed");
            base.Dispose(disposing);
        }
    }
}
