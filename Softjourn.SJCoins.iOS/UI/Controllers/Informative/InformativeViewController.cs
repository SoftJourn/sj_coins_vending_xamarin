using System;

using UIKit;

namespace Softjourn.SJCoins.iOS
{
	public partial class InformativeViewController : UIViewController
	{
		//Properties
		private UIPageViewController _pageContainer;

		//Life cycle
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			_pageContainer = new UIPageViewController();				
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		//Actions

	}
}

