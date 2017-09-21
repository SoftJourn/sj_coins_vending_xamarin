using System;
using System.Collections.Generic;
using CoreGraphics;
using Softjourn.SJCoins.Core.API.Model.Products;
using UIKit;

namespace Softjourn.SJCoins.iOS
{
    public class ShowAllCollectionFlowLayoutDelegate: UICollectionViewDelegateFlowLayout, IDisposable
    {
		// Horizontal CollectionView flowlayout delegate object.

		public event EventHandler<Product> SelectedItem;
		//public event EventHandler<Product> BuyActionExecuted;
		//public event EventHandler<Product> FavoriteActionExecuted;

		public List<Product> Products { get; set; } = new List<Product>();
		public string CategoryName { get; set; }

		public override CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, Foundation.NSIndexPath indexPath)
		{
            return new CGSize(SizeHelper.ShowAllItemWidth(), SizeHelper.ShowAllItemWidth());
		}

		public override void WillDisplayCell(UICollectionView collectionView, UICollectionViewCell cell, Foundation.NSIndexPath indexPath)
		{
			//var _cell = (HomeInternalCell)cell;
			//_cell.ConfigureWith(Products[indexPath.Row]);

			//if (CategoryName == Const.FavoritesCategory)
			//{
			//	_cell.MarkFavorites(Products[indexPath.Row]);
			//}
			//_cell.BuyAction -= BuyActionExecuted;
			//_cell.BuyAction += BuyActionExecuted;

			//_cell.FavoriteAction -= FavoriteActionExecuted;
			//_cell.FavoriteAction += FavoriteActionExecuted;
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


//public override nint RowsInSection(UITableView tableview, nint section) => items.Count;

//public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) => tableView.DequeueReusableCell(ProductCell.Key, indexPath);

//public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
//{
//  var _cell = (ProductCell)cell;
//  var item = items[indexPath.Row];
//  // Attach event
//  _cell.ProductCell_FavoriteClicked -= ShowAllSource_FavoriteClicked;
//  _cell.ProductCell_FavoriteClicked += ShowAllSource_FavoriteClicked;

//  _cell.ConfigureWith(item);

//  if (categoryName == Const.FavoritesCategory)
//      _cell.MarkFavorites(item);
//}

//public override void CellDisplayingEnded(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
//{
//  var _cell = (ProductCell)cell;
//  // Detach event
//  _cell.ProductCell_FavoriteClicked -= ShowAllSource_FavoriteClicked;
//}

//public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
//{
//  tableView.DeselectRow(indexPath, true);
//  var item = items[indexPath.Row];
//  ShowAllSource_ItemSelected?.Invoke(this, item);
//}
