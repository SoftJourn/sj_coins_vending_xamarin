﻿using System;
using UIKit;
using Foundation;
using CoreGraphics;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.iOS.UI.Sources;
using System.Collections.Generic;
using Softjourn.SJCoins.iOS.UI.Controllers;
using Softjourn.SJCoins.iOS.General.Constants;

namespace Softjourn.SJCoins.iOS
{
	public partial class NewHomeCell : UITableViewCell//, IUIViewControllerPreviewingDelegate
	{
		public static readonly NSString Key = new NSString("NewHomeCell");
		public static readonly UINib Nib;

		public event EventHandler<Product> NewHomeCell_BuyActionExecuted;
		public event EventHandler<Product> NewHomeCell_FavoriteActionExecuted;
		public event EventHandler<string> NewHomeCell_SeeAllClicked;
		public event EventHandler<Product> NewHomeCell_ItemSelected;

		private string categoryName;
		private List<Product> categoryProducts;
		private NewInternalHomeViewSource source;

		static NewHomeCell()
		{
			Nib = UINib.FromName("NewHomeCell", NSBundle.MainBundle);
		}

		protected NewHomeCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public void ConfigureWith(Categories category, NewInternalHomeViewSource source, int row)
		{

			// Set category name
			categoryName = category.Name;
			NameLabel.Text = categoryName;
			// Set products which need to be displayed 
			this.source = source;
			categoryProducts = category.Products;
			source.Products = categoryProducts;
			source.CategoryName = categoryName;
			// Configure CollectionView
			CollectionView.Source = source;
			CollectionView.SetContentOffset(CollectionView.ContentOffset, false);
			CollectionView.ReloadData();

			//Attach
			ShowAllButton.TouchUpInside -= NewHomeCell_OnSeeAllClickedHandler;
			ShowAllButton.TouchUpInside += NewHomeCell_OnSeeAllClickedHandler;

			source.NewInternalHomeViewSource_ItemSelected -= ItemSelectedHandler;
			source.NewInternalHomeViewSource_ItemSelected += ItemSelectedHandler;

			source.NewInternalHomeViewSource_BuyActionExecuted -= BuyHandler;
			source.NewInternalHomeViewSource_BuyActionExecuted += BuyHandler;

			source.NewInternalHomeViewSource_FavoriteActionExecuted -= FavoriteHandler;
			source.NewInternalHomeViewSource_FavoriteActionExecuted += FavoriteHandler;
		}

		public override void PrepareForReuse()
		{
			NameLabel.Text = "";
			CollectionView.Source = null;
			categoryName = null;
			categoryProducts = null;

			ShowAllButton.TouchUpInside -= NewHomeCell_OnSeeAllClickedHandler;

			if (source != null)
			{
				source.NewInternalHomeViewSource_ItemSelected -= ItemSelectedHandler;
				source.NewInternalHomeViewSource_BuyActionExecuted -= NewHomeCell_BuyActionExecuted;
				source.NewInternalHomeViewSource_FavoriteActionExecuted -= NewHomeCell_FavoriteActionExecuted;
				source = null;
			}
			base.PrepareForReuse();
		}

		#region Events handlers
		public void NewHomeCell_OnSeeAllClickedHandler(object sender, EventArgs e)
		{
			// Execute event and throw category name to HomeViewController
			// User click on button
			NewHomeCell_SeeAllClicked?.Invoke(this, categoryName);
		}

		public void ItemSelectedHandler(object sender, Product product)
		{
			NewHomeCell_ItemSelected?.Invoke(this, product);
		}

		public void FavoriteHandler(object sender, Product product)
		{
			NewHomeCell_FavoriteActionExecuted?.Invoke(this, product);
		}

		public void BuyHandler(object sender, Product product)
		{
			NewHomeCell_BuyActionExecuted?.Invoke(this, product);
		}
		#endregion
	}
}
