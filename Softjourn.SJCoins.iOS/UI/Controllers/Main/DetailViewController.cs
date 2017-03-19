using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.iOS.General.Constants;
using Softjourn.SJCoins.iOS.UI.Controllers.Main;
using Softjourn.SJCoins.iOS.UI.DataSources;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers
{
	[Register("DetailViewController")]
	public partial class DetailViewController : BaseViewController<DetailPresenter>, IDetailView
	{
		#region Properties
		private int productId { get; set; }

		private Product currentProduct;
		private UIPageViewController pageViewController;
		#endregion

		#region Constructor
		public DetailViewController(IntPtr handle) : base(handle)
		{
		}

		public void SetInitialParameter(object productId)
		{
			if (productId is int)
			{
				this.productId = (int)productId;
			}
		}
		#endregion

		#region Controller Life cycle
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			currentProduct = Presenter.GetProduct(productId);
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
			// Attach 
			FavoriteButton.TouchUpInside += FavoriteButtonClickHandler;
			BuyButton.TouchUpInside += BuyButtonClickHandler;
		}

		public override void DetachEvents()
		{
			// Detach
			FavoriteButton.TouchUpInside -= FavoriteButtonClickHandler;
			BuyButton.TouchUpInside -= BuyButtonClickHandler;
			base.DetachEvents();
		}
		#endregion

		#region IDetailView implementation
		public void FavoriteChanged(bool isFavorite)
		{
			// change button image
			ConfigureFavoriteImage(isFavorite);
			// TODO let know another controllers in this product is favorite
		}

		public void LastUnavailableFavoriteRemoved()
		{
			
		}
		#endregion

		#region Private methods
		private void ConfigurePageWith(Product product)
		{
			NameLabel.Text = product.Name;
			PriceLabel.Text = product.Price.ToString();
			ConfigureFavoriteImage(product.IsProductFavorite);
			ConfigurePageViewController();
		}

		private void ConfigureFavoriteImage(bool isFavorite)
		{
			if (isFavorite)
				FavoriteButton.SetImage(UIImage.FromBundle(ImageConstants.FavoriteChecked), forState: UIControlState.Normal);
			else
				FavoriteButton.SetImage(UIImage.FromBundle(ImageConstants.FavoriteUnchecked), forState: UIControlState.Normal);
		}

		private List<UIViewController> CreatePages()
		{
			var pages = new List<UIViewController>();
			foreach (var item in currentProduct.ImagesFullUrls)
			{
				var controller = Instantiate(StoryboardConstants.StoryboardMain, StoryboardConstants.ImageContentViewController) as ImageContentViewController;
				controller.SetImage(item);
				pages.Add(controller);
			}
			return pages;
		}

		private UIViewController Instantiate(string storyboard, string viewcontroller) => UIStoryboard.FromName(storyboard, null).InstantiateViewController(viewcontroller);

		private void ConfigurePageViewController()
		{
			// Create UIPageViewController and configure it
			pageViewController = Instantiate(StoryboardConstants.StoryboardLogin, StoryboardConstants.PageViewController) as UIPageViewController;
			var pages = CreatePages();
			pageViewController.DataSource = new PageViewDataSource(pages);
			//pageViewController.Delegate = new PageViewControllerDelegate(this);
			var defaultViewController = new UIViewController[] { pages.ElementAt(0) };
			pageViewController.SetViewControllers(defaultViewController, UIPageViewControllerNavigationDirection.Forward, false, null);
			pageViewController.View.Frame = LogoView.Frame; //new CGRect(0, 0, this.View.Frame.Width, this.View.Frame.Size.Height);
			LogoView.AddSubview(this.pageViewController.View);
		}

		//private void ConfigurePageControl()
		//{
		//	View.BringSubviewToFront(PageControl);
		//	PageControl.Pages = _pages.Count;
		//	PageControl.CurrentPage = 0;
		//}

		// -------------------- Event handlers --------------------
		private void FavoriteButtonClickHandler(object sender, EventArgs e)
		{
			// Handle clicking on the Favorite button
			Presenter.OnFavoriteClick(currentProduct);
		}

		private void BuyButtonClickHandler(object sender, EventArgs e)
		{
			// Handle clicking on the Buy button
			Presenter.OnBuyProductClick(currentProduct);
		}
		// -------------------------------------------------------- 
		#endregion
	}
}
