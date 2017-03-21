using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.iOS.General.Constants;
using Softjourn.SJCoins.iOS.UI.Controllers;
using UIKit;

namespace Softjourn.SJCoins.iOS
{
	public partial class HomeCell : UICollectionViewCell, IUIViewControllerPreviewingDelegate
	{
		public static readonly NSString Key = new NSString("HomeCell");
		public static readonly UINib Nib;

		public event EventHandler<Product> HomeCell_BuyActionExecuted;
		public event EventHandler<Product> HomeCell_FavoriteActionExecuted;
		public event EventHandler<string> HomeCell_SeeAllClicked;
		public event EventHandler<Product> HomeCell_ItemSelected;

		private AppDelegate _currentApplication
		{
			get { return (AppDelegate)UIApplication.SharedApplication.Delegate; }
		}
		private string categoryName; 
		private List<Product> categoryProducts;
		private HomeCellDelegate _delegate;
		private HomeCellDataSource _dataSource;
		private PreViewController previewController;

		static HomeCell()
		{
			Nib = UINib.FromName("HomeCell", NSBundle.MainBundle);
		}

		protected HomeCell(IntPtr handle) : base(handle)
		{
		}

		public void ConfigureWith(Categories category)
		{
			// Save and set category name
			categoryName = category.Name;
			CategoryNameLabel.Text = category.Name;
			categoryProducts = category.Products;

			_delegate = new HomeCellDelegate(category.Products, categoryName);
			_dataSource = new HomeCellDataSource(category.Products);

			InternalCollectionView.DataSource = _dataSource;
			InternalCollectionView.Delegate = _delegate;
			InternalCollectionView.ReloadData();

			//Attach
			ShowAllButton.TouchUpInside -= HomeCell_OnSeeAllClickedHandler;
			ShowAllButton.TouchUpInside += HomeCell_OnSeeAllClickedHandler;

			_delegate.HomeCellDelegate_ItemSelected -= HomeCell_ItemSelectedHandler;
			_delegate.HomeCellDelegate_ItemSelected += HomeCell_ItemSelectedHandler;
		}

		public override void PrepareForReuse()
		{
			// Reset category name and products
			categoryName = null;
			CategoryNameLabel.Text = "";
			categoryProducts = null;

			// Dettach
			if (previewController != null)
			{
				previewController.PreViewController_BuyActionExecuted -= HomeCell_OnFavoriteActionClickedHandler;
				previewController.PreViewController_FavoriteActionExecuted -= HomeCell_OnFavoriteActionClickedHandler;
			}
			ShowAllButton.TouchUpInside -= HomeCell_OnSeeAllClickedHandler;
			//_delegate.HomeCellDelegate_ItemSelected -= HomeCell_ItemSelectedHandler;

			_dataSource = null;
			_delegate = null;

			Layer.ShouldRasterize = true;
			Layer.RasterizationScale = UIScreen.MainScreen.Scale;

			base.PrepareForReuse();
		}

		// -------------------- Event handlers --------------------
		public void HomeCell_OnSeeAllClickedHandler(object sender, EventArgs e)
		{
			// Execute event and throw category name to HomeViewController
			// User click on button
			HomeCell_SeeAllClicked?.Invoke(this, categoryName);
		}

		private void HomeCell_ItemSelectedHandler(object sender, Product product)
		{
			// ItemSelected from delegate object
			HomeCell_ItemSelected?.Invoke(this, product);
		}

		public void HomeCell_OnBuyActionClickedHandler(object sender, Product product)
		{
			// Execute event via 3D Touch functionality and throw product to HomeViewController
			HomeCell_BuyActionExecuted?.Invoke(this, product);
		}

		public void HomeCell_OnFavoriteActionClickedHandler(object sender, Product product)
		{
			// Execute event via 3D Touch functionality and throw product to HomeViewController
			HomeCell_FavoriteActionExecuted?.Invoke(this, product);
		}
		// --------------------------------------------------------

		#region IUIViewControllerPreviewingDelegate implementation
		public override void TraitCollectionDidChange(UITraitCollection previousTraitCollection)
		{
			// Must call base method
			base.TraitCollectionDidChange(previousTraitCollection);

			if (TraitCollection.ForceTouchCapability == UIForceTouchCapability.Available)
			{
				_currentApplication.VisibleViewController.RegisterForPreviewingWithDelegate(this, InternalCollectionView);
			}
			//else {
			//	// TODO Need move fom here !!!
			//	UIAlertController alertController = UIAlertController.Create("3D Touch Not Available", "Unsupported device.", UIAlertControllerStyle.Alert);
			//	_currentApplication.VisibleViewController.PresentViewController(alertController, true, null);
			//}
		}

		public UIViewController GetViewControllerForPreview(IUIViewControllerPreviewing previewingContext, CGPoint location)
		{
			// Obtain the index path and the cell that was pressed.
			var indexPath = InternalCollectionView.IndexPathForItemAtPoint(location);

			if (indexPath == null)
				return null;

			var cell = InternalCollectionView.CellForItem(indexPath);

			if (cell == null)
				return null;

			// Create a preview controller and set its properties.
			previewController = (PreViewController)UIStoryboard.FromName(StoryboardConstants.StoryboardMain, null).InstantiateViewController(StoryboardConstants.PreViewController);
			if (previewController == null)
				return null;

			var previewItem = categoryProducts[indexPath.Row];
			previewController.SetItem(previewItem);
			// Attach
			previewController.PreViewController_BuyActionExecuted += HomeCell_OnBuyActionClickedHandler;
			previewController.PreViewController_FavoriteActionExecuted += HomeCell_OnFavoriteActionClickedHandler;

			previewController.PreferredContentSize = new CGSize(0, 420);
			previewingContext.SourceRect = cell.Frame;
			return previewController;
		}

		public void CommitViewController(IUIViewControllerPreviewing previewingContext, UIViewController viewControllerToCommit)
		{
			_currentApplication.VisibleViewController.ShowViewController(viewControllerToCommit, this);
		}
		#endregion
	}

	#region UICollectionViewSource implementation
	public class HomeCellDataSource : UICollectionViewDataSource
	{
		private List<Product> products = new List<Product>(); 

		public HomeCellDataSource(List<Product> products)
		{
			this.products = products;
		}

		public override nint GetItemsCount(UICollectionView collectionView, nint section) => products.Count;

		public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath) => (UICollectionViewCell)collectionView.DequeueReusableCell(HomeInternalCell.Key, indexPath);
	}
	#endregion

	#region UICollectionViewDelegate implementation
	public class HomeCellDelegate : UICollectionViewDelegate, IDisposable
	{
		public event EventHandler<Product> HomeCellDelegate_ItemSelected;

		private List<Product> products = new List<Product>();
		private string categoryName;

		public HomeCellDelegate(List<Product> products, string categoryName)
		{
			this.products = products;
			this.categoryName = categoryName;
		}

		public override void WillDisplayCell(UICollectionView collectionView, UICollectionViewCell cell, NSIndexPath indexPath)
		{
			var _cell = cell as HomeInternalCell;
			var item = products[indexPath.Row];
			_cell.ConfigureWith(item);

			if (categoryName == Const.FavoritesCategory)
				_cell.MarkFavorites(item);
		}

		public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
		{
			HomeCellDelegate_ItemSelected?.Invoke(this, products[indexPath.Row]);
		}

		protected override void Dispose(bool disposing)
		{
			System.Diagnostics.Debug.WriteLine(String.Format("{0} object disposed", this.GetType()));
			base.Dispose(disposing);
		}
	}
	#endregion
}
