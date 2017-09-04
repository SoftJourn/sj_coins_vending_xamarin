using UIKit;
using CoreGraphics;
using Softjourn.SJCoins.iOS.General.Constants;
using System;
using System.Collections.Generic;
using Softjourn.SJCoins.Core.API.Model.Products;

namespace Softjourn.SJCoins.iOS.UI.Delegates
{
    public class CollectionViewFlowLayoutDelegate: UICollectionViewDelegateFlowLayout, IDisposable
    {
        // Horizontal CollectionView flowlayout delegate object.

		public List<Product> Products { get; set; } = new List<Product>();
		public event EventHandler<Product> SelectedItem;

		public override CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, Foundation.NSIndexPath indexPath)
        {
            return new CGSize(collectionView.Bounds.Width / Const.widthCoefficient, collectionView.Bounds.Height);
        }

        public override void ItemSelected(UICollectionView collectionView, Foundation.NSIndexPath indexPath)
        {
            SelectedItem?.Invoke(this, Products[indexPath.Row]);
        }

		protected override void Dispose(bool disposing)
		{
			System.Diagnostics.Debug.WriteLine(String.Format("{0} disposed", this.GetType()));
			base.Dispose(disposing);
		}
    }
}
