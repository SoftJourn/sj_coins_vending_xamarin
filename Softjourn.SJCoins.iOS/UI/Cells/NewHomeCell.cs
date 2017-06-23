using System;
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
	public partial class NewHomeCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString("NewHomeCell");
		public static readonly UINib Nib;

		public event EventHandler<Product> NewHomeCell_BuyActionExecuted;
		public event EventHandler<Product> NewHomeCell_FavoriteActionExecuted;
		public event EventHandler<string> NewHomeCell_SeeAllClicked;
		public event EventHandler<Product> NewHomeCell_ItemSelected;

		private string categoryName;
		private NewInternalHomeViewSource source;

		static NewHomeCell()
		{
			Nib = UINib.FromName("NewHomeCell", NSBundle.MainBundle);
		}

		protected NewHomeCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public void ConfigureWith(Categories category)
		{
			// Configure CollectionView
			source = new NewInternalHomeViewSource();
			CollectionView.Source = source;
			//CollectionView.SetContentOffset(CollectionView.ContentOffset, false);

			// Set products which need to be displayed 
			categoryName = category.Name;
			NameLabel.Text = categoryName;
			source.Category = category;

			CollectionView.ReloadData();

			//Attach
			ShowAllButton.TouchUpInside -= NewHomeCell_OnSeeAllClickedHandler;
			ShowAllButton.TouchUpInside += NewHomeCell_OnSeeAllClickedHandler;

			source.NewInternalHomeViewSource_ItemSelected -= NewHomeCell_OnItemClickedHandler;
			source.NewInternalHomeViewSource_ItemSelected += NewHomeCell_OnItemClickedHandler;

			source.NewInternalHomeViewSource_BuyActionExecuted -= NewHomeCell_BuyActionExecuted;
			source.NewInternalHomeViewSource_BuyActionExecuted += NewHomeCell_BuyActionExecuted;

			source.NewInternalHomeViewSource_FavoriteActionExecuted -= NewHomeCell_FavoriteActionExecuted;
			source.NewInternalHomeViewSource_FavoriteActionExecuted += NewHomeCell_FavoriteActionExecuted;
		}

		public override void PrepareForReuse()
		{
			NameLabel.Text = "";
			CollectionView.Source = null;
			categoryName = null;

			ShowAllButton.TouchUpInside -= NewHomeCell_OnSeeAllClickedHandler;

			if (source != null)
			{
				source.NewInternalHomeViewSource_ItemSelected -= NewHomeCell_ItemSelected;
				source.NewInternalHomeViewSource_BuyActionExecuted -= NewHomeCell_BuyActionExecuted;
				source.NewInternalHomeViewSource_FavoriteActionExecuted -= NewHomeCell_FavoriteActionExecuted;
				source = null;
			}
			base.PrepareForReuse();
		}

		#region Event handlers
		public void NewHomeCell_OnSeeAllClickedHandler(object sender, EventArgs e)
		{
			// Execute event and throw category name to HomeViewController
			// User click on button
			NewHomeCell_SeeAllClicked?.Invoke(this, categoryName);
		}

		public void NewHomeCell_OnItemClickedHandler(object sender, Product product)
		{
			// Execute event and throw product to HomeViewController
			// User tap on item
			NewHomeCell_ItemSelected?.Invoke(this, product);
		}
		#endregion
	}
}
