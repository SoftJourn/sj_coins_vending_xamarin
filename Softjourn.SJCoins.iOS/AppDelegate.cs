using Foundation;
using UIKit;
using Softjourn.SJCoins.iOS.Bootstraper;
using Softjourn.SJCoins.iOS.General.Constants;
using BigTed;

namespace Softjourn.SJCoins.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations

        public override UIWindow Window { get; set; }
		public UIViewController VisibleViewController { get; set; }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
			InitIoC();
			InitInitialViewControllerManually();
			ConfigureProgressHUD();
			return true;
        }

		private void InitIoC()
		{
			new Bootstraper.Bootstraper().Init();
		}

		private void InitInitialViewControllerManually()
		{
			Window = new UIWindow(UIScreen.MainScreen.Bounds);
			var storyboard = UIStoryboard.FromName(StoryboardConstants.StoryboardLogin, null);
			var controller = storyboard.InstantiateViewController(StoryboardConstants.InitialViewController);
			Window.RootViewController = controller;
			Window.MakeKeyAndVisible();
		}

		private void ConfigureProgressHUD()
		{
			ProgressHUD.Shared.HudBackgroundColour = UIColor.Black;
			ProgressHUD.Shared.HudForegroundColor = UIColor.White;
		}
    }
}
