using Foundation;
using System;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers
{
    public partial class LoginViewController : UIViewController
    {
		//Constructor
        public LoginViewController (IntPtr handle) : base (handle)
        {
        }

		//Life cycle
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			loginErrorLabel.Hidden = true;
			passwordErrorLabel.Hidden = true;
		}

		//Actions
		partial void LoginButton_TouchUpInside(UIButton sender)
		{
			Console.WriteLine("sjdhfjkshgjksdhgjksdhgjkhdsjkghdsjkghdjksghjk sdgsdhgjkdhsgjk");
		}
	}
}