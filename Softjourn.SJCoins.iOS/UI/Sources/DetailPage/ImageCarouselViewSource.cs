using System.Diagnostics;
using Foundation;
using Softjourn.SJCoins.iOS.UI.Cells;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Sources.DetailPage
{
    public class ImageCarouselViewSource : BaseImageSource
    {
        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath) =>
            (ImageDetailCell)collectionView.DequeueReusableCell(ImageDetailCell.Key, indexPath);

        protected override void Dispose(bool disposing)
        {
            Debug.WriteLine($"{GetType()} disposed");
            base.Dispose(disposing);
        }
    }
}
