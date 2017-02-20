using System;
using Foundation;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.iOS.General.Constants;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers.AccountPage
{
	[Register("QRCodeViewController")]
	public partial class QRCodeViewController : BaseViewController<QrPresenter>, IQrView
	{
		#region Constants
		private const string Scan = "ScanMode";
		private const string Generate = "GenerateMode";
		#endregion

		#region Properties
		private string initialParameter { get; set; }

		UITapGestureRecognizer qrcodeImageTap;
		#endregion

		#region Constructor
		public QRCodeViewController(IntPtr handle) : base(handle)
		{
		}

		public void SetInitialParameter(object productId)
		{
			if (productId is string)
			{
				this.initialParameter = (string)initialParameter;
			}
		}
		#endregion

		#region Controller Life cycle
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			ConfigurePageWith(initialParameter);
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

		#region IAccountView implementation
		public void UpdateBalance(string remain)
		{
			//Update balance after success debiting funds
			BalanceLabel.Text = remain;
		}
		#endregion

		#region Private methods
		private void ConfigurePageWith(string parameter)
		{
			switch (parameter)
			{
				case Scan:
					ConfigureScanMode();
					break;
				case Generate:
					ConfigureGenerateMode();
					break;
			}
		}

		private void ConfigureGenerateMode()
		{
			AmountTexfield.Hidden = false;
			GenerateButton.Hidden = false;
			QRCodeImage.Hidden = false;
		}

		private void ConfigureScanMode()
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
