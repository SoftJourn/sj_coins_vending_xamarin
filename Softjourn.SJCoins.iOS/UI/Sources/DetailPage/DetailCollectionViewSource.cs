using Foundation;
using Softjourn.SJCoins.iOS.UI.Cells;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Sources.DetailPage
{
    public class DetailCollectionViewSource : BaseImageSource
    {
        // Detail screen Horizontal Image CollectionView.

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath) => (ImageCell)collectionView.DequeueReusableCell(ImageCell.Key, indexPath);

        protected override void Dispose(bool disposing)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("{0} disposed", this.GetType()));
            base.Dispose(disposing);
        }
    }
}
