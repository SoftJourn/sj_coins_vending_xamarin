﻿using System;
using System.Collections.Generic;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using UIKit;

namespace Softjourn.SJCoins.iOS
{
	public partial class HomeCell : UICollectionViewCell
	{
		public static readonly NSString Key = new NSString("HomeCell");
		public static readonly UINib Nib;

		private string categoryName; 

		static HomeCell()
		{
			Nib = UINib.FromName("HomeCell", NSBundle.MainBundle);
		}

		protected HomeCell(IntPtr handle) : base(handle)
		{
		}

		public void ConfigureWith(Categories category, HomeCellDelegate _delegate)
		{
			// Save and set category name
			categoryName = category.Name;
			CategoryNameLabel.Text = category.Name;

			InternalCollectionView.DataSource = new HomeCellDataSource(category.Products);
			InternalCollectionView.Delegate = _delegate;
			InternalCollectionView.ReloadData();
		}
	}

	#region UICollectionViewSource implementation
	public class HomeCellDataSource : UICollectionViewDataSource
	{
		private List<Product> _products;

		public HomeCellDataSource(List<Product> products)
		{
			_products = products;
		}

		public override nint GetItemsCount(UICollectionView collectionView, nint section) => _products == null ? 0 : _products.Count;

		public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath) => (UICollectionViewCell)collectionView.DequeueReusableCell(HomeInternalCell.Key, indexPath);
	}

	#endregion

	#region UICollectionViewDelegate implementation
	public class HomeCellDelegate : UICollectionViewDelegate
	{
		private List<Product> _products;
		public event EventHandler<Product> ItemSelectedEvent = delegate { };

		public HomeCellDelegate(List<Product> products)
		{
			_products = products;
		}

		public override void WillDisplayCell(UICollectionView collectionView, UICollectionViewCell cell, NSIndexPath indexPath)
		{
			var _cell = cell as HomeInternalCell;
			var item = _products[indexPath.Row];
			_cell.ConfigureWith(item);
		}

		public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
		{
			ItemSelectedEvent(this, _products[indexPath.Row]);
		}
	}
	#endregion
}
