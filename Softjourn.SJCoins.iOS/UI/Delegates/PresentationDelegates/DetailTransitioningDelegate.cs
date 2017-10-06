using UIKit;

namespace Softjourn.SJCoins.iOS
{
    public class DetailTransitioningDelegate: UIViewControllerTransitioningDelegate // NOT USED !!!!!!!
	{
        public override UIPresentationController GetPresentationControllerForPresentedViewController(UIViewController presentedViewController, UIViewController presentingViewController, UIViewController sourceViewController)
        {
            return new DetailViewPresentationController(presentedViewController, presentingViewController);
        }

        //public override IUIViewControllerAnimatedTransitioning GetAnimationControllerForPresentedController(UIViewController presented, UIViewController presenting, UIViewController source)
        //{
            
        //}

        //public override IUIViewControllerAnimatedTransitioning GetAnimationControllerForDismissedController(UIViewController dismissed)
        //{
            
        //}
    }
}
