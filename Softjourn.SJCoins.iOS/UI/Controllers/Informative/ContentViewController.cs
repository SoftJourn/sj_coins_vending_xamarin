using System;

using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers.Informative
{
	public partial class ContentViewController : UIViewController
	{
		//Properties
		public int pageIndex = 0;
		public  
		public string titleText;
		public string descriptionText;

		//Constructor
		public ContentViewController(IntPtr handle) : base(handle)
		{
		}

		//Life cycle
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			titleLabel.Text = titleText;
			descriptionLabel.Text = descriptionText;
		}
	}
}
