using System;
using System.Collections.Generic;
using CoreGraphics;
using Softjourn.SJCoins.iOS.General.Helper;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Delegates.DetailPage
{
    public class BaseFlowLayoutDelegate : UICollectionViewDelegateFlowLayout
    {
        public event EventHandler<string> SelectedItem;
        public event EventHandler<int> VisibleItem;

        public List<string> Images { get; set; } = new List<string>();

        public override CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, Foundation.NSIndexPath indexPath)
        {
            return new CGSize(collectionView.Bounds.Width, collectionView.Bounds.Height);
        }

        public override void ItemSelected(UICollectionView collectionView, Foundation.NSIndexPath indexPath)
        {
            SelectedItem?.Invoke(this, Images[indexPath.Row]);
        }

        public override void DecelerationEnded(UIScrollView scrollView)
        {
            var offset = scrollView.ContentOffset;
            var itemIndex = offset.X / SizeHelper.MainBounds.Width;
            VisibleItem?.Invoke(this, (int)itemIndex);
        }
    }
}
