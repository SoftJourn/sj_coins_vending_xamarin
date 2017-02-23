using System;
using CoreFoundation;
using Foundation;
using Social;
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
		private string amount 
		{ 
			get { return AmountTexfield.Text; } 
		}

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
			AmountTexfield.EditingChanged += AmountTextFieldChanged;
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
			AmountTexfield.EditingChanged -= AmountTextFieldChanged;
			GenerateButton.TouchUpInside -= GenerateButtonClickHandler;
			DoneButton.Clicked -= DoneButtonClickHandler;
			QRCodeImage.RemoveGestureRecognizer(qrcodeImageTap);
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
			ErrorLabel.Text = message;
			ErrorLabel.Hidden = false;
		}

		public void ShowImage(string image)
		{
			// Show QRCode after success generating
			QRCodeImage.Image = new QRCodeHelper().GenerateQRImage(image, 700, 700);
			// Clear texfield
			AmountTexfield.Text = "";
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
			ErrorLabel.Hidden = true;
			AmountTexfield.Hidden = true;
			GenerateButton.Hidden = true;
			QRCodeImage.Hidden = true;
			//Execute scaning
			ScanQRCode();
		}

		private void ConfigureGenerateMode()
		{
			BalanceLabel.Text = "Your balance is " + Presenter.GetBalance().ToString() + " coins"; 
			ErrorLabel.Hidden = true;
			AmountTexfield.Hidden = false;
			AmountTexfield.ShouldReturn = TextFieldShouldReturn;
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

		private bool TextFieldShouldReturn(UITextField textField)
		{
			textField.ResignFirstResponder();
			return true;
		}

		// -------------------- Event handlers --------------------
		private void AmountTextFieldChanged(object sender, EventArgs e)
		{
			ErrorLabel.Text = "";
			Presenter.ValidateAmount(AmountTexfield.Text);
		}

		private void GenerateButtonClickHandler(object sender, EventArgs e)
		{
			// Handle clicking on the Generate button

			Presenter.GenerateCode(amount);
			// Hide keyboard
			AmountTexfield.ResignFirstResponder();
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
