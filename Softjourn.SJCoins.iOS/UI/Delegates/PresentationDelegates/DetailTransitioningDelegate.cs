﻿using Softjourn.SJCoins.iOS.UI.Controllers.DetailPage;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Delegates.PresentationDelegates
{
    public class DetailTransitioningDelegate : UIViewControllerTransitioningDelegate // NOT USED !!!!!!!
    {
        public override UIPresentationController GetPresentationControllerForPresentedViewController(UIViewController presentedViewController, UIViewController presentingViewController, UIViewController sourceViewController)
        {
            return new DetailViewPresentationController(presentedViewController, presentingViewController);
        }
    }
}
