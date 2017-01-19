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
		//public new string LogoString;

		//Constructor
		public ContentViewController(IntPtr handle) : base(handle)
		{
		}

		//Life cycle
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			TitleLabel.Text = Title;
			//UIImage image = UIImage.FromBundle(LogoString);

			//Console.WriteLine("sdf");
			//Image.Image = UIImage.FromBundle(ImageLogo1);
		}
	}
}
