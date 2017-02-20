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

		private AppDelegate _currentApplication
		{
			get { return (AppDelegate)UIApplication.SharedApplication.Delegate; }
		}
		private string categoryName; 
		private List<Product> categoryProducts;
		public event EventHandler<string> SeeAllClickedEvent;
		public HomeCellDelegate _delegate;

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

			_delegate = new HomeCellDelegate(category.Products);

			InternalCollectionView.DataSource = new HomeCellDataSource(category.Products);
			InternalCollectionView.Delegate = _delegate;
			InternalCollectionView.ReloadData();

			ShowAllButton.TouchUpInside += OnSeeAllClicked;
		}

		public override void PrepareForReuse()
		{
			ShowAllButton.TouchUpInside -= OnSeeAllClicked;
			base.PrepareForReuse();
		}

		// -------------------- Event handlers --------------------
		public void OnSeeAllClicked(object sender, EventArgs e)
		{
			// Execute event and throw category name to HomeViewController
			SeeAllClickedEvent(this, categoryName);
		}
		// --------------------------------------------------------

		#region IUIViewControllerPreviewingDelegate implementation
		public override void TraitCollectionDidChange(UITraitCollection previousTraitCollection)
		{
			// Must call base method
			base.TraitCollectionDidChange(previousTraitCollection);

			if (TraitCollection.ForceTouchCapability == UIForceTouchCapability.Available)
			{
				var visibleController = _currentApplication.VisibleViewController;
				visibleController.RegisterForPreviewingWithDelegate(this, InternalCollectionView);
			}
			else {
				// TODO Need move fom here !!!
				UIAlertController alertController = UIAlertController.Create("3D Touch Not Available", "Unsupported device.", UIAlertControllerStyle.Alert);
				_currentApplication.VisibleViewController.PresentViewController(alertController, true, null);
			}
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
			var previewController = (PreViewController)UIStoryboard.FromName(StoryboardConstants.StoryboardMain, null).InstantiateViewController(StoryboardConstants.PreViewController);
			if (previewController == null)
				return null;
			
			var previewItem = categoryProducts[indexPath.Row];
			previewController.SetItem(previewItem);
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
	public class HomeCellDelegate : UICollectionViewDelegate
	{
		private List<Product> products = new List<Product>();
		public event EventHandler<Product> ItemSelectedEvent;

		public HomeCellDelegate(List<Product> products)
		{
			this.products = products;
		}

		public override void WillDisplayCell(UICollectionView collectionView, UICollectionViewCell cell, NSIndexPath indexPath)
		{
			var _cell = cell as HomeInternalCell;
			var item = products[indexPath.Row];
			_cell.ConfigureWith(item);
		}

		public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
		{
			ItemSelectedEvent(this, products[indexPath.Row]);
		}
	}
	#endregion
}
