using System;
using System.Collections.Generic;
using UIKit;

namespace Softjourn.SJCoins.iOS
{
    public class BaseImageSource: UICollectionViewSource
    {
		public List<string> Images { get; set; } = new List<string>();

		public override nint GetItemsCount(UICollectionView collectionView, nint section) => Images.Count;
	}
}
