using Foundation;
using UIKit;
using Softjourn.SJCoins.iOS.General.Constants;
using BigTed;
using HockeyApp.iOS;

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
			ConfigureHockeyAppCrashAnalitics();
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
            ProgressHUD.Shared.HudBackgroundColour = UIColorConstants.HudBackgroundColor;
            ProgressHUD.Shared.HudForegroundColor = UIColorConstants.ProductNameColor;
		}

		private void ConfigureHockeyAppCrashAnalitics()
		{
			var manager = BITHockeyManager.SharedHockeyManager;
			manager.Configure("9e94cec1cac44720ba336059aa00f430"); // key from hockeyapp.net
			manager.CrashManager.CrashManagerStatus = BITCrashManagerStatus.AutoSend;
   			manager.StartManager();
   			manager.Authenticator.AuthenticateInstallation(); // This line is obsolete in crash only builds
		}
    }
}
