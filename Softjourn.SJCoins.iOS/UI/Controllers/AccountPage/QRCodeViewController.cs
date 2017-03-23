using System;
using System.Linq;
using Foundation;
using Softjourn.SJCoins.Core.Exceptions;
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
		private UIImage qrcode;
		private string initialParameter { get; set; }
		private string amount
		{ 
			get { return AmountTexfield.Text; } 
		}

		MobileBarcodeScanner scanner; 
		UITapGestureRecognizer qrcodeImageTap;
		private AmountTextFieldDelegate textFieldDelegate;
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
			AmountTexfield.Text = "";
		}
		#endregion

		#region Private methods
		private void ConfigurePageWith(string parameter)
		{
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
			BalanceLabel.Hidden = true;
			AmountTexfield.Hidden = true;
			GenerateButton.Hidden = true;
			//Execute scaning
			ScanQRCode();
		}

		private void ConfigureGenerateMode()
		{
			BalanceLabel.Text = "Your balance is " + Presenter.GetBalance().ToString() + " coins"; 
			textFieldDelegate = new AmountTextFieldDelegate();
			AmountTexfield.Hidden = false;
			AmountTexfield.KeyboardType = UIKeyboardType.NumberPad;
			AmountTexfield.Delegate = textFieldDelegate;
			GenerateButton.Hidden = false;
		}

		private async void ScanQRCode()
		{
			try
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
			catch (CameraException e)
			{
				// TODO show exeption
				//AlertService.ShowToastMessage(e.ToString());
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
			PresentSharedSheet();
		}
		// -------------------------------------------------------- 
		#endregion
	}
}
