using System;
using Foundation;
using UIKit;

namespace Softjourn.SJCoins.iOS
{
    public class DetailCollectionViewSource: BaseImageSource, IDisposable
    {
		// Detail screen Horizontal Image CollectionView.

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath) => (ImageCell)collectionView.DequeueReusableCell(ImageCell.Key, indexPath);

		protected override void Dispose(bool disposing)
		{
			System.Diagnostics.Debug.WriteLine(String.Format("{0} disposed", this.GetType()));
			base.Dispose(disposing);
		}
    }
}
