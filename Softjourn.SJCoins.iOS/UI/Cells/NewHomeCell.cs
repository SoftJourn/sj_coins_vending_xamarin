using System;
using UIKit;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.iOS.UI.Sources;
using Softjourn.SJCoins.iOS.UI.Delegates;

namespace Softjourn.SJCoins.iOS
{
    public partial class NewHomeCell : UITableViewCell, IDisposable
	{
		#region Properties
		public static readonly NSString Key = new NSString("NewHomeCell");
		public static readonly UINib Nib;

		public event EventHandler<Product> NewHomeCell_BuyActionExecuted;
		public event EventHandler<Product> NewHomeCell_FavoriteActionExecuted;
		public event EventHandler<string> NewHomeCell_SeeAllClicked;
		public event EventHandler<Product> NewHomeCell_ItemSelected;

		private string categoryName;
        private InternalHomeViewSource collectionSource;
        private CollectionViewFlowLayoutDelegate collectionDelegate;

		static NewHomeCell()
		{ 
			Nib = UINib.FromName("NewHomeCell", NSBundle.MainBundle);
		}
        #endregion

		protected NewHomeCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

			this.collectionSource = new InternalHomeViewSource();
			this.collectionDelegate = new CollectionViewFlowLayoutDelegate();

			// Configure CollectionView
			CollectionView.DataSource = collectionSource;
			CollectionView.Delegate = collectionDelegate;

			//Attach
			ShowAllButton.TouchUpInside -= NewHomeCell_OnSeeAllClickedHandler;
			ShowAllButton.TouchUpInside += NewHomeCell_OnSeeAllClickedHandler;

			collectionDelegate.SelectedItem -= NewHomeCell_ItemSelectedHandler;
			collectionDelegate.SelectedItem += NewHomeCell_ItemSelectedHandler;
        }

		public void ConfigureWith(Categories category)
		{
			// Set category name
			categoryName = category.Name;
			NameLabel.Text = categoryName;

			// Set products which need to be displayed
			collectionSource.Products = category.Products;
            collectionDelegate.Products = category.Products;

            CollectionView.ReloadData();
		}

		public override void PrepareForReuse()
		{
			NameLabel.Text = "";
			categoryName = null;

			//ShowAllButton.TouchUpInside -= NewHomeCell_OnSeeAllClickedHandler;
			base.PrepareForReuse();		
		}

		#region Event handlers
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
		#endregion

		protected override void Dispose(bool disposing)
		{
			System.Diagnostics.Debug.WriteLine(String.Format("{0} object disposed", this.GetType()));
			base.Dispose(disposing);
		}
	}
}
