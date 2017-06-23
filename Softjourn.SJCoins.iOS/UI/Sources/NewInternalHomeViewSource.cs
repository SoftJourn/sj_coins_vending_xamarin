using System;
using CoreGraphics;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.iOS.General.Constants;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Sources
{
	public class NewInternalHomeViewSource : UICollectionViewSource, IDisposable
	{
		public Categories Category { get; set; } = new Categories();

		public event EventHandler<Product> NewInternalHomeViewSource_ItemSelected;
		public event EventHandler<Product> NewInternalHomeViewSource_BuyActionExecuted;
		public event EventHandler<Product> NewInternalHomeViewSource_FavoriteActionExecuted;

		public override nint GetItemsCount(UICollectionView collectionView, nint section) => Category.Products.Count;

		public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath) => (HomeInternalCell)collectionView.DequeueReusableCell(HomeInternalCell.Key, indexPath);

		public override void WillDisplayCell(UICollectionView collectionView, UICollectionViewCell cell, NSIndexPath indexPath)
		{
			var _cell = (HomeInternalCell)cell;
			_cell.SetUpUI();
			_cell.ConfigureWith(Category.Products[indexPath.Row]);

			if (Category.Name == Const.FavoritesCategory)
			{
				_cell.MarkFavorites(Category.Products[indexPath.Row]);
			}

			_cell.HomeInternalCell_BuyActionExecuted -= NewInternalHomeViewSource_BuyActionExecuted;
			_cell.HomeInternalCell_BuyActionExecuted += NewInternalHomeViewSource_BuyActionExecuted;

			_cell.HomeInternalCell_FavoriteActionExecuted -= NewInternalHomeViewSource_FavoriteActionExecuted;
			_cell.HomeInternalCell_FavoriteActionExecuted += NewInternalHomeViewSource_FavoriteActionExecuted;
		}

		public override void CellDisplayingEnded(UICollectionView collectionView, UICollectionViewCell cell, NSIndexPath indexPath)
		{
			var _cell = (HomeInternalCell)cell;
			_cell.HomeInternalCell_BuyActionExecuted -= NewInternalHomeViewSource_BuyActionExecuted;
			_cell.HomeInternalCell_FavoriteActionExecuted -= NewInternalHomeViewSource_FavoriteActionExecuted;
		}

		public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
		{
			NewInternalHomeViewSource_ItemSelected?.Invoke(this, Category.Products[indexPath.Row]);
		}

		protected override void Dispose(bool disposing)
		{
			System.Diagnostics.Debug.WriteLine(String.Format("{0} object disposed", this.GetType()));
			base.Dispose(disposing);
		}
	}
}
