using System;
using CoreGraphics;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers.DetailPage
{
    public class DetailViewPresentationController: UIPresentationController // NOT USED !!!!!!!
    {
        private UIView dimmingView;

		public override CGRect FrameOfPresentedViewInContainerView => FrameOfPresentedView();

		public DetailViewPresentationController(UIViewController presentedViewController, UIViewController presentingViewController) 
            : base(presentedViewController, presentingViewController)
        {
            SetupDimmingView();
        }

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

		#region Private methods

		private CGRect FrameOfPresentedView()
        {
            var presentedFrame = new CGRect
            {
                Size = GetSizeForChildContentContainer(PresentedViewController, ContainerView.Bounds.Size),
                Y = ContainerView.Frame.Height * (1.0f / 2.0f)
            };

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

            void TapAction()
            {
                PresentingViewController.DismissViewController(true, null);
            }

            var tapGestureRecognizer = new UITapGestureRecognizer(TapAction) { };
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
