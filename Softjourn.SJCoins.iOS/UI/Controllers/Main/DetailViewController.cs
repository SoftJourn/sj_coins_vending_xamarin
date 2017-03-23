using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using CoreAnimation;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.iOS.General.Constants;
using Softjourn.SJCoins.iOS.UI.Controllers.Main;
using Softjourn.SJCoins.iOS.UI.DataSources;
using UIKit;
using Softjourn.SJCoins.iOS.UI.Services;

namespace Softjourn.SJCoins.iOS.UI.Controllers
{
	[Register("DetailViewController")]
	public partial class DetailViewController : BaseViewController<DetailPresenter>, IDetailView
	{
		#region Properties
		private Lazy<AnimationService> lazyAnimationService = new Lazy<AnimationService>(() => { return new AnimationService(); });
		private AnimationService animationService { get { return lazyAnimationService.Value; } }
		private int productId { get; set; }

		private Product currentProduct;
		private List<UIViewController> pages;
		private UIPageViewController pageViewController;
		private PageViewDataSource pageDataSource;
		private DetailViewSource tableSource;
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
			ConfigurePageViewController();
			ConfigurePageControl();
			ConfigureTableView();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			ConfigurePageWith(currentProduct);
		}

		public override void ViewDidDisappear(bool animated)
		{
			animationService.Dispose();
			base.ViewDidDisappear(animated);
		}
		#endregion

		#region BaseViewController 
		public override void AttachEvents()
		{
			base.AttachEvents();
			FavoriteButton.TouchUpInside += FavoriteButtonClicked;
			BuyButton.TouchUpInside += BuyButtonClicked;
			pageDataSource.CurrentIndexChanged += ImageIndexChanged;
		}

		public override void DetachEvents()
		{
			FavoriteButton.TouchUpInside -= FavoriteButtonClicked;
			BuyButton.TouchUpInside -= BuyButtonClicked;
			pageDataSource.CurrentIndexChanged -= ImageIndexChanged;
			base.DetachEvents();
		}
		#endregion

		#region IDetailView implementation
		public void FavoriteChanged(Product product)
		{
			// End button rotation
			animationService.CompleteRotation(FavoriteButton);
			animationService.ScaleEffect(FavoriteButton);
			// change button image
			ConfigureFavoriteImage(product.IsProductFavorite);
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
		}

		private void ConfigureTableView()
		{
			tableSource = new DetailViewSource(currentProduct);
			TableView.Source = tableSource;
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
			pages = new List<UIViewController>();
			if (currentProduct.ImageUrls != null)
			{
				foreach (var item in currentProduct.ImagesFullUrls)
				{
					pages.Add(InstantiateImageContentController(item));
				}
			}
			else
			{
				pages.Add(InstantiateImageContentController(currentProduct.ImageFullUrl));
			}
			return pages;
		}

		private UIViewController Instantiate(string storyboard, string viewcontroller) => UIStoryboard.FromName(storyboard, null).InstantiateViewController(viewcontroller);

		private ImageContentViewController InstantiateImageContentController(string imageFullUrl)
		{
			var controller = Instantiate(StoryboardConstants.StoryboardMain, StoryboardConstants.ImageContentViewController) as ImageContentViewController;
			controller.SetImage(imageFullUrl);
			return controller;
		}

		private void ConfigurePageViewController()
		{
			// Create UIPageViewController and configure it
			pageViewController = Instantiate(StoryboardConstants.StoryboardLogin, StoryboardConstants.PageViewController) as UIPageViewController;
			pages = CreatePages();
			pageDataSource = new PageViewDataSource(pages);
			pageViewController.DataSource = pageDataSource;
			var defaultViewController = new UIViewController[] { pages.ElementAt(0) };
			pageViewController.SetViewControllers(defaultViewController, UIPageViewControllerNavigationDirection.Forward, false, null);
			pageViewController.View.Frame = new CGRect(25, 25, LogoView.Frame.Width - 50, LogoView.Frame.Size.Height - 50);
			LogoView.AddSubview(this.pageViewController.View);
		}

		private void ConfigurePageControl()
		{
			if (pages.Count > 1)
			{
				PageControl.Pages = pages.Count;
				PageControl.CurrentPage = 0;
				LogoView.BringSubviewToFront(PageControl);
				PageControl.Hidden = false;
				pageViewController.View.UserInteractionEnabled = true;
			}
			else
			{
				PageControl.Hidden = true;
				pageViewController.View.UserInteractionEnabled = false;
			}
		}

		// -------------------- Event handlers --------------------
		private void FavoriteButtonClicked(object sender, EventArgs e)
		{
			// Handle clicking on the Favorite button
			animationService.StartRotation(FavoriteButton);
			Presenter.OnFavoriteClick(currentProduct);
		}

		private void BuyButtonClicked(object sender, EventArgs e)
		{
			// Handle clicking on the Buy button
			Presenter.OnBuyProductClick(currentProduct);
		}

		private void ImageIndexChanged(object sender, int currentIndex)
		{
			// Change dot on Page Control
			PageControl.CurrentPage = currentIndex;
		}
		// -------------------------------------------------------- 
		#endregion
	}
}
