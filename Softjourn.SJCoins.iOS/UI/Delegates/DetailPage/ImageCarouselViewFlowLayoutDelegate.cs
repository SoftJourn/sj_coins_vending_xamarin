using System;
using UIKit;

namespace Softjourn.SJCoins.iOS
{
    public class ImageCarouselViewFlowLayoutDelegate: BaseFlowLayoutDelegate, IDisposable
    {
		// Main screen Horizontal CollectionView flowlayout delegate object.

		public override void WillDisplayCell(UICollectionView collectionView, UICollectionViewCell cell, Foundation.NSIndexPath indexPath)
		{
			var _cell = (ImageDetailCell)cell;
			_cell.ConfigureWith(Images[indexPath.Row]);
		}

		protected override void Dispose(bool disposing)
		{
			System.Diagnostics.Debug.WriteLine(String.Format("{0} disposed", this.GetType()));
			base.Dispose(disposing);
		}
    }
}
