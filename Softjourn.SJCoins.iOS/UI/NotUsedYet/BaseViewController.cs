using System;

using UIKit;

namespace Softjourn.SJCoins.iOS
{
	public partial class BaseViewController : UIViewController
	{
		public BaseViewController() : base("BaseViewController", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		protected void connectionVerification()
		{
			
		}



	}
}

