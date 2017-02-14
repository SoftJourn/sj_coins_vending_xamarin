﻿using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.iOS.General.Constants;
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
		public event EventHandler<string> SeeAllClickedEvent = delegate { };

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
			categoryProducts = category.Products;

			ConfigureInternalCollectionView(category.Products, _delegate);
			ConfigureSeeAllButton();
		}

		private void ConfigureSeeAllButton()
		{
			// Add click event to button
			ShowAllButton.TouchUpInside += (o,s) =>
			{
				// Execute event and throw category name to HomeViewController
				SeeAllClickedEvent(this, categoryName);
			};
		}

		private void ConfigureInternalCollectionView(List<Product> products, HomeCellDelegate _delegate)
		{
			InternalCollectionView.DataSource = new HomeCellDataSource(products);
			InternalCollectionView.Delegate = _delegate;
			InternalCollectionView.ReloadData();
		}

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
				// Need move fom here !!!
				UIAlertController alertController = UIAlertController.Create("3D Touch Not Available", "Unsupported device.", UIAlertControllerStyle.Alert);
				_currentApplication.VisibleViewController.PresentViewController(alertController, true, null);
			}
		}

		public UIViewController GetViewControllerForPreview(IUIViewControllerPreviewing previewingContext, CGPoint location)
		{
			// Convert location to collection view coordinate system.
			//CGPoint newLocation = _currentApplication.VisibleViewController.View.ConvertPointToView(location, InternalCollectionView);

			// Obtain the index path and the cell that was pressed.
			var indexPath = InternalCollectionView.IndexPathForItemAtPoint(location);

			if (indexPath == null)
				return null;

			var cell = InternalCollectionView.CellForItem(indexPath);

			if (cell == null)
				return null;

			// Create a preview controller and set its properties.
			var previewController = UIStoryboard.FromName(StoryboardConstants.StoryboardMain, null).InstantiateViewController(StoryboardConstants.PreViewController);
			if (previewController == null)
				return null;
			
			var previewItem = categoryProducts[indexPath.Row];
			previewController.PreferredContentSize = new CGSize(0, 320);
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
		private List<Product> products;

		public HomeCellDataSource(List<Product> products)
		{
			this.products = products;
		}

		public override nint GetItemsCount(UICollectionView collectionView, nint section) => products == null ? 0 : products.Count;

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
