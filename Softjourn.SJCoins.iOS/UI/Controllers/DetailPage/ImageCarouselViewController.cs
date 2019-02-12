using System;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.iOS.UI.Delegates.DetailPage;
using Softjourn.SJCoins.iOS.UI.Sources.DetailPage;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers.DetailPage
{
    [Register("ImageCarouselViewController")]
    public partial class ImageCarouselViewController: UIViewController
    {
        public Product CurrentProduct { get; set; }
        public int CurrentImage { get; set; }
		public event EventHandler<int> VisibleItem;

		private ImageCarouselViewSource collectionSource;
		private ImageCarouselViewFlowLayoutDelegate collectionDelegate;

		public ImageCarouselViewController(IntPtr handle) : base(handle)
        {
		}

		#region Controller Life cycle

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			ConfigureImageCollection();
            ConfigurePageControl();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
            BackButton.Clicked += BackButtonClicked;
			collectionDelegate.VisibleItem += ImageIndexChanged;
            UIApplication.SharedApplication.SetStatusBarHidden(true, UIStatusBarAnimation.Fade);

            NavigationController.NavigationBar.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
            NavigationController.NavigationBar.ShadowImage = new UIImage();
            NavigationController.NavigationBar.Translucent = true;
            NavigationController.View.BackgroundColor = UIColor.Clear;
		}

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
			PageControl.CurrentPage = CurrentImage;
        }

        public override void ViewWillDisappear(bool animated)
        {
            BackButton.Clicked -= BackButtonClicked;
			collectionDelegate.VisibleItem -= ImageIndexChanged;
            UIApplication.SharedApplication.SetStatusBarHidden(false, UIStatusBarAnimation.Fade);

            NavigationController.NavigationBar.SetBackgroundImage(null, UIBarMetrics.Default);
            NavigationController.NavigationBar.ShadowImage = null;
            NavigationController.View.BackgroundColor = null;
			base.ViewWillDisappear(animated);
        }

		#endregion

		#region Private methods

		private void ConfigureImageCollection()
		{
			collectionSource = new ImageCarouselViewSource
			{
				Images = CurrentProduct.ImagesFullUrls
			};

            collectionDelegate = new ImageCarouselViewFlowLayoutDelegate
			{
                Images = CurrentProduct.ImagesFullUrls
			};

			CollectionView.DataSource = collectionSource;
			CollectionView.Delegate = collectionDelegate;
		}

		private void ConfigurePageControl()
		{
			if (CurrentProduct.ImagesFullUrls.Count > 1)
			{
				PageControl.Pages = CurrentProduct.ImagesFullUrls.Count;
				PageControl.CurrentPage = 0;
                View.BringSubviewToFront(PageControl);
				PageControl.Hidden = false;
			}
			else
				PageControl.Hidden = true;
		}

        #endregion

		#region Event handlers

		private void BackButtonClicked(object sender, EventArgs e)
		{
            PresentingViewController.DismissViewController(true, null);
		}

		private void ImageIndexChanged(object sender, int currentIndex)
		{
			// Change dot on Page Control
			PageControl.CurrentPage = currentIndex;
            VisibleItem?.Invoke(this, currentIndex);
		}

		#endregion

		protected override void Dispose(bool disposing)
		{
			System.Diagnostics.Debug.WriteLine(string.Format("{0} disposed", GetType()));
			base.Dispose(disposing);
		}
	}
}
