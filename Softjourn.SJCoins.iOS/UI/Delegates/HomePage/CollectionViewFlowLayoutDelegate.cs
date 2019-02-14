using UIKit;
using CoreGraphics;
using Softjourn.SJCoins.iOS.General.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Softjourn.SJCoins.Core.Models.Products;
using Softjourn.SJCoins.iOS.General.Helper;
using Softjourn.SJCoins.iOS.UI.Cells;

namespace Softjourn.SJCoins.iOS.UI.Delegates
{
    public class CollectionViewFlowLayoutDelegate : UICollectionViewDelegateFlowLayout
    {
        // Main screen Horizontal CollectionView flowLayout delegate object.

        public event EventHandler<Product> SelectedItem;
        public event EventHandler<Product> BuyActionExecuted;
        public event EventHandler<Product> FavoriteActionExecuted;

        public List<Product> Products { get; set; } = new List<Product>();

        public string CategoryName { get; set; }

        public override CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout,
            Foundation.NSIndexPath indexPath) =>
            new CGSize(SizeHelper.HorizontalCellWidth(), collectionView.Bounds.Height);

        public override void WillDisplayCell(UICollectionView collectionView, UICollectionViewCell cell, Foundation.NSIndexPath indexPath)
        {
            var _cell = (HomeInternalCell)cell;
            _cell.ConfigureWith(Products[indexPath.Row]);

            if (CategoryName == Const.FavoritesCategory)
                _cell.MarkFavorites(Products[indexPath.Row]);
        }

        public override void ItemSelected(UICollectionView collectionView, Foundation.NSIndexPath indexPath) =>
            SelectedItem?.Invoke(this, Products[indexPath.Row]);

        protected override void Dispose(bool disposing)
        {
            Debug.WriteLine($"{GetType()} disposed");
            base.Dispose(disposing);
        }
    }
}
