using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.iOS.General.Constants;
using Softjourn.SJCoins.iOS.UI.Controllers.Main;
using Softjourn.SJCoins.iOS.UI.DataSources;
using UIKit;
using Softjourn.SJCoins.iOS.UI.Services;
using Softjourn.SJCoins.iOS.UI.Delegates;

namespace Softjourn.SJCoins.iOS.UI.Controllers
{
	[Register("DetailViewController")]
	public partial class DetailViewController : BaseViewController<DetailPresenter>, IDetailView
	{
        public const float pageHeightCoefficient = 1.85f;

		#region Properties
		private int productId { get; set; }

		private Product currentProduct;
		private List<UIViewController> pages;
		private UIPageViewController pageViewController;
		private PageViewDataSource pageDataSource;
		private PageViewDelegate pageDelegate;
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
            var headerHeight = new SizeHelper().DetailHeaderHeight(TableView.Frame.Height);
            TableView.TableHeaderView.Frame = new CGRect(0, 0, TableView.Frame.Width, headerHeight);
			pageViewController.View.Frame = new CGRect(0, 0, TableView.TableHeaderView.Frame.Width, TableView.TableHeaderView.Frame.Height * 0.88f);
		}

		#endregion

		#region BaseViewController
		public override void AttachEvents()
		{
			base.AttachEvents();
			FavoriteButton.Clicked += FavoriteButtonClicked;
			BuyButton.TouchUpInside += BuyButtonClicked;
			pageDelegate.CurrentIndexChanged += ImageIndexChanged;
		}

		public override void DetachEvents()
		{
			FavoriteButton.Clicked -= FavoriteButtonClicked;
			BuyButton.TouchUpInside -= BuyButtonClicked;
			pageDelegate.CurrentIndexChanged -= ImageIndexChanged;
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
		private void ConfigurePageWith(Product product)
		{
			Title = product.Name;
			PriceLabel.Text = product.Price.ToString();
			ConfigureFavoriteImage(product.IsProductFavorite);
		}

		private void ConfigureTableView()
		{
			tableSource = new DetailViewSource(currentProduct);
			TableView.Source = tableSource;
			TableView.EstimatedRowHeight = 50;
			TableView.RowHeight = UITableView.AutomaticDimension;
		}

		private void ConfigureFavoriteImage(bool isFavorite)
		{
            if (isFavorite)
                FavoriteButton.Image = UIImage.FromBundle(ImageConstants.HeartFilled);
			else
                FavoriteButton.Image = UIImage.FromBundle(ImageConstants.Heart);
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
			pageDelegate = new PageViewDelegate(pages);

			pageViewController.DataSource = pageDataSource;
			pageViewController.Delegate = pageDelegate;
			
            var defaultViewController = new UIViewController[] { pages.ElementAt(0) };
			pageViewController.SetViewControllers(defaultViewController, UIPageViewControllerNavigationDirection.Forward, false, null);

            var height = TableView.TableHeaderView.Frame.Height;
			HeaderView.AddSubview(this.pageViewController.View);
		}

		private void ConfigurePageControl()
		{
			if (pages.Count > 1)
			{
				PageControl.Pages = pages.Count;
				PageControl.CurrentPage = 0;
				HeaderView.BringSubviewToFront(PageControl);
				PageControl.Hidden = false;
				pageViewController.View.UserInteractionEnabled = true;
			}
			else
			{
				PageControl.Hidden = true;
				pageViewController.View.UserInteractionEnabled = false;
			}
		}
		#endregion

		#region Event handlers
		private void FavoriteButtonClicked(object sender, EventArgs e)
		{
            // Handle clicking on the Favorite button
            LoaderService.Show("Loading");
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
		#endregion
	}
}
