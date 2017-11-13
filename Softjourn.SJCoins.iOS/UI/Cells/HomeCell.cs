using System;
using UIKit;
using Foundation;
using CoreGraphics;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.iOS.UI.Sources;
using Softjourn.SJCoins.iOS.UI.Delegates;
using Softjourn.SJCoins.iOS.General.Constants;

namespace Softjourn.SJCoins.iOS
{
    public partial class HomeCell : UITableViewCell, IDisposable
	{
		#region Properties
		public static readonly NSString Key = new NSString("HomeCell");
		public static readonly UINib Nib;

		public event EventHandler<Product> HomeCell_BuyActionExecuted;
		public event EventHandler<Product> HomeCell_FavoriteActionExecuted;
		public event EventHandler<string> HomeCell_SeeAllClicked;
		public event EventHandler<Product> HomeCell_ItemSelected;

		private string categoryName;
        private CGPoint contentOffset;
        private InternalHomeViewSource collectionSource;
        private CollectionViewFlowLayoutDelegate collectionDelegate;

		static HomeCell()
		{ 
			Nib = UINib.FromName("HomeCell", NSBundle.MainBundle);
		}
        #endregion

		protected HomeCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public void ConfigureWith(Categories category)
		{
			// Set category name
			categoryName = category.Name;
			NameLabel.Text = categoryName;

			// Configure CollectionView
			this.collectionSource = new InternalHomeViewSource();
			this.collectionDelegate = new CollectionViewFlowLayoutDelegate();

			CollectionView.DataSource = collectionSource;
			CollectionView.Delegate = collectionDelegate;
			
            collectionSource.Products = category.Products;
			collectionDelegate.Products = category.Products;

            CollectionView.ReloadData();

            AttachEvents();

            ShowAllButton.Hidden |= categoryName == Const.MatchesCategory;
		}

		public override void PrepareForReuse()
		{
			CollectionView.SetContentOffset(new CGPoint(0, 0), false);

			NameLabel.Text = "";
			categoryName = null;

            DetachEvents();
            this.collectionSource = null;
            this.collectionDelegate = null;

            ShowAllButton.Hidden = false;

			base.PrepareForReuse();		
		}

		#region Private methods
		private void AttachEvents()
        {
            ShowAllButton.TouchUpInside -= HomeCell_OnSeeAllHandler;
			ShowAllButton.TouchUpInside += HomeCell_OnSeeAllHandler;

            collectionDelegate.SelectedItem -= HomeCell_ItemHandler;
			collectionDelegate.SelectedItem += HomeCell_ItemHandler;

            collectionDelegate.BuyActionExecuted -= HomeCell_OnBuyActionHandler;
			collectionDelegate.BuyActionExecuted += HomeCell_OnBuyActionHandler;

            collectionDelegate.FavoriteActionExecuted -= HomeCell_OnFavoriteActionHandler;
			collectionDelegate.FavoriteActionExecuted += HomeCell_OnFavoriteActionHandler;
		}

		private void DetachEvents()
		{
			ShowAllButton.TouchUpInside -= HomeCell_OnSeeAllHandler;
			collectionDelegate.SelectedItem -= HomeCell_ItemHandler;
			collectionDelegate.BuyActionExecuted -= HomeCell_OnBuyActionHandler;
			collectionDelegate.FavoriteActionExecuted -= HomeCell_OnFavoriteActionHandler;
		}
		#endregion

		#region Event handlers
        private void HomeCell_OnSeeAllHandler(object sender, EventArgs e)
		{
			// Execute event and throw category name to HomeViewController
			// User click on button
			HomeCell_SeeAllClicked?.Invoke(this, categoryName);
		}

		private void HomeCell_ItemHandler(object sender, Product product)
		{
			// ItemSelected from delegate object
			HomeCell_ItemSelected?.Invoke(this, product);
		}

        private void HomeCell_OnBuyActionHandler(object sender, Product product)
		{
			// Execute event via 3D Touch functionality and throw product to HomeViewController
			HomeCell_BuyActionExecuted?.Invoke(this, product);
		}

        private void HomeCell_OnFavoriteActionHandler(object sender, Product product)
		{
			// Execute event via 3D Touch functionality and throw product to HomeViewController
			HomeCell_FavoriteActionExecuted?.Invoke(this, product);
		}
		#endregion

		protected override void Dispose(bool disposing)
		{
			System.Diagnostics.Debug.WriteLine(String.Format("{0} disposed", this.GetType()));
			base.Dispose(disposing);
		}
	}
}
