using System;
using Foundation;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers.Informative
{
	[Register("ContentViewController")]
	public partial class ContentViewController : UIViewController
	{
		//Properties
		public int Index = 0;
		public new string Title;

		//Constructor
		public ContentViewController(IntPtr handle) : base(handle)
		{
		}

		//Life cycle
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			Image.BackgroundColor = UIColor.Red.ColorWithAlpha(0.7f);
			View.BackgroundColor = UIColor.Gray.ColorWithAlpha(0.7f);
			TitleLabel.Text = Title;
		}
	}
}
