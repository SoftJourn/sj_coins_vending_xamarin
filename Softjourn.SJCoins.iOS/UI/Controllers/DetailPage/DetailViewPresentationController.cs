using UIKit;
using CoreGraphics;
using System;

namespace Softjourn.SJCoins.iOS
{
    public class DetailViewPresentationController: UIPresentationController // NOT USED !!!!!!!
    {
        #region Properties
        private UIView dimmingView;

		public override CGRect FrameOfPresentedViewInContainerView
		{
            get { return FrameOfPresentedView(); }
		}
		#endregion

		#region Constructor
		public DetailViewPresentationController(UIViewController presentedViewController, UIViewController presentingViewController) : base(presentedViewController, presentingViewController)
        {
            SetupDimmingView();
        }
        #endregion

        #region Public methods
        public override void PresentationTransitionWillBegin()
        {
            ContainerView?.InsertSubview(dimmingView, 0);

			// TODO //NSConstraints !!!!!!

			ChangeDimmingViewAlpha(1.0f);
        }

        public override void DismissalTransitionWillBegin()
        {
            ChangeDimmingViewAlpha(0.0f);
        }

        public override void ContainerViewWillLayoutSubviews()
        {
            PresentedView.Frame = FrameOfPresentedViewInContainerView;
        }

        public override CGSize GetSizeForChildContentContainer(IUIContentContainer contentContainer, CGSize parentContainerSize)
        {
            return new CGSize(parentContainerSize.Width, parentContainerSize.Height * (2.0f / 3.0f));
        }
		#endregion

		#region Private methods
		private CGRect FrameOfPresentedView()
        {
            var presentedFrame = new CGRect
            {
                Size = GetSizeForChildContentContainer(PresentedViewController, ContainerView.Bounds.Size)
            };
            presentedFrame.Y = ContainerView.Frame.Height * (1.0f / 2.0f);
            return presentedFrame;
        }

        private void SetupDimmingView()
        {
            dimmingView = new UIView
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = UIColor.White.ColorWithAlpha(0.5f),
                Alpha = 0
            };

			Action tapAction = () => { PresentingViewController.DismissViewController(true, null); };
			var tapGestureRecognizer = new UITapGestureRecognizer(tapAction) { };
			dimmingView.AddGestureRecognizer(tapGestureRecognizer);
        }

        private void ChangeDimmingViewAlpha(nfloat alpha)
        {
			var coordinator = PresentedViewController.GetTransitionCoordinator();
			if (coordinator != null)
				coordinator.AnimateAlongsideTransition(_ => { dimmingView.Alpha = alpha; }, null);
			else
				dimmingView.Alpha = alpha;
        }
		#endregion

	}
}
