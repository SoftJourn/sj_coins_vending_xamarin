using System;
using CoreAnimation;
using Foundation;
using SDWebImage;
using Softjourn.SJCoins.Core.API.Model.AccountInfo;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers.Main
{
	[Register("AccountViewController")]
	public partial class AccountViewController : BaseViewController<AccountPresenter>, IAccountView
	{
		#region Properties
		#endregion

		#region Constructor
		public AccountViewController(IntPtr handle) : base(handle)
		{
		}
		#endregion

		#region Controller Life cycle
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			ConfigureAvatarImage(AvatarImage);
			Presenter.OnStartLoadingPage();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);


		}
		#endregion

		#region IAccountView implementation
		public void SetAccountInfo(Account account)
		{
			if (account != null)
			{
				NameLabel.Text = account.Name + " " + account.Surname;
				AmountLabel.Text = account.Amount.ToString() + " coins";

				AvatarImage.SetImage(url: new NSUrl("https://sjcoins-testing.softjourn.if.ua/vending/v1/products/100/image.jpeg"), placeholder: UIImage.FromBundle("Placeholder.png"));
			}
		}
		#endregion

		#region Private methods
		private void ConfigureAvatarImage(UIImageView imageView)
		{
			// Add tap gesture to avatar image
			imageView.AddGestureRecognizer(new UITapGestureRecognizer(OnAvatarTap));

			// Make image rounded
			CALayer imageCircle = imageView.Layer;
			imageCircle.CornerRadius = 60;
			imageCircle.BorderWidth = 0.2f;
			imageCircle.MasksToBounds = true;
		}

		private void OnAvatarTap()
		{
			//Presenter shows action sheet
			Presenter.OnPhotoClicked();
		}

		public void ImageAcquired(byte[] receipt)
		{
			// Set image to imageView
			AvatarImage.Image = UIImage.LoadFromData(NSData.FromArray(receipt));
		}
		#endregion

		#region BaseViewController -> IBaseView implementation
		#endregion
	}
}
