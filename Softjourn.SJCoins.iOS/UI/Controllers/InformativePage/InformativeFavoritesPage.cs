using System;
using Foundation;
using UIKit;

namespace Softjourn.SJCoins.iOS
{
	[Register("InformativeFavoritesPage")]
    public partial class InformativeFavoritesPage: UIViewController
    {
		#region Properties
        public event EventHandler GotItButtonTapped;
		#endregion

		public InformativeFavoritesPage(IntPtr handle) : base(handle)
        {
        }

		#region Controller Life cycle
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
            ConfigureGotItButton();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			GotItButton.TouchUpInside += GotItButtonTapped;
		}

		public override void ViewWillDisappear(bool animated)
		{
            if (GotItButtonTapped != null) {
				GotItButton.TouchUpInside -= GotItButtonTapped;
			}
			base.ViewWillDisappear(animated);
		}
		#endregion Controller Life cycle

        private void ConfigureGotItButton()
        {
            GotItButton.Layer.CornerRadius = 5;
            GotItButton.Layer.BorderWidth = 0.7f;
            GotItButton.Layer.BorderColor = UIColor.White.CGColor;
        }
	}
}
