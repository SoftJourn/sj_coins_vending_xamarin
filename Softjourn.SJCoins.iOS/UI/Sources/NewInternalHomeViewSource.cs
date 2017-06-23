﻿using System;
using System.Collections.Generic;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.iOS.General.Constants;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Sources
{
	public class NewInternalHomeViewSource : UICollectionViewSource, IDisposable
	{
		public List<Product> Products { get; set; } = new List<Product>();
		public string CategoryName { get; set; }
		public event EventHandler<Product> NewInternalHomeViewSource_ItemSelected;
		public event EventHandler<Product> NewInternalHomeViewSource_BuyActionExecuted;
		public event EventHandler<Product> NewInternalHomeViewSource_FavoriteActionExecuted;

		public override nint GetItemsCount(UICollectionView collectionView, nint section) => Products.Count;

		public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
		{
			var cell = (HomeInternalCell)collectionView.DequeueReusableCell(HomeInternalCell.Key, indexPath);
			cell.SetUpUI();
			return cell;
		}

		public override void WillDisplayCell(UICollectionView collectionView, UICollectionViewCell cell, NSIndexPath indexPath)
		{
			var _cell = (HomeInternalCell)cell;
			_cell.Fill(Products[indexPath.Row]);

			if (CategoryName == Const.FavoritesCategory)
			{
				_cell.MarkFavorites(Products[indexPath.Row]);
			}
			_cell.HomeInternalCell_BuyActionExecuted -= NewInternalHomeViewSource_BuyActionExecuted;
			_cell.HomeInternalCell_BuyActionExecuted += NewInternalHomeViewSource_BuyActionExecuted;

			_cell.HomeInternalCell_FavoriteActionExecuted -= NewInternalHomeViewSource_FavoriteActionExecuted;
			_cell.HomeInternalCell_FavoriteActionExecuted += NewInternalHomeViewSource_FavoriteActionExecuted;
            _cell.Fill(Products[indexPath.Row]);
		}

		public override void CellDisplayingEnded(UICollectionView collectionView, UICollectionViewCell cell, NSIndexPath indexPath)
		{
			var _cell = (HomeInternalCell)cell;
			_cell.HomeInternalCell_BuyActionExecuted -= NewInternalHomeViewSource_BuyActionExecuted;
			_cell.HomeInternalCell_FavoriteActionExecuted -= NewInternalHomeViewSource_FavoriteActionExecuted;
		}

		public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
		{
			NewInternalHomeViewSource_ItemSelected?.Invoke(this, Products[indexPath.Row]);
		}

		protected override void Dispose(bool disposing)
		{
			System.Diagnostics.Debug.WriteLine(String.Format("{0} object disposed", this.GetType()));
			base.Dispose(disposing);
		}
	}
}
