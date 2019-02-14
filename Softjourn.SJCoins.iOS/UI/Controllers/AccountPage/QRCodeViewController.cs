using System;
using Foundation;
using Softjourn.SJCoins.Core.Common.Exceptions;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.iOS.General.Helper;
using Softjourn.SJCoins.iOS.UI.Delegates;
using Softjourn.SJCoins.iOS.UI.Services;
using UIKit;
using ZXing.Mobile;

namespace Softjourn.SJCoins.iOS.UI.Controllers.AccountPage
{
	[Register("QRCodeViewController")]
	public partial class QRCodeViewController : BaseViewController<QrPresenter>, IQrView
	{
		private const string Scan = "Scan";
		private const string Generate = "Generate";
		private const string notValidCode = "Not valid QR code.";

		private UIImage qrcode;
		private string InitialParameter { get; set; }
		private string Amount => AmountTexfield.Text;

        private MobileBarcodeScanner scanner; 
		private UITapGestureRecognizer qrcodeImageTap;
		private AmountTextFieldDelegate textFieldDelegate;

		public QRCodeViewController(IntPtr handle) : base(handle)
		{
		}

		public void SetInitialParameter(object initialParameter)
		{
			if (initialParameter is string parameter)
				InitialParameter = parameter;
		}

		#region Controller Life cycle

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			ConfigurePageWith(InitialParameter);
		}

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
			GenerateButton.Hidden = false;
            BalanceLabel.Hidden = false;
            CoinLogo.Hidden = false;
        }

		#endregion

		#region BaseViewController

		public override void AttachEvents()
		{
			base.AttachEvents();
			AmountTexfield.EditingChanged += AmountTextFieldChanged;
			GenerateButton.TouchUpInside += GenerateButtonClickHandler;
			DoneButton.Clicked += DoneButtonClickHandler;
			
			// Add tap gesture to QRCode image
			qrcodeImageTap = new UITapGestureRecognizer(QRCodeImageTapHandler);
			QRCodeImage.AddGestureRecognizer(qrcodeImageTap);
		}

		public override void DetachEvents()
		{
			AmountTexfield.EditingChanged -= AmountTextFieldChanged;
			GenerateButton.TouchUpInside -= GenerateButtonClickHandler;
			DoneButton.Clicked -= DoneButtonClickHandler;
			QRCodeImage.RemoveGestureRecognizer(qrcodeImageTap);
			base.DetachEvents();
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
			qrcode = new QRCodeHelper().GenerateQRImage(image, 800, 800);
			QRCodeImage.Image = qrcode;
			QRCodeImage.Hidden = false;
			// Save qrcode
			SaveImageToPhotoAlbum(qrcode);
			// Clear texfield
			AmountTexfield.Text = string.Empty;
		}

		#endregion

		#region Private methods

		private void ConfigurePageWith(string parameter)
		{
			BalanceLabel.Text = $"Your balance is {Presenter.GetBalance()}";
			ErrorLabel.Hidden = true;
			QRCodeImage.Hidden = true;

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
			// Execute scaning
			ScanQRCode();
			// Prepare Controller to ReScaning
			AmountTexfield.Hidden = true;
			GenerateButton.SetTitle("Scan Again", UIControlState.Normal);	
		}

		private void ConfigureGenerateMode()
		{
			GenerateButton.Hidden = false;
			BalanceLabel.Hidden = false;
            CoinLogo.Hidden = false;

			textFieldDelegate = new AmountTextFieldDelegate();
			AmountTexfield.Hidden = false;
			AmountTexfield.KeyboardType = UIKeyboardType.NumberPad;
			AmountTexfield.Delegate = textFieldDelegate;
		}

		private async void ScanQRCode()
		{
			try
			{
				await Presenter.CheckPermission();

				scanner = new MobileBarcodeScanner(this);

                var result = await scanner.Scan();
				if (result != null)
				{
					var cashObject = new QRCodeHelper().ConvertScanResult(result);
					Presenter.ScanCodeIOS(cashObject);
				}
			}
			catch (JsonReaderExceptionCustom)
			{
				new AlertService().ShowToastMessage(notValidCode);
			}
			catch (CameraException e)
			{
				new AlertService().ShowToastMessage(e.ToString());
			}
		}

		private void SaveImageToPhotoAlbum(UIImage image)
		{
			image.SaveToPhotosAlbum((img, error) =>
			{
				if (error != null)
					Console.WriteLine("error saving image: {0}", error);
				// TODO show success message on view 
				else
					Console.WriteLine("image saved to photo album");
				// TODO show success message on view 
			});
		}

		private void PresentSharedSheet()
		{
			var activityController = new UIActivityViewController(new NSObject[] { qrcode }, null);
			PresentViewController(activityController, true, null);
		}

		#endregion

		#region Event handlers

		private void AmountTextFieldChanged(object sender, EventArgs e)
		{
			ErrorLabel.Text = string.Empty;
			Presenter.ValidateAmount(AmountTexfield.Text);
		}

		private void GenerateButtonClickHandler(object sender, EventArgs e)
		{
			if (InitialParameter == Scan)
			{
				// Execute scanning
				ScanQRCode();
			}
			else if (InitialParameter == Generate)
			{
				// Handle clicking on the Generate button
				Presenter.GenerateCode(Amount);
				// Hide keyboard
				AmountTexfield.ResignFirstResponder();
			}
		}

        private void DoneButtonClickHandler(object sender, EventArgs e) =>
            DismissViewController(true, completionHandler: null); // Handle clicking on the Done button

        private void QRCodeImageTapHandler(UITapGestureRecognizer gestureRecognizer) => PresentSharedSheet(); // Handle tapping on the QRCode image

        #endregion
    }
}
