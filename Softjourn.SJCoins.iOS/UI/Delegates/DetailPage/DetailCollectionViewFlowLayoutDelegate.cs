using System;
using UIKit;

namespace Softjourn.SJCoins.iOS
{
    public class DetailCollectionViewFlowLayoutDelegate: BaseFlowLayoutDelegate
    {
		// Main screen Horizontal CollectionView flowlayout delegate object.

		public override void WillDisplayCell(UICollectionView collectionView, UICollectionViewCell cell, Foundation.NSIndexPath indexPath)
		{
            var _cell = (ImageCell)cell;
            _cell.ConfigureWith(Images[indexPath.Row]);
		}

		protected override void Dispose(bool disposing)
		{
			System.Diagnostics.Debug.WriteLine(String.Format("{0} disposed", this.GetType()));
			base.Dispose(disposing);
		}
    }
}
