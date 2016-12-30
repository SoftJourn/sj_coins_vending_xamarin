using Foundation;
using System;
using UIKit;

namespace Softjourn.SJCoins.iOS
{
    public partial class ContentViewController : UIViewController
    {
        public int pageIndex = 0;
		public string titleText;
		public string imageFile;

		public ContentViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			//imageView.Image = UIImage.FromBundle(imageFile);
			//label.Text = titleText;
		}
    }
}