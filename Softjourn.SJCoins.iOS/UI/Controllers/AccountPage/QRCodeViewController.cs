using System;
using Foundation;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.iOS.General.Helper;
using UIKit;
using ZXing.Mobile;

namespace Softjourn.SJCoins.iOS.UI.Controllers.AccountPage
{
	[Register("QRCodeViewController")]
	public partial class QRCodeViewController : BaseViewController<QrPresenter>, IQrView
	{
		#region Constants
		private const string Scan = "Scan";
		private const string Generate = "Generate";
		#endregion

		#region Properties
		private string initialParameter { get; set; }

		MobileBarcodeScanner scanner; 

		UITapGestureRecognizer qrcodeImageTap;
		#endregion

		#region Constructor
		public QRCodeViewController(IntPtr handle) : base(handle)
		{
		}

		public void SetInitialParameter(object initialParameter)
		{
			if (initialParameter is string)
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

		#region IQrView implementation
		public void UpdateBalance(string remain)
		{
			//Update balance after success debiting funds
			BalanceLabel.Text = "Your balance is " + remain + " coins";
			BalanceLabel.Hidden = false;
		}

		public void SetEditFieldError(string message)
		{
			// Show error when amount more than balance
			throw new NotImplementedException();
		}

		public void ShowImage(string image)
		{
			// Show QRCode after success generating
			throw new NotImplementedException();
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

		private void ConfigureScanMode()
		{
			BalanceLabel.Hidden = true;
			AmountTexfield.Hidden = true;
			GenerateButton.Hidden = true;
			QRCodeImage.Hidden = true;
			ScanQRCode();
		}

		private void ConfigureGenerateMode()
		{
			BalanceLabel.Text = "Your balance is " + Presenter.GetBalance().ToString() + " coins";
			AmountTexfield.Hidden = false;
			GenerateButton.Hidden = false;
			QRCodeImage.Hidden = false;
		}

		private async void ScanQRCode()
		{
			await Presenter.CheckPermission();

			scanner = new ZXing.Mobile.MobileBarcodeScanner(this);
			var result = await scanner.Scan();

			if (result != null)
			{
				var cashObject = new QRCodeHelper().ConvertScanResult(result);
				Presenter.ScanCodeIOS(cashObject);
			}
		}

		// -------------------- Event handlers --------------------
		private void GenerateButtonClickHandler(object sender, EventArgs e)
		{
			// Handle clicking on the Generate button
			// take ammount from edit field
			//Presenter.GetMoney();
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
