using System;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers.Informative
{
	public partial class InformativeViewController : UIViewController
	{
		//Properties
		private UIPageViewController _pageContainer;
		private InformativeDataSource _dataSource;

		//Constructor
		public InformativeViewController(IntPtr handle, UIViewController[] pages) : base (handle)
		{
			_dataSource = new InformativeDataSource(pages: pages);
		}

		//Life cycle
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			//Create UIPageViewController and show first page
			_pageContainer = new UIPageViewController(UIPageViewControllerTransitionStyle.Scroll,
			                                          UIPageViewControllerNavigationOrientation.Horizontal);
			_pageContainer.View.Frame = View.Bounds;
			View.AddSubview(_pageContainer.View);
			_pageContainer.DataSource = _dataSource;
			_pageContainer.SetViewControllers(new UIViewController[] { _dataSource._pages[0] as UIViewController }, UIPageViewControllerNavigationDirection.Forward, false, null);
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			Console.WriteLine("InformativeVC disposed");
		}

		//private void ShowController(UIViewController controller)
		//{
		//	_pageContainer.SetViewControllers(controller[], direction: UIPageViewControllerNavigationDirection.Forward, animated: true, completionHandler: null);	
		//}
	}
}

