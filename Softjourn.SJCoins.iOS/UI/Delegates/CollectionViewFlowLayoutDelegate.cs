﻿using UIKit;
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

		public event EventHandler<Product> SelectedItem;
		public event EventHandler<Product> BuyActionExecuted;
		public event EventHandler<Product> FavoriteActionExecuted;

		public List<Product> Products { get; set; } = new List<Product>();
		public string CategoryName { get; set; }

		public override CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, Foundation.NSIndexPath indexPath)
        {
            var cellWidth = new SizeHelper().HorizontalCellWidth(collectionView.Bounds.Width);
            return new CGSize(cellWidth, collectionView.Bounds.Height);
        }

        public override void WillDisplayCell(UICollectionView collectionView, UICollectionViewCell cell, Foundation.NSIndexPath indexPath)
        {
			var _cell = (HomeInternalCell)cell;
			_cell.ConfigureWith(Products[indexPath.Row]);

			if (CategoryName == Const.FavoritesCategory)
			{
				_cell.MarkFavorites(Products[indexPath.Row]);
			}
            _cell.BuyAction -= BuyActionExecuted;
            _cell.BuyAction += BuyActionExecuted;

            _cell.FavoriteAction -= FavoriteActionExecuted;
            _cell.FavoriteAction += FavoriteActionExecuted;
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
