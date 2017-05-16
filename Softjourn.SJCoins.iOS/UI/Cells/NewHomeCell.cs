using System;
using UIKit;
using Foundation;
using CoreGraphics;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.iOS.UI.Sources;
using System.Collections.Generic;
using Softjourn.SJCoins.iOS.UI.Controllers;

namespace Softjourn.SJCoins.iOS
{
	public partial class NewHomeCell : UITableViewCell
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
		private PreViewController previewController;

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
			source.Products = category.Products;
			source.CategoryName = categoryName;
			// Configure CollectionView
			CollectionView.Source = source;
			CollectionView.SetContentOffset(CollectionView.ContentOffset, false);
			CollectionView.ReloadData();

			//Attach
			ShowAllButton.TouchUpInside -= NewHomeCell_OnSeeAllClickedHandler;
			ShowAllButton.TouchUpInside += NewHomeCell_OnSeeAllClickedHandler;

			source.NewInternalHomeViewSource_ItemSelected -= NewHomeCell_ItemSelectedHandler;
			source.NewInternalHomeViewSource_ItemSelected += NewHomeCell_ItemSelectedHandler;
		}

		public override void PrepareForReuse()
		{
			NameLabel.Text = "";
			CollectionView.Source = null;
			categoryName = null;
			categoryProducts = null;

			// Dettach
			if (previewController != null)
			{
				previewController.PreViewController_BuyActionExecuted -= NewHomeCell_OnFavoriteActionClickedHandler;
				previewController.PreViewController_FavoriteActionExecuted -= NewHomeCell_OnFavoriteActionClickedHandler;
			}

			ShowAllButton.TouchUpInside -= NewHomeCell_OnSeeAllClickedHandler;

			if (source != null)
			{
				source.NewInternalHomeViewSource_ItemSelected -= NewHomeCell_ItemSelectedHandler;
				source = null;
			}
			base.PrepareForReuse();		
		}

		// -------------------- Event handlers --------------------
		public void NewHomeCell_OnSeeAllClickedHandler(object sender, EventArgs e)
		{
			// Execute event and throw category name to HomeViewController
			// User click on button
			NewHomeCell_SeeAllClicked?.Invoke(this, categoryName);
		}

		private void NewHomeCell_ItemSelectedHandler(object sender, Product product)
		{
			// ItemSelected from delegate object
			NewHomeCell_ItemSelected?.Invoke(this, product);
		}

		public void NewHomeCell_OnBuyActionClickedHandler(object sender, Product product)
		{
			// Execute event via 3D Touch functionality and throw product to HomeViewController
			NewHomeCell_BuyActionExecuted?.Invoke(this, product);
		}

		public void NewHomeCell_OnFavoriteActionClickedHandler(object sender, Product product)
		{
			// Execute event via 3D Touch functionality and throw product to HomeViewController
			NewHomeCell_FavoriteActionExecuted?.Invoke(this, product);
		}
		// --------------------------------------------------------
	}
}
