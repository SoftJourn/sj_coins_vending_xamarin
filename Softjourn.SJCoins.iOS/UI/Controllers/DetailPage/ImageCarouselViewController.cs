using System;
using CoreGraphics;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers
{
    [Register("ImageCarouselViewController")]
    public partial class ImageCarouselViewController: UIViewController, IDisposable
    {
		#region Properties
        public Product CurrentProduct { get; set; }
        public int CurrentImage { get; set; }
		public event EventHandler<int> VisibleItem;

		private ImageCarouselViewSource collectionSource;
		private ImageCarouselViewFlowLayoutDelegate collectionDelegate;
		#endregion

		#region Constructor
		public ImageCarouselViewController(IntPtr handle) : base(handle)
        {
		}
		#endregion

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
            BackButton.TouchUpInside += BackButtonClicked;
			collectionDelegate.VisibleItem += ImageIndexChanged;
			UIApplication.SharedApplication.SetStatusBarHidden(true, UIStatusBarAnimation.Fade);
		}

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            CollectionView.SetContentOffset(new CGPoint(CurrentImage * SizeHelper.mainBounds.Width, 0.0), false);
			PageControl.CurrentPage = CurrentImage;
        }

        public override void ViewWillDisappear(bool animated)
        {
			BackButton.TouchUpInside -= BackButtonClicked;
			collectionDelegate.VisibleItem -= ImageIndexChanged;
            UIApplication.SharedApplication.SetStatusBarHidden(false, UIStatusBarAnimation.Fade);
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
			{
				PageControl.Hidden = true;
			}
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
			System.Diagnostics.Debug.WriteLine(String.Format("{0} disposed", this.GetType()));
			base.Dispose(disposing);
		}
	}
}
