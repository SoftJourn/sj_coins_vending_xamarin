using System;
using CoreGraphics;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.iOS.General.Constants;
using UIKit;
using Softjourn.SJCoins.iOS.UI.Services;

namespace Softjourn.SJCoins.iOS.UI.Controllers
{
	[Register("DetailViewController")]
    public partial class DetailViewController : BaseViewController<DetailPresenter>, IDetailView
	{
        public const float pageHeightCoefficient = 1.85f;

		#region Properties
		private int ProductId { get; set; }

		private Product currentProduct;
		private int currentImage;

		private DetailTableViewSource tableSource;
        private DetailCollectionViewSource collectionSource;
        private DetailCollectionViewFlowLayoutDelegate collectionDelegate;
		#endregion

		#region Constructor
		public DetailViewController(IntPtr handle) : base(handle)
		{
		}

		public void SetInitialParameter(object productId)
		{
			if (productId is int)
				this.ProductId = (int)productId;
		}
		#endregion

		#region Controller Life cycle
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			currentProduct = Presenter.GetProduct(ProductId);
            ConfigurePage();
			ConfigurePageControl();
			ConfigureTableView();
            ConfigureImageCollection();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			ConfigurePageWith(currentProduct);
        }
		#endregion

		#region BaseViewController
		public override void AttachEvents()
		{
			base.AttachEvents();
			FavoriteButton.Clicked += FavoriteButtonClicked;
			BuyButton.TouchUpInside += BuyButtonClicked;
			collectionDelegate.SelectedItem += ShowImageCarousel;
            collectionDelegate.VisibleItem += ImageIndexChanged;
            //tableSource.DidScroll += TableViewScrolled;
		}

		public override void DetachEvents()
		{
			FavoriteButton.Clicked -= FavoriteButtonClicked;
			BuyButton.TouchUpInside -= BuyButtonClicked;
            collectionDelegate.SelectedItem -= ShowImageCarousel;
			collectionDelegate.VisibleItem -= ImageIndexChanged;
            //tableSource.DidScroll -= TableViewScrolled;
			base.DetachEvents();
		}
		#endregion

		#region IDetailView implementation
		public void FavoriteChanged(Product product)
		{
			LoaderService.Hide();
			// change button image
			ConfigureFavoriteImage(product.IsProductFavorite);
		}

		public void LastUnavailableFavoriteRemoved(Product product)
		{
            LoaderService.Hide();
			// change button image
			ConfigureFavoriteImage(product.IsProductFavorite);
		}
		#endregion

		#region Private methods
        private void ConfigurePage() 
        {
            StyleNavigationBar();
        }

		private void ConfigurePageWith(Product product)
		{
			Title = product.Name;
			PriceLabel.Text = product.Price.ToString();
			ConfigureFavoriteImage(product.IsProductFavorite);
		}

		private void ConfigureTableView()
		{
			tableSource = new DetailTableViewSource(currentProduct);
			TableView.Source = tableSource;
			TableView.EstimatedRowHeight = 50;
			TableView.RowHeight = UITableView.AutomaticDimension;
		}

        private void ConfigureImageCollection()
        {
            collectionSource = new DetailCollectionViewSource
            {
                Images = currentProduct.ImagesFullUrls
            };

            collectionDelegate = new DetailCollectionViewFlowLayoutDelegate
            {
                Images = currentProduct.ImagesFullUrls
            };

            ImageCollectionView.DataSource = collectionSource;
			ImageCollectionView.Delegate = collectionDelegate;
        }

		private void ConfigurePageControl()
		{
            if (currentProduct.ImagesFullUrls.Count > 1)
			{
				PageControl.Pages = currentProduct.ImagesFullUrls.Count;
				PageControl.CurrentPage = 0;
				HeaderView.BringSubviewToFront(PageControl);
				PageControl.Hidden = false;
			}
			else
			{
				PageControl.Hidden = true;
			}
		}

		private void ConfigureFavoriteImage(bool isFavorite)
		{
			if (isFavorite)
				FavoriteButton.Image = UIImage.FromBundle(ImageConstants.HeartFilled);
			else
				FavoriteButton.Image = UIImage.FromBundle(ImageConstants.Heart);
		}
		#endregion

		#region Event handlers
		private void FavoriteButtonClicked(object sender, EventArgs e)
		{
            // Handle clicking on the Favorite button
            LoaderService.Show("Loading...");
			Presenter.OnFavoriteClick(currentProduct);
		}

		private void BuyButtonClicked(object sender, EventArgs e)
		{
			// Handle clicking on the Buy button
			Presenter.OnBuyProductClick(currentProduct);
		}

		private void ImageIndexChanged(object sender, int currentIndex)
		{
			// Change dot on Page Controlсщшт
			currentImage = currentIndex;
			PageControl.CurrentPage = currentIndex;
		}

        private void ShowImageCarousel(object sender, string image)
		{
            // Show carousel on image
            var controller = UIStoryboard.FromName(StoryboardConstants.StoryboardMain, null).InstantiateViewController(StoryboardConstants.ImageCarouselViewController) as UINavigationController;
            var firstController = controller.ViewControllers[0] as ImageCarouselViewController;
            firstController.CurrentProduct = currentProduct;
            firstController.CurrentImage = currentImage;
            firstController.VisibleItem += ImageChanged;

			PresentViewController(controller, true, null);
		}

		private void ImageChanged(object sender, int currentIndex)
		{
            ImageCollectionView.SetContentOffset(new CGPoint(currentIndex * SizeHelper.mainBounds.Width, 0.0), false);

            // Change dot on Page Control
            currentImage = currentIndex;
			PageControl.CurrentPage = currentIndex;
		}

        //private void TableViewScrolled(object sender, EventArgs e)
        //{
        //    var offsetY = TableView.ContentOffset.Y;
        //    if (offsetY < 0) {
                
        //    }
        //}
		#endregion
	}
}
