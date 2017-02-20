using System;
using Foundation;
using UIKit;

namespace Softjourn.SJCoins.iOS
{
	[Register("QRCodeViewController")]
	public partial class QRCodeViewController : UIViewController
	{
		#region Properties
		UITapGestureRecognizer qrcodeImageTap;
		#endregion

		#region Constructor
		public QRCodeViewController(IntPtr handle) : base(handle)
		{
		}
		#endregion

		#region Controller Life cycle
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			// Attach 
			GenerateButton.TouchUpInside += GenerateButtonClickHandler;
			DoneButton.Clicked += DoneButtonClickHandler;

			// Add tap gesture to QRCode image
			qrcodeImageTap = new UITapGestureRecognizer(QRCodeImageTapHandler);
			QRCodeImage.AddGestureRecognizer(qrcodeImageTap);
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
		}

		public override void ViewWillDisappear(bool animated)
		{
			// Detach
			GenerateButton.TouchUpInside -= GenerateButtonClickHandler;
			DoneButton.Clicked -= DoneButtonClickHandler;
			base.ViewWillDisappear(animated);
		}
		#endregion

		#region Private methods
		private void GenerateMode()
		{
			AmountTexfield.Hidden = false;
			GenerateButton.Hidden = false;
			QRCodeImage.Hidden = false;
		}

		private void ScanMode()
		{
			AmountTexfield.Hidden = true;
			GenerateButton.Hidden = true;
			QRCodeImage.Hidden = true;
		}

		// -------------------- Event handlers --------------------
		private void GenerateButtonClickHandler(object sender, EventArgs e)
		{
			// Handle clicking on the Generate button
			//Presenter.OnFavoriteClick(currentProduct);
		}

		private void DoneButtonClickHandler(object sender, EventArgs e)
		{
			// Handle clicking on the Done button
			DismissViewController(true, completionHandler: null);
		}

		private void QRCodeImageTapHandler(UITapGestureRecognizer gestureRecognizer)
		{
			// Handle tapping on the QRCode image

		}
		// -------------------------------------------------------- 
		#endregion
	}
}
